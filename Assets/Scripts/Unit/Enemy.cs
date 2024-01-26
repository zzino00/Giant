using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

[RequireComponent(typeof(NavMeshAgent))] // 이 스크립트가 추가되면 자동으로 NavMeshAgent컴포넌트도 추가됨
public class Enemy : UnitBase
{
    //Enemy Stat
    public float myMaxHp = 100;
    public float moveSpeed = 1.0f;

    //스폰할 오브젝트들
    public GameObject[] gearDropArr;
    public GameObject EnemyBullet;

    //스폰할 방향
    Vector3 gearDropPos;
    Vector3 ShootDir;

    //스폰할 오브젝트의 리지드바디
    Rigidbody gearRb;
    Rigidbody bulletRb;

    //총알발사속도
    public float fireRate = 3.0f;
    float currentFireRate;

    //사격범위 
    float minRangeAtkDis = 5.0f;
    float maxRangeAtkDis = 10.0f;

    //현재 타겟과의 거리
    float curDistance;

    //총알 스폰포인트
    public GameObject bulletSpawnPoint;

    //추격여부를 결정하는 변수
    public bool isChase;

    public float defaultSize;
    //이동방향
    Vector3 runDir;

    //추가예정 아직사용안함
  //  float mileAttackRange = 3.0f;

    Animator anim;
    public NavMeshAgent nav; // navMeshAgnet로 이동
    public Player player;
    EnemyState enemyState;
    PoolAble poolAble;

    enum EnemyState
    {
        Move,
        Fire
    }

    protected override void Init()
    {
        //base.maxHp = myMaxHp; // 최대체력
        enemyState = EnemyState.Move;
        player = GameObject.FindWithTag("Player").GetComponent<Player>();// 플레이어와의 거리와 크기를 계속 비교해야함으로 
        nav = GetComponent<NavMeshAgent>(); // NavMesh로 플레이어 추격
        nav.speed = moveSpeed;
        anim = GetComponent<Animator>();
        base.targetSize = player.playerSize;
        currentFireRate = 0; // 사격빈도
        base.Init();// 
        isChase = !IsSmaller(player.gameObject);
        poolAble = GetComponent<PoolAble>();
        defaultSize = 1;
       
    }
    private void Start()
    {
        Init();
    }

    //if (curDistance <= mileAttackRange)
    //{
    //    anim.SetBool("isCharge", true);
    //    nav.SetDestination(player.transform.position * 2);


    //}
    //else
    //{
    //    nav.speed = moveSpeed;
    //    nav.SetDestination(player.transform.position);
    //}

    //  Debug.Log("Chase");
    void ChaseTarget()
    {
        
        nav.SetDestination(player.transform.position); // nav로 추격

    }

   

  
    
    void RunAway()
    {
        runDir = (transform.position - player.transform.position).normalized;// 타겟의 반대 방향으로 도망
        nav.SetDestination(transform.position+runDir);
     
        enemyState = EnemyState.Fire;
        

    }

    void MileAttack()
    {

    }

    
    void RangeAttack()
    {
        // Lerp 사용해서 자연스럽게 회전하게
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), 5 * Time.deltaTime); 
        anim.SetBool("isShoot", true);
       // ShootDir = player.transform.position - transform.position;
        if (currentFireRate <= 0)
        {
            ShootBullet();
            currentFireRate = fireRate;
            enemyState = EnemyState.Move;
         
        }
    }

    
    void ShootBullet()
    {
       
        ShootDir = player.transform.position - transform.position;
        GameObject bullet = PoolManager.instance.GetGoDic("EnemyBullet");
        bullet.transform.position = bulletSpawnPoint.transform.position;
        bullet.transform.rotation =Quaternion.Euler(bulletSpawnPoint.transform.position);
       // float bulletSpeed = 10.0f;
      //  bullet.transform.position = Vector3.Lerp(bulletSpawnPoint.transform.position,player.transform.position, bulletSpeed*Time.deltaTime);
        bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.AddForce(ShootDir * 30.0f, ForceMode.Force);

    }

   
    private void Update() //FSM
    {
        gearDropPos = new Vector3(transform.position.x, 2, transform.position.z); // 부품아이템을 떨어뜨리는 위치
        curDistance = Vector3.Distance(transform.position, player.transform.position);//  타겟과의 현재거리
       
        switch (enemyState)
        {
            case EnemyState.Move:

                
                nav.speed = moveSpeed;
                nav.isStopped = false; 
                anim.SetBool("isShoot", false);
                isChase = !IsSmaller(player.gameObject);// 상대가 나보다 작을때
               
                if (isChase|| curDistance>maxRangeAtkDis) // 상대가 나보다 작거나 최대범위 밖일때 추격
                    ChaseTarget();
                else RunAway();// 아니면 도망감
                break;

            case EnemyState.Fire:

                if (curDistance < minRangeAtkDis )// 사격범위보다 가까우면 이동
                {
                    
                    enemyState = EnemyState.Move;
                }
                else// 아니면 공격
                {
                    nav.isStopped = true;
                    RangeAttack();
                   
                }
               
                break;
        }

        FireRateCalc(); // 총알발사빈도 계산
      

    }


    public void FireRateCalc()
    {
        if (currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime; // currentFireRate에서 1초씩 빼서 0이되면 발사
        }

    }



    public override void GetDamage(float damage)// UnitBase의 GetDamage함수를 오버라이드
    {
        base.GetDamage(damage);
        DropGear();// 아이템을 떨어뜨림
        if (curHp < 0)
        {
            curHp = 0;
            gameObject.SetActive(false);
            GameManager.AddKillCount();// 체력이 0이되면 킬카운트를 올림
        }
    }

   
    public void DropGear()// 공격을 당했을때 아이템을 흩뿌리는 함수
    {
        int randGearIndex = Random.Range(0,gearDropArr.Length);
        float randDropforceX = Random.Range(0.0f, 2.0f);
        float randDropforceY = Random.Range(0.0f, 2.0f);
        float randDropforceZ = Random.Range(0.0f, 2.0f);
        Vector3 randDropVec = new Vector3(randDropforceX, randDropforceY, randDropforceZ);
        GameObject dropGear = Instantiate(gearDropArr[randGearIndex], gearDropPos, transform.rotation);
        gearRb = dropGear.GetComponent<Rigidbody>();
        gearRb.AddForce(randDropVec, ForceMode.Impulse);
    }

    
   
    private void OnTriggerEnter(Collider other)// 플레이어보다 크기 작은 상황에서 플레이어와 충돌하면 바로 사망
    {
        if (other.tag != "Player")
            return;

       // base.mySize = transform.lossyScale.y;
        bool isSmaller = IsSmaller(player.gameObject);
        if (isSmaller)
        {
            SoundManager.instance.PlayDestroyAudioSource(SoundManager.instance.destroyClip);
            poolAble.ReleaseObject();
           // gameObject.SetActive(false);
            GameManager.AddKillCount();
            
        }

    }
}
