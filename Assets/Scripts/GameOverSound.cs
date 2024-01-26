using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverSound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.PlayAudioSource(SoundManager.instance.overClip);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
