using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioSource audioSource;

    //public AudioSource bgmSource;
    public AudioClip audio_put;
    public AudioClip audio_pick;

    //public AudioClip bgm;
    // Start is called before the first frame update

    public void  Sound(int a)
    {
        switch (a)
        {
            case 0:
                audioSource.PlayOneShot(audio_pick);
                break;
            case 1:
                audioSource.PlayOneShot(audio_put);
                break;
            default:
                Debug.Log("沒有這個音效");
                break;
        }
    }

    void Start()
    {
        //bgmSource.Play(audio_pick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
