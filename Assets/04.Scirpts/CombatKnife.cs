using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatKnife : WeaponBase
{
    public CombatKnifeCollider combatKnifeCollider;//단검의 충돌범위 제어
  
    public AudioClip audioClipTakeOutWeapon; //무기 장착 사운드
    //public AudioClip audioClipFire; // 무기 발사 사운드
    private void Awake()
    {
        base.Setup();

        //처음 탄창 수는 최대
        weaponSetting.currentMagazine = weaponSetting.maxMagazine;
        //처음 탄 수는 최대
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;
    }
    
    private void OnEnable()
    {
        isAttack = false;
        
        audioSource.PlayOneShot(audioClipTakeOutWeapon); //무기 장착 사운드
       
        onMagazineEvent.Invoke(weaponSetting.currentMagazine);//무기 탄창 정보 갱신
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo); //무기 탄 수 정보 갱신
    }

    public override void StartWeaponAction(int type = 0)
    {
        if (isAttack == true) return;

        // 연속공격
        if (weaponSetting.isAutomaticAttack == true)
        {
            StartCoroutine("OnAttackLoop", type);
        }
        else // 단일공격
        {
            StartCoroutine("OnAttack", type);
        }
    }

    public override void StopWeaponAction(int type = 0)
    {
        isAttack = false;
        StopCoroutine("OnAttackLoop");
    }

    public override void StartReload()
    {
    }

    private IEnumerator OnAttackLoop(int type)
    {
        while (true)
        {
            yield return StartCoroutine("OnAttack", type);
        }
    }

    private IEnumerator OnAttack(int type)
    {
        isAttack = true;
        animator.Play("Fire",-1,0);
        yield return new WaitForEndOfFrame();
        while (true)
        {
            if (animator.CurrentAnimationIs("Movement"))
            {
                isAttack = false;
                
                yield break;
            }

            yield return null;
        }
    }

    public void StartCombatKnifeCollider()
    {
        combatKnifeCollider.StartCollider(weaponSetting.damege);
    }
}
