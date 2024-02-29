using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class DesertEagle : WeaponBase
{
    public GameObject muzzleFlashEffect; // 총구 이펙트
    
    public Transform casingSpawnPoint; // 탄피 생성 위치
    public Transform bulletSpawnPoint; // 총알 생성 위치
    
    public AudioClip audioClipTakeOutWeapon; //무기 장착 사운드
    public AudioClip audioClipFire; // 무기 발사 사운드
    public AudioClip audioClipEmpty; // 무기 탄약 없을 때 사운드
    public AudioClip audioClipReload; // 재장전 사운드
    
    private CasingMemoryPool casingMemoryPool; // 탄피 생성 후 활성 관리
    private ImpactMemoryPool impactMemoryPool;//공격 효과 생성 후 활성 관리
    private Camera mainCamera;//광선 발사

    private void Awake()
    {
        base.Setup();
        casingMemoryPool = GetComponent<CasingMemoryPool>();
        impactMemoryPool = GetComponent<ImpactMemoryPool>();
        mainCamera = Camera.main;
        
        //처음 탄창 수는 최대
        weaponSetting.currentMagazine = weaponSetting.maxMagazine;
        //처음 탄 수는 최대
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;
    }
    
    private void OnEnable()
    {
        audioSource.PlayOneShot(audioClipTakeOutWeapon); //무기 장착 사운드
        muzzleFlashEffect.SetActive(false);//총구 이펙트 비활성화
        onMagazineEvent.Invoke(weaponSetting.currentMagazine);//무기 탄창 정보 갱신
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo); //무기 탄 수 정보 갱신
        ResetVariables();
    }
    
    public override void StartWeaponAction(int type = 0)
    {
        if (type == 0 )
        {
            if (weaponSetting.isAutomaticAttack == true)//마우스 왼쪽 클릭, 재장전 중이 아니라면
            {
                StartCoroutine("OnAttackLoop");
            } //연발사격
            else
            {
                OnAttack();
            } //단발사격
        }
    }

    public override void StopWeaponAction(int type = 0)
    {
        if (type == 0)
        {
            StopCoroutine("OnAttackLoop");
        }
        isAttack = false;
    }

    public override void StartReload()
    {
        if (isReload == true || weaponSetting.currentMagazine <=0  || Store.isStore ) return; //재장전 중일 때, 탄창 수가 0이하 일때, 상점 메뉴가 켜졌을 때 재장전 불가능
        StopWeaponAction(); //무기 사용 도중에 재장전 시도하면 사용 중지 후 재장전
        StartCoroutine("OnReload");
    }

    private IEnumerator OnAttackLoop() //무한루프 내부에서 OnAttack()을 매프레임 호출
    {
        while (true)
        {
            OnAttack();
            yield return null;
        }
    }
    public void OnAttack()
    {
        if (Time.time - lastAttackTIme > weaponSetting.attackRate)
        {
            //공격주기가 되어야 공격할 수 있도록 하기 위해 현재 시간 저장
            lastAttackTIme = Time.time;

            //현재 탄약이 없으면 공격 불가
            if (weaponSetting.currentAmmo <= 0)
            {
                if (!(animator.MoveSpeed > 0.5f)) //뛰는 상태가 아닐 때
                    PlaySound(audioClipEmpty); //탄약 없을 때 사운드 재생
                return;
            }

            //공격시 탄약 감소
            weaponSetting.currentAmmo--;
            onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

            //무기 애니메이션 재생
            animator.Play("Fire", -1, 0);

            //총구 이펙트
            StartCoroutine("OnMuzzleFlashEffect");

            //무기 발사 사운드 재생
            PlaySound(audioClipFire);
            
            //탄피생성
            casingMemoryPool.SpawnCasing(casingSpawnPoint.position, transform.right);
            TwoStepRaycast();
        }
    }
    
    private void TwoStepRaycast()
    {
        Ray ray;
        RaycastHit hit;
        Vector3 targetPoint = Vector3.zero;
        //화면 중앙좌표
        ray = mainCamera.ViewportPointToRay(Vector2.one * 0.5f);
        //공격 사거리 안에 부딪히는 오브젝트가 있으면 targetPoint는 광선에 부딪힌 위치
        if (Physics.Raycast(ray, out hit, weaponSetting.attakDistance))
        {
            targetPoint = hit.point;
        }
        else
        {
            // 공격 사거리 안에 부딪히는 오브젝트가 없으면 targetPoint는 최대 사거리
            targetPoint = ray.origin + ray.direction * weaponSetting.attakDistance;
        }

        //목표지점 설정하고 총구를 시작지점으로 해 연산
        Vector3 attackDirection = (targetPoint - bulletSpawnPoint.position).normalized;
        if (Physics.Raycast(bulletSpawnPoint.position, attackDirection, out hit, weaponSetting.attakDistance))
        {
            impactMemoryPool.SpawnImpact(hit);
            
            if (hit.transform.CompareTag("Zombie"))//맞은 지점이 좀비라면
            {
                //좀비 컨트롤러를 가져와 OnDamege 실행
                hit.transform.GetComponent<ZombieController>().OnDamege(weaponSetting.damege);
            }
        }
    }
    
    private IEnumerator OnMuzzleFlashEffect()
    {
        muzzleFlashEffect.SetActive(true);

        yield return new WaitForSeconds(weaponSetting.attackRate * 0.3f);
        
        muzzleFlashEffect.SetActive(false);
    }

    private IEnumerator OnReload()
    {
        isReload = true;
        
        animator.OnReload();
        PlaySound(audioClipReload);

        while (true)
        {
            if (audioSource.isPlaying == false && animator.CurrentAnimationIs("Movement"))
            {//사운드가 재생 중이 아니고 현재 애니메이션이 Movement이면 재장전 애니메이션과 사운드는 종료되었다는 것
                isReload = false;
                weaponSetting.currentMagazine--;//탄창 수 감소
                onMagazineEvent.Invoke(weaponSetting.currentMagazine);//UI 업데이트
                weaponSetting.currentAmmo = weaponSetting.maxAmmo;// 탄알 수 최대 탄수로
                onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);//UI 업데이트
                yield break;
            }

            yield return null;
        }
    }

    private void ResetVariables()
    {
        isReload = false;
        isAttack = false;
    }
    
    public void StoreBullet()
    {
        //처음 탄창 수는 최대
        weaponSetting.currentMagazine = weaponSetting.maxMagazine;
        //처음 탄 수는 최대
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;
        onMagazineEvent.Invoke(weaponSetting.currentMagazine);//UI 업데이트
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);//UI 업데이트
    }
}
