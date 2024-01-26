using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public int maxEnemyCount;
    public int curEnemyCount;
    public float spawnRate;
    float curRate;
    float enemyWaitRait;
    public Transform[] spawnTrs;
    // Update is called once per frame

    private void Start()
    {
        curRate = 0;
        maxEnemyCount =10;
        curEnemyCount =0;
    }
    public void SpawnRateCalc()
    {
        if (curRate > 0)
        {
            curRate -= Time.deltaTime; // currentFireRate에서 1초씩 빼서 0이되면 발사
        }

    }

    int randSpawnIndex;
    Enemy enemy;
    public void Spawn()
    {
        
        if (curEnemyCount < maxEnemyCount)
        {
            if (curRate <= 0)
            {
                GameObject enemyGO = PoolManager.instance.GetGoDic("Enemy");
                randSpawnIndex = Random.Range(0,spawnTrs.Length);
                enemy = enemyGO.GetComponent<Enemy>();
                enemy.nav = enemyGO.GetComponent<NavMeshAgent>();
                enemyGO.transform.position =spawnTrs[randSpawnIndex].position;
                Debug.Log(enemyGO.transform.position);
                curRate = spawnRate;
                curEnemyCount++;
            }
            
        }
    }
    void Update()
    {
        Spawn();
        SpawnRateCalc();
    }
}
