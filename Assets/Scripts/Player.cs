using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))] // 플레이어 스크립트를 컴포넌트로 적용할시 자동으로 CharacterController컴포넌트도 적용됨
public class Player : MonoBehaviour
{
  
    enum State
    {
        Idle,
        Walk,
        Run
    }



    [SerializeField]
    private float moveSpeed = 10.0f;
    [SerializeField]
    private Vector3 moveForce;

    private RotateMouse rotateMouse;
    private CharacterController characterController;
    private Animator animator;

    private Transform playerChestTr;
    Vector3 ChestDir = new Vector3();
    public Vector3 ChestOffset = new Vector3(0, 0, 0);
    State playerState;

    float axisX;
    float axisZ;
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        rotateMouse = GetComponent<RotateMouse>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if(animator)
        {
            playerChestTr = animator.GetBoneTransform(HumanBodyBones.UpperChest);
        }
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = 10;
            Debug.Log("moveSpeed:" + moveSpeed);
        }
        else
        {
            moveSpeed = 3;
        }
        if (axisX == 0 && axisZ == 0)
        {
            moveSpeed = 0;
        }

        UpdateMove();
     
        characterController.Move(moveForce * Time.deltaTime);
        
    }

    public void MoveTo(Vector3 direction)
    {
        //moveForce = Camera.main.transform.rotation* new Vector3(direction.x, 0 ,direction.z);// 위나 아래를 바라보고 움직일 때 공중에 뜨거나 바닥아래로 내려가는 것을 막기위해서
        animator.SetFloat("Speed",moveSpeed);
       moveForce =  transform.rotation * direction*moveSpeed;
    }

    private void UpdateMove()
    {
        axisX = Input.GetAxisRaw("Horizontal");
        axisZ = Input.GetAxisRaw("Vertical");

       

        if(Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = 10;
        }
        else
        {
            moveSpeed = 3;
        }
        if (axisX == 0 && axisZ == 0) 
        {
            moveSpeed = 0;
        }

        MoveTo(new Vector3(axisX, 0, axisZ));
    }

   
    void Move_BoneRotation()
    {
        ChestDir = Camera.main.transform.position + Camera.main.transform.forward * 50f;
        playerChestTr.LookAt(ChestDir);
        playerChestTr.rotation = playerChestTr.rotation*Quaternion.Euler(ChestOffset);
    }

    private void LateUpdate()
    {
        Vector3 BodyDir = new Vector3(ChestDir.x, 0, ChestDir.z);
        Move_BoneRotation();

        
        if(Input.anyKey)
        {
            transform.LookAt(BodyDir);
        }

    }

    
}
