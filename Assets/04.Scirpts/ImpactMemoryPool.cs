using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ImpactType
{
    Normal = 0, Zombie
}

public class ImpactMemoryPool : MonoBehaviour
{
    public GameObject[] impactPrefab;
    private MemoryPool[] memoryPool;

    private void Awake()
    {
        memoryPool = new MemoryPool[impactPrefab.Length];
        for (int i = 0; i < impactPrefab.Length; i++)
        {
            memoryPool[i] = new MemoryPool(impactPrefab[i]);
        }
    }

    public void SpawnImpact(RaycastHit hit)
    {
        //부딪힌 태그에 따라 처리
        if (hit.transform.CompareTag("ImpactNormal"))
        {
            OnSpawnImpact(ImpactType.Normal, hit.point, Quaternion.LookRotation(hit.normal));
        }
        
        else if(hit.transform.CompareTag("Zombie"))
        {
            OnSpawnImpact(ImpactType.Zombie, hit.point, Quaternion.LookRotation(hit.normal));
        }
    }

    public void SpawnImpact(Collider other, Transform transform)
    {   //부딪힌 오브젝트의 Tag 정보에 따라 다르게 처리
        if (other.CompareTag("ImpactNormal"))
        {
            OnSpawnImpact(ImpactType.Normal, transform.position, Quaternion.Inverse(transform.rotation));
        }
        else if (other.CompareTag("Zombie"))
        {
            OnSpawnImpact(ImpactType.Zombie, transform.position, Quaternion.Inverse(transform.rotation));
        }
       
    }

    public void OnSpawnImpact(ImpactType type, Vector3 position, Quaternion rotation)
    {
        GameObject item = memoryPool[(int)type].ActivatePoolItem();
        item.transform.position = position;
        item.transform.rotation = rotation;
        item.GetComponent<Impact>().Setup(memoryPool[(int)type]);
    }
}
