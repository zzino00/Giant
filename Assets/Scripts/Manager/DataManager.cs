


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stage // 안에 있는 필드의 이름을 json파일의 이름과 맞추는게 중요
{
    public int level;
    public int clearNum;
    public int maxNum;
    public float maxHp;
    public float spawnRate;
    public float enemySize;
    public float enemySpeed;
    public float enemyFireRate;

}

[Serializable]
public class StageData
{
    public List<Stage> stages = new List<Stage>(); // 이름을 Json파일의 리스트랑 맞추는게 중요


    //public Dictionary<int, Stage> MakeDic()
    // 지금은 단순히 스테이지 단계별로 변수를 받아오면 되는 것이기 때문에 List로 충분하지만 나중에 ID를 활용해서 찾게 된다면 Dictionary를 쓸것
    //{

    //    Dictionary<int, Stage> dict = new Dictionary<int, Stage>();
    //    foreach (Stage stage in stages)
    //    {
    //        dict.Add(stage.level, stage);
    //    }
    //    return dict;
    //} 


}
public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    // Start is called before the first frame update
    public StageData data;
    void Awake()
    {
        instance = this;
        TextAsset textAsset = Resources.Load<TextAsset>("Json/StageData");
        data = JsonUtility.FromJson<StageData>(textAsset.text);
      
    }
 
   
}
