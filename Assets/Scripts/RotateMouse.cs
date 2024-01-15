using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMouse : MonoBehaviour
{
    [SerializeField]
    private float XSpeed = 5;
    [SerializeField]
    private float YSpeed = 5;

    public GameObject Player;
    private float limitMinX; // = -50;
    private float limitMaxX; // = 50;
    private float limitMinY; // = -180;
    private float limitMaxY; // = 180;
    private float eulerAnlgleX;
    public float eulerAnlgleY;
   

    public void UpdateRotate(float mouseX, float mouseY)
    {
        limitMinX = -50;
        limitMaxX = 50;
        limitMinY = Player.transform.eulerAngles.y - 180;
        limitMaxY = Player.transform.eulerAngles.y + 180;
        Debug.Log("PlayerY:"+Player.transform.rotation.y);
        eulerAnlgleY += mouseX * YSpeed; //  Y축을 기준으로 좌,우로 회전
        eulerAnlgleX -= mouseY * XSpeed;// X축을 기준으로 상,하로 회전

        eulerAnlgleX = ClampAngle(eulerAnlgleX, limitMinX, limitMaxX);
        eulerAnlgleY = ClampAngle(eulerAnlgleY, limitMinY, limitMaxY);
        transform.rotation = Quaternion.Euler(eulerAnlgleX, eulerAnlgleY, 0);
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        UpdateRotate(mouseX, mouseY);
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360; // 360도를 넘어갔을때 넘어간 만큼만 회전하게
        if (angle > 360)  angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }
}
