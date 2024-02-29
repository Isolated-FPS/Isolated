using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject Setting;
    public void OnStart()
    {
        SceneManager.LoadScene("MainScene");
    }
    
    public void OnSetting()
    {
        Setting.SetActive(true);
    }

    public void OnExit()
    {
        Application.Quit();
    }

    public void OnClose()
    {
        Setting.SetActive(false);
    }
}
