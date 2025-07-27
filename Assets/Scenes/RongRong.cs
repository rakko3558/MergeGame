using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RongRong : MonoBehaviour
{
    private Sprite firstSprite;
    private Sprite secondSprite;
    private Sprite thirdSprite;
    private SpriteRenderer sr;
    public bool isNotiOpen=false;
    public void Open()
    {
        this.gameObject.SetActive(true);
        //GetComponent<Collider2D>().enabled = true;  // ¶}±Ò
    }
    
    // Start is called before the first frame update
    void Start()
    {
       firstSprite = Resources.Load<Sprite>("Source/" + "Rong_00");
       secondSprite = Resources.Load<Sprite>("Source/" + "Rong_01");
       thirdSprite = Resources.Load<Sprite>("Source/" + "Rong_02");
        sr = GetComponent<SpriteRenderer>();
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.GetComponent<Farm>().CropIndex==1)
            sr.sprite = secondSprite;
        else
            sr.sprite = thirdSprite;

    }
    void OnTriggerExit2D()
    {
        sr.sprite = firstSprite;  
    }
}
