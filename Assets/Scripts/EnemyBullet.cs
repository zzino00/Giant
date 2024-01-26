using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    PoolAble poolAble;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        poolAble = GetComponent<PoolAble>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Bullet Released");


        if (other.tag == "Enemy")
            return;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        poolAble.ReleaseObject();
    }
}
