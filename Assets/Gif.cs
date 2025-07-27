using System.Collections;
using UnityEngine;
public class Gif : MonoBehaviour
{
    public Sprite[] frames; // 放你分解的圖片
    public float frameRate = 0.1f; // 每幀播放時間

    private SpriteRenderer image;
    private int currentFrame;

    void Start()
    {
        image = GetComponent<SpriteRenderer>();
        if (image == null || frames == null || frames.Length == 0)
        {
            Debug.LogError("缺少 SpriteRenderer 或 frames 沒有設");
            return;
        }
        StartCoroutine(PlayGif());
    }

    IEnumerator PlayGif()
    {
        while (true)
        {
            image.sprite = frames[currentFrame];
            currentFrame = (currentFrame + 1) % frames.Length;
            yield return new WaitForSeconds(frameRate);
        }
    }
}
