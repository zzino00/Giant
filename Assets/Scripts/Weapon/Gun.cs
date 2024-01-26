using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum GunType
    {
        Pistol,
        Rifle,
        ShotGun,
      
    }

    public int maxAmmo;
    public float fireRate;
    public float damage;
    public float reloadTime;
    public GunType type;
    // Start is called before the first frame update
    void Awake()
    {
        type = GunType.Rifle;
        switch(type)
        {
            case GunType.Pistol:
                maxAmmo = 12;
                fireRate = 0.7f;
                damage = 5;
                reloadTime = 2.5f;
                break;
                case GunType.Rifle:
                maxAmmo = 30;
                fireRate = 0.3f;
                damage = 3;
                reloadTime = 2.5f;
                break;
                case GunType.ShotGun:
                maxAmmo = 8;
                fireRate = 2;
                damage = 10;
                reloadTime = 2.5f;
                break;
        }

    //    type = GunType.Rifle;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
