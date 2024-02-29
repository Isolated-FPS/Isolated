using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPool : MonoBehaviour
{
    private class PoolItem
    {
        public bool isActive;//게임오브젝트의 활성화,비활성화
        public GameObject gameObject;// 화면에 보이는 실제 게임오브젝트
    }

    private int increaseCount = 5;// 오브젝트가 부족할 떄 Instantiate()로 추가 생성되는 오브젝트 개수
    private int maxCount;// 현재 리스트에 등록되어 있는 오브젝트 개수
    private int activeCount;// 현재 게임에 사용되고 있는 오브젝트 개수

    private GameObject poolObject; //오브젝트 풀링에서 관리하는 게임 오브젝트 프리팹
    private List<PoolItem> poolItemList; // 관리되는 모든 오브젝트를 저장하는 리스트

    public int MaxCount => maxCount; //외부에서 현재 리스트에 등록되어있는 오브젝트 개수 확인을 위한 프로퍼티
    public int ActiveCount => activeCount; //외부에서 현재 활성화 되어있는 오브젝트 개수 확인을 위한 프로퍼티

    public MemoryPool(GameObject poolObject)
    {
        maxCount = 0;
        activeCount = 0;
        this.poolObject = poolObject;

        poolItemList = new List<PoolItem>();

        InstantiateObjects();
    }//메모리풀 생성자에서 변수들을 초기화하고 인스턴티에이트오브젝트를 호출해 최초 5개의 아이템을 생성

    public void InstantiateObjects()
    {
        maxCount += increaseCount;

        for (int i = 0; i < increaseCount; i++)
        {
            PoolItem poolItem = new PoolItem();

            poolItem.isActive = false;
            poolItem.gameObject = GameObject.Instantiate(poolObject);
            poolItem.gameObject.SetActive(false);
            
            poolItemList.Add(poolItem);
        }
    }//Instantiate로 오브젝트 생성 후 해당 오브젝트 정보를 poolItem 리스트에 저장

    private void DestroyObjects()
    {
        if (poolItemList == null) return;

        int count = poolItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            GameObject.Destroy(poolItemList[i].gameObject);
        }
        
        poolItemList.Clear();
    }// 현재 관리 중인 모드 오브젝트 삭제 씬이 바뀌거나 게임이 종료될때 한번 호출

    public GameObject ActivatePoolItem()
    {
        //리스트가 비어있으면 관리중인 오브젝트가 없으므로 null 반환
        if (poolItemList == null) return null;

        //모든 오브젝트가 활성화되어 사용 중인 것이므로 더 생성함
        if (maxCount == activeCount)
        {
            InstantiateObjects();
        }

        //poolItemList의 요소들을 탐색해 비활성화 상태인 오브젝트를 찾아 활성화 시킴
        int count = poolItemList.Count;
        for (int i = 0; i < count; i++)
        {
            PoolItem poolItem = poolItemList[i];

            if (poolItem.isActive == false)
            {
                activeCount++;
                poolItem.isActive = true;
                poolItem.gameObject.SetActive(true);

                return poolItem.gameObject;
            }
        }
        
        return null;
    }//현재 비활성화된 오브젝트 중 하나를 활성화 해주는 메소드

    public void DeactivatePoolItem(GameObject removeObject)
    {
        if (poolItemList == null || removeObject == null) return;
        int count = poolItemList.Count;
        for (int i = 0; i < count; i++)
        {
            PoolItem poolItem = poolItemList[i];

            if (poolItem.gameObject == removeObject)
            {
                activeCount--;
                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);

                return;
            }
        }
    }//사용이 끝난 오브젝트를 비활성화하는 메소드

    public void DeactivateAllPoolItems()
    {
        if (poolItemList == null) return;

        int count = poolItemList.Count;
        for (int i = 0; i < count; i++)
        {
            PoolItem poolItem = poolItemList[i];

            if (poolItem.gameObject != null && poolItem.isActive == true)
            {
                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);
            }
        }

        activeCount = 0;

    }//활성화 상태인 모든 리스트요소를 비활성화 하는 메소드

}
