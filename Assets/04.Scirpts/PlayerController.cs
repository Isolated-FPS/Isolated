using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    //달리기
    public KeyCode keyCodeRun = KeyCode.LeftShift;
    //점프
    public KeyCode keyCodeJump = KeyCode.Space;
    //재장전
    public KeyCode keyCodeReload = KeyCode.R;
    
    public AudioClip audioClipWalk;//걷기 사운드
    public AudioClip audioCliRun;//뛰기 사운드

    public UnityEngine.UI.Image playerBlood;//플레이어 피격 표현
    
    private GameObject SoundManager;
    
    private RotateToMouse rotateToMouse; // 마우스 이동으로 카메라 회전
    private MovementCharacterController movement;// 키보드 입력으로 이동 및 점프
    private Status status;//이동속도 등의 정보
    private AudioSource audioSource;//사운드 제어
    private WeaponBase weapon;//무기를 이용한 공격 제어
    
    private bool playerDie = false;//플레이어 사망 여부
    
    private void Awake()
    {
        Cursor.visible = false; // 마우스 커서 가리기
        Cursor.lockState = CursorLockMode.Locked; //중앙 위치 고정
        rotateToMouse = GetComponent<RotateToMouse>();
        movement = GetComponent<MovementCharacterController>();
        status = GetComponent<Status>();
        audioSource = GetComponent<AudioSource>();
        SoundManager = GameObject.Find("SoundManager");
    }

    private void Update()
    {
        if (Store.isStore == false && SubMenu.isSubMenu == false)
        {
            UpdateRotate();//캐릭터 회전
            UpdateJump();//캐릭터 점프
        }
        UpdateMove();//캐릭터 이동
        UpdateWeaponAction();//캐릭터 무기 액션
        
        if (PlayerHealth.pointHP <= 0 && playerDie == false)//플레이어 체력이 0이하이면
            PlayerDie();
    }

    private void UpdateRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        
        rotateToMouse.UpdateRotate(mouseX, mouseY);
    }
    
    private void UpdateMove()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        if (x != 0 || z != 0) // 이동 중 일때
        {
            bool isRun = false;

            if (z > 0) isRun = Input.GetKey(keyCodeRun);
            //달리기는 앞으로 이동할때만 적용
            movement.MoveSpeed = isRun == true ? status.RunSpeed : status.WalkSpeed;
            weapon.Animator.MoveSpeed = isRun == true ? 1f : 0.5f;
            audioSource.clip = isRun == true ? audioCliRun : audioClipWalk;

            if (audioSource.isPlaying == false)
            {
                audioSource.loop = true;
                audioSource.Play();
            }

        }
        else // 멈췄을 때
        {
            movement.MoveSpeed = 0f;
            weapon.Animator.MoveSpeed = 0f;
            if (audioSource.isPlaying == true) 
            {
                audioSource.Stop();
            }
        }

        if (Store.isStore == true || SubMenu.isSubMenu == true)// 상점 화면이 켜졌거나 서브메뉴가 켜졌을 때
        {
            movement.MoveSpeed = 0f;
            weapon.Animator.MoveSpeed = 0f;
            if (audioSource.isPlaying == true)
            {
                audioSource.Stop();
            }
        }

        movement.MoveTo(new Vector3(x,0,z));
    }
    
    private void UpdateJump()
    {
        if (Input.GetKeyDown(keyCodeJump))
        {
            movement.Jump();
        }
    }
    
    private void UpdateWeaponAction() 
    {

        if (Input.GetMouseButtonDown(0) && Store.isStore == false && SubMenu.isSubMenu == false) //마우스 왼쪽 버튼을 누르면서 상점 화면이 꺼졌을때, 서브메뉴가 꺼졌을 때
        { 
            weapon.StartWeaponAction();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            weapon.StopWeaponAction();
        }

        if (Input.GetKeyDown(keyCodeReload))
        {
            weapon.StartReload();
        }
        
    }

    public void SwithchingWeapon(WeaponBase newWeapon)
    {
        weapon = newWeapon;
    }
    public void OnTriggerEnter(Collider collider)
    {
        SoundManager.GetComponent<SoundManager>().PlayHitSound();//사운드 매니저의 캐릭터 피격음 재생
        StartCoroutine(ShowPlayerBlood());//피격 이미지 표시
        
        if (collider.gameObject.tag == "Punch")//좀비 펀치
        {
            if (PlayerHealth.pointAP > 0) //방탄복 게이지가 존재하면
            {
                PlayerHealth.pointAP -= ZombieController.damege; //좀비의 데미지만큼 방탄복 차감
                if (PlayerHealth.pointAP <= 0) //방탄복 게이지가 0 이하라면 +
                    PlayerHealth.pointAP = 0;
            }
            else //방탄복 게이지가 존재하지 않으면
            {
                if (PlayerHealth.pointHP <= 0) //체력 게이지가 0이하라면
                    PlayerHealth.pointHP = 0;
                else
                    PlayerHealth.pointHP -= ZombieController.damege; // 체력 차감
            }
        }
    }
    
    IEnumerator ShowPlayerBlood()
    {
        playerBlood.color = new Color(1,0,0,UnityEngine.Random.Range(0.2f, 0.3f));
        yield return new WaitForSeconds(0.1f);
        playerBlood.color = Color.clear;
    }

    public void PlayerDie()
    {
        playerDie = true;

        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");

        foreach (GameObject zombie in zombies)
        {
            zombie.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        }
        
        Cursor.visible = true; // 마우스 커서 보이기
        Cursor.lockState = CursorLockMode.None;// 마우스 커서 정상 상태
        
        SceneManager.LoadScene("ZombieEnding");
    }
}

