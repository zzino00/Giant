using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


[RequireComponent(typeof(CharacterController))] // 플레이어 스크립트를 컴포넌트로 적용할시 자동으로 CharacterController컴포넌트도 적용됨
public class Player : MonoBehaviour
{
    enum State
    {
        Idle,
        Walk,
        Run
    }

    //이동속도
    [SerializeField]
    private float moveSpeed = 10.0f;
    //이동 벡터
    [SerializeField]
    private Vector3 moveForce;

    //인풋값 저장
    float axisX;
    float axisZ;

    //사용할 컨포넌트들
    private RotateMouse rotateMouse;
    private CharacterController characterController;
    public Animator animator;

    public GameObject WeaponPoint;
    private Weapon weapon;
    //플레이이어 상체움직임을위한 변수
    private Transform playerChestTr;
    Vector3 ChestDir = new Vector3();

    //카메라가 고정될 위치
    public Transform cameraTr;

    //무기위치
    public Transform weaponTr;

    //플레이어 상태를 나눌 Enum
    State PlayerState;

    


    void Start()
    {
        //마우스 포인터 설정
        Cursor.visible = false; // 포인터가 안보이게
        Cursor.lockState = CursorLockMode.Locked;// 포인터가 화면밖으로 못나가게

        // 사용할 컴포넌트들
        rotateMouse = Camera.main.transform.GetComponent<RotateMouse>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        weapon = WeaponPoint.GetComponent<Weapon>();

        if(animator)
        {
            //애니메이터를 통해서 리깅되어있는 본을 사용(머리, 가슴);
            playerChestTr = animator.GetBoneTransform(HumanBodyBones.UpperChest);
            cameraTr = animator.GetBoneTransform(HumanBodyBones.Head);
            weaponTr = animator.GetBoneTransform(HumanBodyBones.RightHand);
        }
    }

    public void MoveTo(Vector3 direction)
    {

        // 마우스의 Y축회전에 따라 플레이어도 회전
        transform.rotation = Quaternion.Euler(0, rotateMouse.eulerAnlgleY, 0);

        animator.SetFloat("Speed", moveSpeed);// animator의 Speed변수에 따라 블랜딩의 정도가 변함

        moveForce = transform.rotation * direction * moveSpeed; // 플레이어가 바라보는 방향으로 이동방향이 정해짐
        characterController.Move(moveForce * Time.deltaTime);

    }


   
    private void UpdateMove()
    {
        //X,Z 인풋받기
        axisX = Input.GetAxisRaw("Horizontal");
        axisZ = Input.GetAxisRaw("Vertical");



        if (Input.GetKey(KeyCode.LeftShift)) // 쉬프트키를 누르고있는 동안 달림
        {
            PlayerState = State.Run;
           
        }
        else // 아닐때는 걷기
        {
            PlayerState = State.Walk;
        }
        if (axisX == 0 && axisZ == 0)// x,z인풋이 없으면 Idle상태
        {
            PlayerState = State.Idle;
           
        }

      
    }
  
    Vector3 WalkWeaponPos = new Vector3(0, (float)-0.01, (float)0.01);
    Vector3 WalkWeaponRot = new Vector3((float)79.40032, (float)349.7,180);

    Vector3 IdleWeaponPos = new Vector3(0, (float)-0.01, (float)0.01);
    Vector3 IdleWeaponRot = new Vector3((float)65.3, (float)26.7,(float)219.7);

    void Update()
    {

        UpdateMove();
        switch (PlayerState)
        {
            case State.Idle:
                moveSpeed = 0;
                weapon.WeaponPosOffset = IdleWeaponPos;
                weapon.WeaponRotOffset = IdleWeaponRot;
                break;

            case State.Walk:
                moveSpeed = 3;
                weapon.WeaponPosOffset = WalkWeaponPos;
                weapon.WeaponRotOffset = WalkWeaponRot;
                Debug.Log("Walk");
                break;

            case State.Run:
                moveSpeed = 10;
                break;
        }
    }

    void Move_BoneRotation()
    {
        //플레이어에 리깅된 UpperChest가 카메라의 정면을 쳐다봄
        ChestDir = Camera.main.transform.position + Camera.main.transform.forward * 50f;

        playerChestTr.LookAt(ChestDir);
    }


    private void LateUpdate()
    {

        Move_BoneRotation();

        MoveTo(new Vector3(axisX, 0, axisZ)); // 플레이어가 하늘을 보고 움직일때 공중에 뜨는것을 막기위해  y =0 으로 세팅

    }

    
}
