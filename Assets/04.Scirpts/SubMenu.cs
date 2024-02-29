using System;
using System.Collections;
using System.Collections.Generic;
using Boxophobic.StyledGUI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Cursor = UnityEngine.Cursor;
public class SubMenu : MonoBehaviour
{
    public GameObject imgStore;
    public GameObject imgSubMenu;
    public GameObject crossHair;
    
    public static bool isSubMenu = false;// 서브 메뉴 확인용 bool
    public static bool isReisMain = false;
    private void Start()
    {
        imgSubMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if(!Store.isStore)//상점 메뉴가 꺼져 있다면
                if (imgSubMenu.activeSelf)//만약 서브메뉴가 켜져있다면
                {
                    crossHair.SetActive(true);//크로스 헤어 보이기
                    Cursor.visible = false; // 마우스 커서 가리기
                    Cursor.lockState = CursorLockMode.Locked; //중앙 위치 고정
                    imgSubMenu.SetActive(false);//서브메뉴 가리기
                    isSubMenu = false;
                }
                else
                {
                    crossHair.SetActive(false);//크로스 헤어 가리기
                    Cursor.visible = true; // 마우스 커서 보이기
                    Cursor.lockState = CursorLockMode.None;// 마우스 커서 정상 상태
                    imgSubMenu.SetActive(true);//서브메뉴 보이기 
                    isSubMenu = true;
                }
            else
            {
                if (imgSubMenu.activeSelf) //만약 서브메뉴가 켜져있다면
                {
                    imgSubMenu.SetActive(false); //서브메뉴 가리기
                    isSubMenu = false;
                }
                else
                {
                    imgSubMenu.SetActive(true);//서브메뉴 보이기
                    isSubMenu = true;
                }
            }
        }
      
    }

    public void OnRestart()
    {
        CountData.WavePoint = 0;
        Store.isStore = false;
        Store.point = 0;
        isReisMain = true;
        isSubMenu = false;
        imgStore.SetActive(false);
        imgSubMenu.SetActive(false);
        SceneManager.LoadScene("MainScene");
        
    }

    public void OnMenu()
    {
        CountData.WavePoint = 0;
        Store.isStore = false;
        Store.point = 0;
        isReisMain = true;
        isSubMenu = false;
        imgStore.SetActive(false);
        imgSubMenu.SetActive(false);
        SceneManager.LoadScene("Menu");
    }

    public void OnExit()
    {
        Application.Quit();
    }
}
