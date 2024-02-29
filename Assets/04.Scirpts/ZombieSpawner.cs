using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class ZombieSpawner : MonoBehaviour
{
    public ZombieController[] zombiePrefabs; // 생성할 좀비 Prefabs
    public Transform[] spawnPoints; //소환될 좀비 위치

    public ZombieController[] specialzombiePrefabs;//생성할 특수 좀비 Prefabs

    public ZombieController[] bosszombiePrefabs; // 생성할 보스 좀비 Prefabs
    public Transform [] bossspawnPoints; //소환될 보스 위치
    
    public TextMeshProUGUI textWave;
    public TextMeshProUGUI textZombieLeft;
    
    private List<ZombieController> zombies = new List<ZombieController>(); //생성할 좀비를 담을 List
    private int wave = 0;
    void Update()
    {
        if (zombies.Count <= 0 && Store.isStore == false) //좀비 숫자가 0이하이고 상점 메뉴가 열리지 않았다면
            SpawnWave();

        UpdateUI();

        if (zombies.Count <= 0 && wave < 5) //좀비 숫자가 0이하이고 wave가 5보다 작다면
        {
            Store.isStore = true;
            PlayerPrefs.SetInt("WavePoint",CountData.WavePoint);//클리어한 WavePoint 저장
        }

        if (SubMenu.isReisMain)//서브메뉴를 통해 재시작시 혹은 메인으로 향할시 Point 초기화
        {
            Store.point = 0;
            SubMenu.isReisMain = false;
        }

    }

    private void SpawnWave()
    {
        wave++;
        CountData.WavePoint++;

        if (wave == 6)
        {
            CountData.WavePoint--;
            PlayerPrefs.SetInt("WavePoint",CountData.WavePoint);
            Cursor.visible = true; // 마우스 커서 보이기
            Cursor.lockState = CursorLockMode.None;// 마우스 커서 정상 상태
            SceneManager.LoadScene("ClearEnding");
        }//wave가 6단계라면 보스를 클리어한 것이므로 PlayerPrefs에 지금까지의 내용을 저장하고 엔딩씬 불러오기
        
        switch (wave)//wave에 따라 Store의 Point 값을 더해줌
        {
            case 1: Store.point += 50; break;
            case 2: Store.point += 100; break;
            case 3: Store.point += 150; break;
            case 4: Store.point += 200; break;
        }

        int spawnCount = wave * 10;

        for (int i = 0; i < spawnCount; i++)
        {
            CreateZombies();
        }
    
        if (wave >= 3)//wave가 3단계 이상이라면
        {
            for(int i = 0; i < wave * 2; i++)
            CreateSpecialZombies();
        }
        
        if(wave == 5)//wave가 5단계라면
            CreateBossZombie();//보스 좀비 생성
        
    }

    private void UpdateUI()
    {
        for(int i = 0; i < zombies.Count; i++)
            if (zombies[i].isDie == true)
            {
                zombies.Remove(zombies[i]); //List에서 좀비 삭제
                CountData.ZombieKill++;
                PlayerPrefs.SetInt("ZombieKill",CountData.ZombieKill);//좀비 처치시 마다 좀비 처치 수 증가 PlayerPrefs
            }

        textWave.text = $"<size=40>Wave : {wave}</size>";
        textZombieLeft.text = $"<size=40>Zombie Left: {zombies.Count}</size>";
    }

    private void CreateZombies()
    {
        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        ZombieController zombiePrefab = zombiePrefabs[UnityEngine.Random.Range(0, zombiePrefabs.Length)];

        ZombieController zombie = Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);

        zombies.Add(zombie);//List에 좀비 추가
    }
    
    private void CreateSpecialZombies()
    {
        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        ZombieController specialzombiePrefab = specialzombiePrefabs[UnityEngine.Random.Range(0, specialzombiePrefabs.Length)];

        ZombieController specialzombie = Instantiate(specialzombiePrefab, spawnPoint.position, spawnPoint.rotation);

        zombies.Add(specialzombie);//List에 특수 좀비 추가
    }
    
    private void CreateBossZombie()
    {
        Transform spawnPoint = bossspawnPoints[UnityEngine.Random.Range(0, bossspawnPoints.Length)];
        ZombieController bosszombiePrefab = bosszombiePrefabs[UnityEngine.Random.Range(0, bosszombiePrefabs.Length)];

        ZombieController bosszombie = Instantiate(bosszombiePrefab, spawnPoint.position, spawnPoint.rotation);

        zombies.Add(bosszombie);//List에 보스 좀비 추가
    }


}

