using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    public static int ClearCount = 0;
    public static int KillCount;
    public static int stageLevel = 0;
    public  GameObject Spawner;
    public static EnemySpawner enemySpawner;
    public static void AddKillCount()
    {
        KillCount++;
        enemySpawner.curEnemyCount--;
        if(stageLevel == 1 && CheckClearCount())
        {
            MoveToClear();
        }
    }
    public static bool CheckClearCount()
    {
        return KillCount == ClearCount;
    }

    public static void MoveToClear()
    {
        stageLevel = 0;
        ClearCount = 0;
        ResetSetting();
        SceneManager.LoadScene("Clear");
    }

    public static void MoveToGameOver()
    {
        stageLevel = 0;
        ClearCount = 0;
        ResetSetting();
        SceneManager.LoadScene("GameOver");
    }

    public static void ResetSetting()
    {
       
        KillCount = 0;
        ClearCount = DataManager.instance.data.stages[stageLevel].clearNum;
        enemySpawner.spawnRate = DataManager.instance.data.stages[stageLevel].spawnRate;
        enemySpawner.maxEnemyCount = DataManager.instance.data.stages[stageLevel].maxNum;
        enemySpawner.curEnemyCount = 0;


    }
            
    void Start()
    {
        stageLevel = 0;
        enemySpawner = Spawner.GetComponent<EnemySpawner>();
        ResetSetting();
    }

    GameObject[] enemyArr;
    GameObject[] bulletArr;
    void MoveToNextLevel()
    {
        if(CheckClearCount())
        {
            if(0<=stageLevel&&stageLevel<1)
            {
                enemyArr = GameObject.FindGameObjectsWithTag("Enemy");
                bulletArr = GameObject.FindGameObjectsWithTag("EnemyBullet");
                for (int i=0; i<enemyArr.Length; i++)
                {
                    PoolManager.instance.ReturnToPool(enemyArr[i]);
                }

                for (int i = 0; i < bulletArr.Length; i++)
                {
                    PoolManager.instance.ReturnToPool(bulletArr[i]);
                }
                stageLevel++;
                ResetSetting();

                SoundManager.instance.PlayAudioSource(SoundManager.instance.clearClip);

            }
           
        }

    }
    // Update is called once per frame
    void Update()
    {
    
        MoveToNextLevel();

    }
}
