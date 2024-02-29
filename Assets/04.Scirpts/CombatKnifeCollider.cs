using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatKnifeCollider : MonoBehaviour
{
    public ImpactMemoryPool impactMemoryPool;
    public Transform knifeTransform;

    private new Collider collider;
    private int damege;

    private void Awake()
    {
        //충돌범위 컴포넌트 정보를 얻어와 저장
        collider = GetComponent<Collider>();
        collider.enabled = false;
    }

    public void StartCollider(int damege)//단검 공격시 호출
    {
        this.damege = damege;
        collider.enabled = true;
        //0.1초 뒤 충돌범위 컴포넌트 비활성화
        StartCoroutine("DisableTime", 0.1f);
    }

    private IEnumerator DisableTime(float time)
    {
        yield return new WaitForSeconds(time);

        collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        impactMemoryPool.SpawnImpact(other, knifeTransform);

        if (other.CompareTag("Zombie"))
        {
            //좀비 컨트롤러를 가져와 OnDamege 실행
            other.transform.GetComponent<ZombieController>().OnDamege(damege);
        }
    }
}
