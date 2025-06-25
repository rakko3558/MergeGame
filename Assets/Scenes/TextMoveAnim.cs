using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMoveAnim : MonoBehaviour
{
    public float floatSpeed = 20f;       // 向上速度
    public float duration = 1f;        // 存在時間
                                       // Start is called before the first frame update
    private float timer = 0f;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > duration)
        {
            transform.localPosition += new Vector3(0, floatSpeed * Time.deltaTime, 0);

        }

    }
}
