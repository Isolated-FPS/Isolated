using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    //좀비의 상태 정보
    public enum ZombieState { idle, trace, attack, die };
    //몬스터의 현재 상태 정보 저장
    public ZombieState zombieState = ZombieState.idle; 
    
    private Transform zombieTr;
    private Transform playerTr;
    private NavMeshAgent nvAgent;
    private Animator animator;
    
    public float traceDist = 10.0f;//추적 사정거리
    public float attackDist = 0.1f; //공격 사정거리
    public bool isDie = false;//좀비 사망여부
    public float zombieSpeed = 1.0f;  //좀비 속도
    public static float damege = 0.0f; //좀비 공격 데미지
    public float inputDamege = 0.0f; //좀비 공격 데미지 입력
    public int zombieHP = 100;//좀비 체력
    
    private AudioSource audioSource;//사운드 제어
    public AudioClip[] attackSounds;//공격 사운드
    public AudioClip hitSound;//피격 사운드
    private void Start()
    {
        //좀비의 Transform 할당
        zombieTr = this.gameObject.GetComponent<Transform>();
        //추적하는 플레이어의 Transform 할당
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();
        animator = this.gameObject.GetComponent<Animator>();

        nvAgent.speed = zombieSpeed;
        StartCoroutine(this.CheckZombieState());
        StartCoroutine(this.ZombieAction());
        
        audioSource = GetComponent<AudioSource>();
    }

    IEnumerator CheckZombieState()
    {
        while (!isDie)
        {
            yield return new WaitForSeconds(0.2f);
            //좀비와 플레이어 사이의 거리 측정
            float dist = Vector3.Distance(playerTr.position, zombieTr.position);
            if (dist <= attackDist) // 공격 사정거리라면 공격
            {
                damege = inputDamege;//공격시 각각 입력된 데미지를 전역변수 데미지에 입력
                zombieState = ZombieState.attack;
                
            }
            else if(dist <= traceDist)
            {
                zombieState = ZombieState.trace;
            }
            else
            {
                zombieState = ZombieState.idle;
            }
        }
    }

    IEnumerator ZombieAction()
    {
        while (!isDie)
        {
            switch (zombieState)
            {
                case ZombieState.idle:
                    nvAgent.Stop();
                    animator.SetBool("IsTrace",false);
                    break;
                case ZombieState.trace:
                    nvAgent.destination = playerTr.position;
                    nvAgent.Resume();
                    animator.SetBool("IsAttack",false);
                    animator.SetBool("IsTrace",true);
                    break;
                case ZombieState.attack:
                    nvAgent.Stop();
                    animator.SetBool("IsAttack",true);
                    break;
            }
            
            yield return null;
        }
    }

    public void OnPlayerDie()
    {
        StopAllCoroutines();
        nvAgent.Stop();
        animator.SetTrigger("IsPlayerDie");
    }

    public void OnDamege(int ondamage)
    {
        audioSource.PlayOneShot(hitSound);
        damege = 0; //데미지를 0으로 만들어 데미지 못주게 하기
        nvAgent.speed = 0; // 스피드를 0으로 만들어 움직이지 못하게 하기
        zombieHP -= ondamage;
        animator.SetTrigger("IsHit");
        if(zombieHP <= 0)
            ZombieDie();
        StartCoroutine(ReTrace());
    }

    IEnumerator ReTrace()
    {
        yield return new WaitForSeconds(2.0f);//2초 뒤에 다시 움직이게 하기
        damege = inputDamege;
        nvAgent.speed = zombieSpeed;
    }

    public void ZombieDie()
    {
        StopAllCoroutines();
        isDie = true;
        zombieState = ZombieState.die;
        nvAgent.Stop();
        animator.SetTrigger("IsDie");
        gameObject.GetComponentInChildren<CapsuleCollider>().enabled = false;
        foreach (Collider collider in gameObject.GetComponentsInChildren<SphereCollider>())
        {
            collider.enabled = false;
        }
        Destroy(gameObject,5f);
    }

    private void AttackSound()// 좀비 공격 애니메이션에 연결된 이벤트 
    {
        audioSource.PlayOneShot(attackSounds[UnityEngine.Random.Range(0, attackSounds.Length)]);
    }

}

