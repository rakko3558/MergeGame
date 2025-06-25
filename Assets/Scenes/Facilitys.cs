using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Facilitys : MonoBehaviour
{
    public int requireCropAmount = 1;//該設施需要開啟的圖鑑數量
    public int expAmount = 100;//該設施可獲得的經驗量

    public bool isOpen = false;

    public GameObject NotifyUI;
    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Collider2D>().enabled = false;  // 開啟
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open()
    {
        if (isOpen == false)
        {
            isOpen = true;
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f); // 白色 + 半透明
            GetComponent<Collider2D>().enabled = true;  // 開啟
        }
    }

    public void showNotify()
    {
        GameObject clonedUI = Instantiate(NotifyUI.gameObject);
        clonedUI.transform.SetParent(this.transform, false);
        clonedUI.SetActive(true); // 啟用 GameObject（如果 template 是 hidden 的話）
    }
}
