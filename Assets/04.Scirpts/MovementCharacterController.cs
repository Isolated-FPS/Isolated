using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
//캐릭터 컨트롤러를 사용하기에 잊지 않고 포함할수 있도록 RequireComponent 사용
public class MovementCharacterController : MonoBehaviour
{
    public float moveSpeed; //이동속도
   
    private Vector3 moveForce; //이동하는 힘

    public float jumpForce; //점프하는 힘
    public float gravity; //중력
    public float MoveSpeed //외부에서 이동속도 제어를 위한 프로퍼티
    {
        set => moveSpeed = Mathf.Max(0, value); // 이동속도는 음수가 적용되지 않게
        get => moveSpeed;
    }

    private CharacterController characterController; //플레이어 이동 제어를 위한 컴포넌트

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!characterController.isGrounded)
        {
            moveForce.y += gravity * Time.deltaTime;
        }//허공에 떠 있으면 중력만큼 y축 속력 감소

        characterController.Move(moveForce * Time.deltaTime);
        // 초당 moveForce 속력으로 이동
    }

    public void MoveTo(Vector3 direction)
    {
        direction = transform.rotation * new Vector3(direction.x, 0, direction.z);
        //이동 방향 = 캐릭터 회전 값 * 방향 값
        moveForce = new Vector3(direction.x * moveSpeed, moveForce.y, direction.z * moveSpeed);
        //이동하는 힘 = 이동방향 * 속도
    }

    public void Jump()
    {
        if (characterController.isGrounded)
        {
            moveForce.y = jumpForce;
        }
    }
}
