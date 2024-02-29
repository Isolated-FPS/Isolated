using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class CasingMemoryPool : MonoBehaviour
{
    //탄피 오브젝트
    public GameObject casingPrefab;
    //탄피 메모리 풀
    private MemoryPool memoryPool;

    private void Awake()
    {
        memoryPool = new MemoryPool(casingPrefab);
    }

    public void SpawnCasing(Vector3 position, Vector3 direction)
    {
        GameObject item = memoryPool.ActivatePoolItem();
        item.transform.position = position;
        item.transform.rotation = UnityEngine.Random.rotation;
        item.GetComponent<Casing>().Setup(memoryPool, direction);
    }
}
