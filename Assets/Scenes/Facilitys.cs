using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
public class Facilitys : MonoBehaviour
{
    public int requireCropAmount = 1;//該設施需要開啟的圖鑑數量
    public int expAmount = 100;//該設施可獲得的經驗量

    public bool isOpen = false;
    public bool isNotiOpen = false;
    public GameObject NotifyUI;

    public GameObject OpenButtom;


    public TMP_Text price;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void showButtom(int money)
    {
        OpenButtom.SetActive(true);
        price.text = money.ToString();
    }
    public void Open()
    {
        if (isOpen == false)
        {
            isOpen = true;
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f); // 白色 + 半透明
            GetComponent<Collider2D>().enabled = true;  // 開啟
            Destroy(OpenButtom);
        }
    }

    public void showNotify()
    {
        GameObject clonedUI = Instantiate(NotifyUI.gameObject);
        clonedUI.transform.SetParent(this.transform, false);
        clonedUI.SetActive(true); // 啟用 GameObject（如果 template 是 hidden 的話）
    }
}
