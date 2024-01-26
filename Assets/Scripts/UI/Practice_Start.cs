using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Practice_Start : MonoBehaviour
{
    UI_Player uI_player;

    private void Start()
    {
       uI_player = GameObject.Find("PlayerUI").GetComponent<UI_Player>();
        uI_player.curStage.text = "practice";
        uI_player.killCountText.text = "0/0";

    }
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag =="Player")
        {
            SceneManager.LoadScene("InGame");
        }
    }
}

