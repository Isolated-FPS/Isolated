using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Casing : MonoBehaviour
{
    //탄피 등장 후 비활성화 되는 시간
    public float deactivateTime = 5.0f;
    //탄피가 회전하는 속력 계수
    public float casingSpin = 1.0f;
    //탄피가 부딪혔을 때 사운드
    public AudioClip[] audioClips;

    private Rigidbody rigidbody3D;
    private AudioSource audioSource;
    private MemoryPool memoryPool;

    public void Setup(MemoryPool pool, Vector3 direction)
    {
        rigidbody3D = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        memoryPool = pool;
        
        //탄피의 이동 속도와 회전 속도 설정
        rigidbody3D.velocity = new Vector3(direction.x, 1.0f, direction.z);
        rigidbody3D.angularVelocity = new Vector3(UnityEngine.Random.Range(-casingSpin, casingSpin),
            UnityEngine.Random.Range(-casingSpin, casingSpin), UnityEngine.Random.Range(-casingSpin, casingSpin));
        
        //탄피 자동 비활성화를 위한 코루틴
        StartCoroutine("DeactivateAfterTime");
    }

    private void OnCollisionEnter(Collision collision)
    {
        //여러개 사운드 중 랜덤 재생
        int index = UnityEngine.Random.Range(0, audioClips.Length);
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }

    private IEnumerator DeactivateAfterTime()
    {
        yield return new WaitForSeconds(deactivateTime);
        
        memoryPool.DeactivatePoolItem(this.gameObject);
    }
}
