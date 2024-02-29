using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dataget : MonoBehaviour
{
    public Text ZombieCount;
    public Text WaveCount;

    private void Update()
    {
        if(PlayerPrefs.HasKey("ZombieKill"))
            ZombieCount.text = PlayerPrefs.GetInt("ZombieKill").ToString();
        else
        {
            ZombieCount.text = 0.ToString();
        }
        if(PlayerPrefs.HasKey("WavePoint"))
            WaveCount.text = PlayerPrefs.GetInt("WavePoint").ToString();
        else
        {
            WaveCount.text = 0.ToString();
        }
    }

    public void OnReset()
    {
        PlayerPrefs.SetInt("ZombieKill",0);
        PlayerPrefs.SetInt("WavePoint",0);
    }
}
