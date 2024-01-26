using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeEnemy : UnitBase
{
    Player player;
    float myMaxHp = 100;
    bool isSmaller;
    void Start()
    {
        Init();
    }
    public override void GetDamage(float damage)// UnitBase의 GetDamage함수를 오버라이드
    {
        base.GetDamage(damage);
        Debug.Log(base.curHp);
    }
    protected override void Init()
    {
        base.maxHp = myMaxHp; // 최대체력
        player = GameObject.FindWithTag("Player").GetComponent<Player>();// 플레이어와의 거리와 크기를 계속 비교해야함으로 
        base.targetSize = player.playerSize;
    
        base.Init();// 
    }
    // Update is called once per frame
    void Update()
    {
        isSmaller = !IsSmaller(player.gameObject);// 상대가 나보다 작을때
    }
}
