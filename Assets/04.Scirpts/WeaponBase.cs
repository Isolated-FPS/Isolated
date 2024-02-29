using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Main = 0, Sub, Melee, Throw }

[System.Serializable] public class AmmoEvent : UnityEngine.Events.UnityEvent<int, int> { }
[System.Serializable] public class MagazineEvent : UnityEngine.Events.UnityEvent<int> { }

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField] protected WeaponType weaponType; //무기 종류
    [SerializeField] protected WeaponSetting weaponSetting;//무기 설정
    
    protected float lastAttackTIme = 0; //마지막 발사시간 체크
    protected bool isReload = false; //재장전 여부
    protected bool isAttack = false; //공격 여부
    protected AudioSource audioSource; //사운드 재생 컴포넌트
    protected PlayerAnimatorController animator; //애니메이션 재생 제어
    
    //무기의 탄수 정보가 바뀔때마다 외부에 있는 메소드를 자동호출할 수 있도록 이벤트 클래스 생성
    [HideInInspector] public AmmoEvent onAmmoEvent = new AmmoEvent();
    //탄창 정보가 바뀔때마다 외부에 있는 메소드를 자동호출할 수 있도록 이벤트 클래스 생성
    [HideInInspector] public MagazineEvent onMagazineEvent = new MagazineEvent();
   
    //외부에서 필요한 정보를 열람하기 위해 정의한 프로퍼티
    public PlayerAnimatorController Animator => animator; 
    public WeaponName WeaponName => weaponSetting.weaponName;
    public int CurrentMagazine => weaponSetting.currentMagazine;
    public int MaxMagazine => weaponSetting.maxMagazine;

    public abstract void StartWeaponAction(int type = 0);
    public abstract void StopWeaponAction(int type = 0);
    public abstract void StartReload();

    protected void PlaySound(AudioClip clip)
    {
        audioSource.Stop(); //기존 재생중인 사운드 정지
        audioSource.clip = clip; // 새로운 사운드 클립 교체
        audioSource.Play(); // 재생
    }

    protected void Setup()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<PlayerAnimatorController>();
    }
}
