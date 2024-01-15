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
       
        eulerAnlgleY += mouseX * YSpeed; //  Y축을 기준으로 좌,우로 회전
        eulerAnlgleX -= mouseY * XSpeed;// X축을 기준으로 상,하로 회전

        eulerAnlgleX = ClampAngle(eulerAnlgleX, limitMinX, limitMaxX);
        eulerAnlgleY = ClampAngle(eulerAnlgleY, limitMinY, limitMaxY);
        if(eulerAnlgleY< limitMinY || eulerAnlgleY > limitMaxY)
        {
            Debug.Log("Out of Lange");
            Debug.Log("PlayerY:" + Player.transform.rotation.eulerAngles.y);
        }
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

        if (angle < -360)
        {
            angle += 360;
            Debug.Log("Over");
            Debug.Log("PlayerY:" + Player.transform.rotation.eulerAngles.y);
            Debug.Log("FixedAngle:" + angle);
            min += 360;
            max += 360;
        }

        if (angle > 360)
        {
            angle -= 360;
            Debug.Log("Over");
            Debug.Log("PlayerY:" + Player.transform.rotation.eulerAngles.y);
            Debug.Log("FixedAngle:" + angle);
            min -= 360;
            max -= 360;

        }
        return Mathf.Clamp(angle, min, max);
    }
}
