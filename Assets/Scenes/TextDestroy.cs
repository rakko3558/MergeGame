using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDestroy : MonoBehaviour
{
    public float lifetime = 3f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

        
}
