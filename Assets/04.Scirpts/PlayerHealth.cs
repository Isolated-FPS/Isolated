using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Audio Clips")] 
    public AudioClip audioClipDeath;
    public AudioClip audioClipHit;

    [Header("Player Health")] 
    public TextMeshProUGUI textHP;//플레이어 체력
    public TextMeshProUGUI textAP;//플레이어 방탄복 게이지

    public static float pointHP;
    public static float pointAP;
    private void Awake()
    {
        pointHP = 100;
        pointAP = 100;
        textHP.text = $"<size=50>HP {pointHP}</size>";
        textAP.text = $"<size=50>AP {pointAP}</size>";
    }
    
    void Update()
    {
        textHP.text = $"<size=50>HP {pointHP}</size>";
        textAP.text = $"<size=50>AP {pointAP}</size>";
    }
}
