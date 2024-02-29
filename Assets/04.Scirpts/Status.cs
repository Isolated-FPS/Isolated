using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [Header("Walk, Run Speed")] //인스펙터 창에서 보기 위해
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    public float WalkSpeed => walkSpeed; //람다식
    public float RunSpeed => runSpeed; //람다식
    //외부에서 값을 확인하는 용도
}
