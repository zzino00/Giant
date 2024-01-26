using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearSound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.PlayAudioSource(SoundManager.instance.finishClip);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
