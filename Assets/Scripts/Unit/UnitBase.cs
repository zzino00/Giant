using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class UnitBase : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxHp;
    public float curHp
    {
        get;
        set;
    }
    float reductionRate;
    public float mySize; // 내 크기
    protected float targetSize;// 비교할 상대의 크기
   
    protected virtual void Init()
    {
        curHp = maxHp;
        mySize = transform.lossyScale.y; // 크기의 기준은 절대크기의 y값
    }
    public virtual void GetDamage(float damage)
    {
        curHp -= damage; // 피격시 체력과 함께 크기도 감소
        reductionRate = curHp / maxHp; // 크기 감소 비율
       
        BodyReduction();
       //if (reductionRate <= 0) // 체력이 0일때는 전체크기의 
       //     reductionRate = 0.1f;
    }

    protected virtual bool IsSmaller(GameObject Target)
    {
        targetSize = Target.transform.lossyScale.y;
        return targetSize > mySize;
    }
    protected virtual void BodyReduction()
    {
        transform.localScale *= reductionRate;
        if (transform.localScale.x <= 0.3)// 크기가 이하로는 안작아짐
        {
            transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        }
        mySize = transform.lossyScale.y;
    }
}
