using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_ReStartButton : MonoBehaviour
{
    public void Awake()
    {
        UnityEngine.Cursor.visible = true; // 포인터가 안보이게
       UnityEngine.Cursor.lockState = CursorLockMode.None;// 포인터가 화면밖으로 못나가게
    }
    public void MoveToStartGameScene()
        {
            Debug.Log("StartSecneButton");
            SceneManager.LoadScene("StartScene");
        }

    
}
