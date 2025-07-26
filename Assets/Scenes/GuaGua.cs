using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuaGua : MonoBehaviour
{
    private Sprite firstSprite;
    private Sprite secondSprite;
    private SpriteRenderer sr;
    public void Open()
    {
        this.gameObject.SetActive(true);
        //GetComponent<Collider2D>().enabled = true;  // ¶}±Ò
    }
    
    // Start is called before the first frame update
    void Start()
    {
       firstSprite = Resources.Load<Sprite>("Source/" + "Guagua_01");
       secondSprite = Resources.Load<Sprite>("Source/" + "Guagua_02");
        sr = GetComponent<SpriteRenderer>();
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.GetComponent<Farm>().CropIndex!=0)
            sr.sprite = secondSprite; 
       
    }
    void OnTriggerExit2D()
    {
        sr.sprite = firstSprite;  
    }
}
