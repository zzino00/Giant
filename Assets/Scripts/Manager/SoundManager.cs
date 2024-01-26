using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
  
    public AudioClip fireClip;
    public AudioClip reloadClip;
    public AudioClip clearClip;
    public AudioClip destroyClip;
    public AudioClip finishClip;
    public AudioClip overClip;
    public AudioClip itemClip;
    public AudioSource audiSource;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        audiSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
      
       

    }
    public void PlayAudioSource(AudioClip clip)
    {
        audiSource.volume = 0.5f;
        audiSource.clip = clip;
        audiSource.Play();
    }

    public void PlayDestroyAudioSource(AudioClip clip)
    {
        audiSource.volume = 1.0f;
        audiSource.clip = clip;
        audiSource.Play();
    }

    public void PlayFireAudioSource(AudioClip clip)
    {
        audiSource.volume = 0.3f;
        audiSource.clip = clip;
        audiSource.Play();
    }
}
