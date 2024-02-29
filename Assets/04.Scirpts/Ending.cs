using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    public void OnRestart()
    {
        CountData.WavePoint = 0;
        SceneManager.LoadScene("MainScene");
    }

    public void OnMenu()
    {
        CountData.WavePoint = 0;
        SceneManager.LoadScene("Menu");
    }

    public void OnExit()
    {
        CountData.WavePoint = 0;
        Application.Quit();
    }
}
