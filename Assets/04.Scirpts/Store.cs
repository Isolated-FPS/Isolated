using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Cursor = UnityEngine.Cursor;


public class Store : MonoBehaviour
{
    public GameObject imgStore;
    
    public AK47 ak47;
    public DesertEagle desertEagle;
    
    public GameObject crossHair;
    
    private Button btnHealth;
    private Button btnArmor;
    private Button btnBigBullet;
    private Button btnSmallBullet;
    
    public Text timeText;
    private float time = 15f;//시간 15초

    public Text pointText;
    public static int point = 0;// 상점에서 사용할 포인트
    public static bool isStore = false; //상점 확인 bool
    void Start()
    {
        btnHealth = GameObject.Find("ImageHPButton").GetComponent<Button>();
        btnArmor = GameObject.Find("ImageArmorButton").GetComponent<Button>();
        btnBigBullet = GameObject.Find("ImageBigBulletButton").GetComponent<Button>();
        btnSmallBullet = GameObject.Find("ImageSmallBulletButton").GetComponent<Button>();
        imgStore.SetActive(false);
    }
    
    void Update()
    {
        if (isStore)
        {
            crossHair.SetActive(false);
            Cursor.visible = true; // 마우스 커서 보이기
            Cursor.lockState = CursorLockMode.None;// 마우스 커서 정상 상태
            imgStore.SetActive(true);
            if (time > 0)
                time -= Time.deltaTime;
            timeText.text = "다음 WAVE까지 " + Mathf.Ceil(time).ToString() + "초 남았습니다.";
            pointText.text = "Point: "+ point.ToString();
        }
        if(time <= 0)
        {
            crossHair.SetActive(true);
            Cursor.visible = false; // 마우스 커서 가리기
            Cursor.lockState = CursorLockMode.Locked; //중앙 위치 고정
            time = 15f; //시간 15초
            btnHealth.interactable = true;
            btnArmor.interactable = true;
            btnBigBullet.interactable = true;
            btnSmallBullet.interactable = true;
            isStore = false;
            imgStore.SetActive(false);
        }
    }

    public void HealthButtonClick()
    {
        if (point >= 100)
        {
            point -= 100;
            PlayerHealth.pointHP = 100;
            btnHealth.interactable = false;
        }
    }
    
    public void ArmorButtonClick()
    {
        if (point >= 100)
        {
            point -= 100;
            PlayerHealth.pointAP = 100;
            btnArmor.interactable = false;
        }
    }
    
    public void BigBulletButtonClick()
    {
        if (point >= 50)
        {
            point -= 50;
            ak47.StoreBullet();
            btnBigBullet.interactable = false;
        }
    }
    public void SmallBulletButtonClick()
    {
        if (point >= 25)
        {
            point -= 25;
            desertEagle.StoreBullet();
            btnSmallBullet.interactable = false;
        }
    }
    
    
}
