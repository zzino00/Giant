using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;


public class PoolManager : MonoBehaviour
{
    [System.Serializable]
    private class ObjectInfo
    {
        public string objectName;
        public GameObject prefab;
        public int count;
    }

    [SerializeField]
    private ObjectInfo[] objectInfos = null;

    public static PoolManager instance;

    private string objectName;
    private Dictionary<string, IObjectPool<GameObject>> objectPoolDic = new Dictionary<string, IObjectPool<GameObject>>();// key로 objectName을 받고 value로 해당오브젝트의 pool을 반환
    private Dictionary<string,GameObject> poolGoDic = new Dictionary<string, GameObject>();// key로 objectName을 받고 value로 해당오브젝트의 prefab 을 반환

    public bool IsReady {  get; private set; }


    

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(instance);

        Init();
    }
    GameObject rootGO;
    void Init()
    {
        IsReady = false;

        for(int i=0; i<objectInfos.Length; i++) // objectInfos에 있는 개수대로 풀 만들기
        {
            rootGO = new GameObject();
            rootGO.transform.parent = gameObject.transform;
            rootGO.name = objectInfos[i].objectName+"Root";
            IObjectPool<GameObject> pool = new ObjectPool<GameObject>
            (CreatePooledGo, TakeFromPool, ReturnToPool, DestroyPoolObject, true, objectInfos[i].count, objectInfos[i].count); //delegate generic 함수들을 매개변수로 받는 생성자, 
                                                                                                                               // 첫번째 매개변수를 제외하면 나머지는 반환값이 없는 generic함수를 매개변수로 받기                          
                                                                                                                               // 때문에 Action으로 받는다.

            if (poolGoDic.ContainsKey(objectInfos[i].objectName))//이미 풀이 생성되어 있다면
            {
                Debug.LogFormat("{0} 이미 등록된 오브젝트 입니다.", objectInfos[i].objectName);
                return;
            }

            poolGoDic.Add(objectInfos[i].objectName, objectInfos[i].prefab);//poolGoDic에 (이름, 프리팹)으로 넣기
            objectPoolDic.Add(objectInfos[i].objectName, pool);//objectPoolDic에 (이름,풀)으로 넣기

            for(int j=0; j < objectInfos[i].count; j++)// 정해진 값만큼 생성해두기
            {
                objectName = objectInfos[i].objectName;
                PoolAble poolAbleGo = CreatePooledGo().GetComponent<PoolAble>();
                poolAbleGo.Pool.Release(poolAbleGo.gameObject);// 풀에 넣기
            }
        }

       

        Debug.Log("오브젝트풀링 준비 완료");
        IsReady = true;
    }

    GameObject CreatePooledGo()
    {
        GameObject poolGo = Instantiate(poolGoDic[objectName],rootGO.transform);// 오브젝트 이름으로 프리팹을 생성
        poolGo.GetComponent<PoolAble>().Pool = objectPoolDic[objectName];// 오브젝트 이름으로 풀 반환?
        return poolGo;
    }

    void TakeFromPool(GameObject poolGo)
    {
        poolGo.SetActive(true);
    }

    public void  ReturnToPool(GameObject poolGo)
    {
        poolGo.SetActive(false);
    }

    private void DestroyPoolObject(GameObject poolGo)
    {
        Destroy(poolGo);
    }

    //public void ReturnAllEnemyToPool()
    //{
    //    poolGoDic.
    //}


    public GameObject GetGoDic(string goName)
    {
        objectName = goName;
        if(poolGoDic.ContainsKey(goName) ==false)
        {
            Debug.LogFormat("{0}풀에 등록되지않은 오브젝트입니다.", goName);
            return null;
        }

        if(goName == "Enemy")
        {
       
            GameObject enemyGo = objectPoolDic[goName].Get();
            Enemy enemy = enemyGo.GetComponent<Enemy>();
            enemyGo.transform.localScale *= DataManager.instance.data.stages[GameManager.stageLevel].enemySize;

            enemy.player = GameObject.FindWithTag("Player").GetComponent<Player>();
            enemy.mySize = enemy.transform.lossyScale.y; 
            enemy.fireRate = DataManager.instance.data.stages[GameManager.stageLevel].enemyFireRate;
            enemy.moveSpeed = DataManager.instance.data.stages[GameManager.stageLevel].enemySpeed;
            enemy.maxHp = DataManager.instance.data.stages[GameManager.stageLevel].maxHp;
            enemy.curHp = enemy.maxHp;
            enemy.nav = enemyGo.GetComponent<NavMeshAgent>();
          
          //  enemy.nav.updatePosition = false;
           // enemy.nav.updateRotation = false;
       
            return enemyGo;
            
        }
        return objectPoolDic[goName].Get();
    }
   
}
