using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMouse : MonoBehaviour
{
    // 카메라회전 속도
    [SerializeField]
    private float XSpeed = 5; 
    [SerializeField]
    private float YSpeed = 5;

    //회전값과 범위제한
    private float limitMinX; // = -50;
    private float limitMaxX; // = 50;
    private float eulerAnlgleX;
    public float eulerAnlgleY;

    //에임포인터?
    public GameObject AimPoint;

    //플레이어의  카메라위치에 카메라 고정을 위해서
    public GameObject Player;


    public void UpdateRotate(float mouseX, float mouseY)
    {
        limitMinX = -50;
        limitMaxX = 50;
       
        eulerAnlgleY += mouseX * YSpeed; //  Y축을 기준으로 좌,우로 회전
        eulerAnlgleX -= mouseY * XSpeed;// X축을 기준으로 상,하로 회전 -로 해줘야 안뒤집힘

       eulerAnlgleX = Mathf.Clamp(eulerAnlgleX, limitMinX, limitMaxX);
       
        transform.rotation = Quaternion.Euler(eulerAnlgleX, eulerAnlgleY, 0);
    }

    [SerializeField]
    private Vector3 CameraOffset; // 카메라 세부위치 조정
    private void Update()
    {
        // MouseX,Y값 받아오기
        float mouseX = Input.GetAxis("Mouse X"); 
        float mouseY = Input.GetAxis("Mouse Y");
         //transform.position =new Vector3(transform.position.x, Player.GetComponent<Player>().cameraTr.position.y, Player.GetComponent<Player>().cameraTr.position.z)+ CameraOffset; // 플레이어의 cameraTr에 카메라 고정
        transform.position = Player.GetComponent<Player>().cameraTr.position + CameraOffset; // 플레이어의 cameraTr에 카메라 고정
        UpdateRotate(mouseX, mouseY);
    }
}
