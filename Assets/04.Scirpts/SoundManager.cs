using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource audioSource;//사운드 제어
    public AudioClip playerhitSound;//피격 사운드
    
    void Awake()
    {
        audioSource = GetComponent<AudioSource>(); 
    }

    public void PlayHitSound()
    {
        audioSource.PlayOneShot(playerhitSound);
    }
}
