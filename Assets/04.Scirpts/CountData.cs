using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class CountData : MonoBehaviour
{
    public static int ZombieKill = 0;
    public static int WavePoint = 0;

    private void Awake()
    {
        if(PlayerPrefs.HasKey("ZombieKill"))
            ZombieKill = PlayerPrefs.GetInt("ZombieKill");
        
        PlayerPrefs.SetInt("WavePoint", WavePoint);
    }

   
}
