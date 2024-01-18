using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;


[RequireComponent(typeof(CharacterController))] // 플레이어 스크립트를 컴포넌트로 적용할시 자동으로 CharacterController컴포넌트도 적용됨
public class Player : MonoBehaviour
{
    enum State
    {
        Idle,
        Walk,
        Run,
        Fire,
        Reload,
    }

    enum PersState
    {
        First,
        Third,
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

    public GameObject weaponPoint;
    private Weapon weapon;
    //플레이이어 상체움직임을위한 변수
    private Transform playerChestTr;
    Vector3 chestDir = new Vector3();

    //카메라가 고정될 위치
    public Transform cameraTr;

    //무기위치
    public Transform weaponTr;

    //플레이어 상태를 나눌 Enum
    State playerState;


    // 플레이어 오프셋
    Vector3 walkWeaponPos = new Vector3(0, (float)-0.01, (float)0.01);
    Vector3 walkWeaponRot = new Vector3((float)79.40032, (float)349.7, 180);

    Vector3 idleWeaponPos = new Vector3(0, (float)-0.01, (float)0.01);
    Vector3 idleWeaponRot = new Vector3((float)65.3, (float)26.7, (float)219.7);


    Vector3 runWeaponPos = new Vector3(0, (float)-0.01, (float)0.01);
    Vector3 runWeaponRot = new Vector3((float)65.3, (float)-43.7, (float)168.5);

    Vector3 runCameraPos = new Vector3(0, (float)0, (float)-0.08);

    Vector3 fireWeaponPos = new Vector3(0, (float)-0.01, (float)0.01);
    Vector3 fireWeaponRot = new Vector3((float)65.3, (float)-43.7, (float)168.5);
    Vector3 fireCameraPos = new Vector3((float)-0.2, (float)0.15, (float)0.3);


    void Start()
    {
        //마우스 포인터 설정
      UnityEngine.Cursor.visible = false; // 포인터가 안보이게
      UnityEngine.Cursor.lockState = CursorLockMode.Locked;// 포인터가 화면밖으로 못나가게

        // 사용할 컴포넌트들
        rotateMouse = Camera.main.transform.GetComponent<RotateMouse>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        weapon = weaponPoint.GetComponent<Weapon>();

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
            playerState = State.Run;
        }
 
        if (axisX==0&&axisZ==0)// x,z인풋이 없으면 Idle상태
        {
            playerState = State.Idle;
           //PlayerState = State.Run;

        }
        else if (!Input.GetKey(KeyCode.LeftShift)) // 아닐때는 걷기
        {
          playerState = State.Walk;
        }


        if (Input.GetMouseButton(0))// 마우스 클릭시 격발
        {
            playerState = State.Fire;
           // Debug.Log("Shoot");
        }

        if(Input.GetKeyDown(KeyCode.R) || weapon.currentAmmo <=0)// 총알이 다 떨어지거나 R키 입력시 재장전
        {
            //총알을 장전하는데 2초가 걸리는데 그사이에 업데이트문이 계속호출되서 장전을 엄청 많이하게됨
            //그렇다고 모션을그대로 넣고 장전을 하게 해주자니 장전애니메이션이 의미가 없음 2초를 기다리는게 아니라 애니메이션 이벤트로 장전을 해야하나 생각중
            // bool로 트리거를 주거나 아니면 총쏠때만 체크
            playerState = State.Reload;
        }

    }
  
  
    void Update()
    {

        UpdateMove();
      
            switch (playerState)
            {
                case State.Idle:

                    moveSpeed = 0; // 이동속도
                    SetOffset(idleWeaponRot, idleWeaponPos,new Vector3(0,0,0),0.1f); // 기본 오프셋 설정
                    break;

                case State.Walk:

                    moveSpeed = 3;
                    SetOffset(walkWeaponRot, walkWeaponPos, new Vector3(0, 0, 0), 0.1f);
                    break;

                case State.Run:

                    moveSpeed = 10;
                    SetOffset(runWeaponRot, runWeaponPos, runCameraPos, 0.2f);
                    break;

                case State.Fire:

                    weapon.Fire();
                    SetOffset(fireWeaponRot, fireWeaponPos, fireCameraPos, 0.01f,true);
                    StopCoroutine("ReloadBullet");// 재장전중 사격하면 장전이 취소되게 설정
                    break;

                case State.Reload:
                animator.SetBool("Fire", false);// 사격 애니메이션 bool
                animator.SetTrigger("Reload");
                StartCoroutine(ReloadBullet(weapon.gun.reloadTime));
               
                   
                    break;
             }

    }

    void SetOffset(Vector3 weaponRot, Vector3 weaponPos, Vector3 cameraOffset, float cameraNear, bool isFire =false)
    {
        weapon.weaponPosOffset = weaponPos; // 무기 위치조정
        weapon.weaponRotOffset = weaponRot; // 무기 로테이션조정
        rotateMouse.CameraOffset = cameraOffset; // 카메라 위치조정
        Camera.main.nearClipPlane = cameraNear; // 카메라 Near값 조정
        animator.SetBool("Fire", isFire);// 사격 애니메이션 bool
    }
    IEnumerator ReloadBullet(float reloadTime)
    {
        yield return new WaitForSeconds(reloadTime); // 매개변수로 받은 시간이 지나야 총알이 장전됨
        weapon.currentAmmo = weapon.gun.maxAmmo;
        animator.SetTrigger("Idle");
        Debug.Log("Reload");

    }

    void Move_BoneRotation()
    {
        //플레이어에 리깅된 UpperChest가 카메라의 정면을 쳐다봄
        chestDir = Camera.main.transform.position + Camera.main.transform.forward * 50f;
        playerChestTr.LookAt(chestDir);
    }

    private void LateUpdate()
    {
        if(playerState != State.Run)
        {
            Move_BoneRotation();
        }
       
        MoveTo(new Vector3(axisX, 0, axisZ)); // 플레이어가 하늘을 보고 움직일때 공중에 뜨는것을 막기위해  y =0 으로 세팅

    }

    
}
