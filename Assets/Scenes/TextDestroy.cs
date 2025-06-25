using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDestroy : MonoBehaviour
{
    public float lifetime = 5.0f;

    public float showUptime = 0.5f;
    private float timer = 0f;
    private CanvasGroup canvasGroup;
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        Destroy(gameObject, lifetime);
    }
        // Update is called once per frame
     void Update()
     {
        if (timer < lifetime)
        {
            timer += Time.deltaTime;
            if (timer < showUptime)
            {

                canvasGroup.alpha = Mathf.Clamp01(timer *( 1/ showUptime));
                return;
            }
            else if (timer < 4.5)
            {
                return;
            }
            else
            {
                canvasGroup.alpha = Mathf.Clamp01((5 - timer) * 2);
                return;
            }
        }
     }

        
}
