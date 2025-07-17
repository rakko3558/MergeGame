using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaySound : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioSource audioSource2;
    //public AudioSource bgmSource;
    public AudioClip audio_put;
    public AudioClip audio_pick;

    private bool BgmMute; // 是否靜音背景音樂
    public AudioClip bgm;
    public GameObject MusicButton;
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
        //AudioClip clip = Resources.Load<AudioClip>("Sounds/AI_2"); // 不含副檔名
        //audioSource2.clip = clip;
        audioSource2.volume = 0.075f; // 設定音量為 50%
        audioSource2.loop = true; // 設定音效循環播放
        BgmMute=false; // 初始化背景音樂靜音狀態為 false
        audioSource2.Play();
    }

    public void ButtonMusic()
    {
        
        if (!BgmMute)
        {
            audioSource2.Pause(); // 暫停音效
            BgmMute= true; // 設定背景音樂靜音狀態為 true
            MusicButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Source/Mmute");

        }    
        else
        {
            audioSource2.UnPause(); // 恢復音效播放
            BgmMute = false; // 設定背景音樂靜音狀態為 true
            MusicButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Source/music");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
