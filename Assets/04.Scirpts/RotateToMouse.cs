using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToMouse : MonoBehaviour
{
    [SerializeField] private float rotCamXAxisSpeed = 5;// 카메라 x축 회전속도
    [SerializeField] private float rotCamYAxisSpeed = 3;// 카메라 y축 회전속도
    private float limitMinX = -80;
    private float limitMaxX = 50;
    private float enlerAngleX; 
    private float enlerAngleY; 
    
    public void UpdateRotate(float mouseX, float mouseY)
    {
        enlerAngleY += mouseX * rotCamYAxisSpeed;// 마우스 좌/우 이동으로 카메라 y축 회전
        enlerAngleX -= mouseY * rotCamXAxisSpeed;// 마우스 위/아래 이동으로 카메라 x축 회전

        enlerAngleX = ClampAngle(enlerAngleX, limitMinX, limitMaxX);
        
        transform.rotation = Quaternion.Euler(enlerAngleX, enlerAngleY, 0);
    }
    
    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }
}
