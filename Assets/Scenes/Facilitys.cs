using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Facilitys : MonoBehaviour
{
    public int requireCropAmount = 1;//�ӳ]�I�ݭn�}�Ҫ���Ų�ƶq
    public int expAmount = 100;//�ӳ]�I�i��o���g��q

    public bool isOpen = false;

    public GameObject NotifyUI;
    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Collider2D>().enabled = false;  // �}��
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
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f); // �զ� + �b�z��
            GetComponent<Collider2D>().enabled = true;  // �}��
        }
    }

    public void showNotify()
    {
        GameObject clonedUI = Instantiate(NotifyUI.gameObject);
        clonedUI.transform.SetParent(this.transform, false);
        clonedUI.SetActive(true); // �ҥ� GameObject�]�p�G template �O hidden ���ܡ^
    }
}
