using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Enemy : MonoBehaviour
{
    public Image sizeCompare;

    public GameObject Self;
    public Enemy enemy;
    private void Start()
    {
        enemy = Self.GetComponent<Enemy>();
    }
    private void Update()
    {
        if (enemy.isChase)
        {
            sizeCompare.color = Color.red;
        }
        else
        {
            sizeCompare.color = Color.green;
        }

        transform.forward = Camera.main.transform.forward; // 이미지가 카메라를 계속 바라보게

    }


}
