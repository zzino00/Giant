using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject playerOb;
    Player player;
    public Gun gun;
   public ParticleSystem firePs;
    float currentFireRate;
    public int currentAmmo;
   
    void Start()
    {
        gun = GetComponentInChildren<Gun>();
        player = playerOb.GetComponent<Player>();
        currentFireRate = 0;
        currentAmmo = gun.maxAmmo;
       
    }
    [SerializeField]
    public  Vector3 weaponPosOffset;
    [SerializeField]
    public Vector3 weaponRotOffset;

    [SerializeField]
    public Quaternion weaponQRotOffset;

    public void FireRateCalc()
    {
        if(currentFireRate >0)
        {
            currentFireRate -= Time.deltaTime; // currentFireRate에서 1초씩 빼서 0이되면 발사
        }

    }

    Enemy targetEnemy;
    PracticeEnemy practiceEnemy;
    public void Fire(float fireRate)
    {
      
        if(currentAmmo > 0) // 총알이 존재하고
        {
            if (currentFireRate <= 0) // 발사카운트가 0이되면 사격가능
            {
               // Debug.Log(currentAmmo);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 카메라에서 화면중앙으로 향하는 ray 
                 Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);//카메라에서 화면중앙으로 디버깅용 ray그리기
                RaycastHit hit;
                ShowMuzzleFleshEffect();
                LayerMask mask = LayerMask.GetMask("Monster");
                if (Physics.Raycast(ray, out hit, 100.0f))// raycast 부딪힌 물체 정보는 hit으로 반환
                {
                    InstantiateBulletImpact(hit);
                    targetEnemy =hit.transform.GetComponent<Enemy>();
                    practiceEnemy = hit.transform.GetComponent<PracticeEnemy>();
                    if (targetEnemy != null)
                    {
                        targetEnemy.GetDamage(gun.damage);
                      
                    }
                    if (practiceEnemy != null)
                    {
                        practiceEnemy.GetDamage(gun.damage);
                    }
                    //   Debug.Log($"Hit @{hit.collider.gameObject.name}");// 부딪힌 물체 이름 로그에 띄우기
                }

                currentFireRate = gun.fireRate; // 쐈으니까 다시 카운트 초기화
                currentAmmo--;
            }
        }
       
      
    }

    void ShowMuzzleFleshEffect()
    {
        SoundManager.instance.PlayFireAudioSource(SoundManager.instance.fireClip);
           // firePs.Stop();
            firePs.Play();
            
       
    }

    public void Fire()//단발
    {
        if (currentAmmo > 0) // 총알이 존재하고
        {
          
                // Debug.Log(currentAmmo);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 카메라에서 화면중앙으로 향하는 ray 
                Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);//카메라에서 화면중앙으로 디버깅용 ray그리기
                RaycastHit hit;
            ShowMuzzleFleshEffect();
            LayerMask mask = LayerMask.GetMask("Monster");
                if (Physics.Raycast(ray, out hit, 100.0f))// raycast 부딪힌 물체 정보는 hit으로 반환
                {
                    targetEnemy = hit.transform.GetComponent<Enemy>();
                    practiceEnemy = hit.transform.GetComponent<PracticeEnemy>();

                   InstantiateBulletImpact(hit);

                    if (targetEnemy != null)
                    {
                        targetEnemy.GetDamage(gun.damage);

                    }
                    if(practiceEnemy != null)
                    {
                    practiceEnemy.GetDamage(gun.damage);
                    }
                    //   Debug.Log($"Hit @{hit.collider.gameObject.name}");// 부딪힌 물체 이름 로그에 띄우기
                }
                currentAmmo--;
            }
        


    }
    public GameObject bulletImpactParticle;
    
    public void InstantiateBulletImpact(RaycastHit hit)
    {


        GameObject bulletImpact = Instantiate(bulletImpactParticle, hit.point,Quaternion.LookRotation(hit.normal));
    
        Destroy(bulletImpact, 2f);


    }

        void Update()
    {
        FireRateCalc();// 발사카운트 계속 세주기

        //애니메이션에 따른 무기위치 실시간 조정
        transform.SetParent(player.weaponTr); //+ WeaponPosOffset;
        transform.localPosition = weaponPosOffset;
        transform.localRotation = Quaternion.Euler(weaponRotOffset);
    }
}
