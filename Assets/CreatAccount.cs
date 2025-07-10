using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.UI;
public class CreatAccount : MonoBehaviour
{
    public GameObject LoginPanel;
    public GameObject register;
    public GameObject canvas;
    public Storage save;
    public FirebaseTest Database;

    public TMP_Text randomID;

    // Start is called before the first frame update
    public void OnClickCreateAccount()
    {
        StartCoroutine(Database.generateID());
        LoginPanel.SetActive(false);
        register.SetActive(true);
    }

    public void backLogin()
    {
        LoginPanel.SetActive(true);
        register.SetActive(false);
    }


    public void OnClickRegister()
    {
        if (randomID.text != "") 
        { 
        save.playerID = randomID.text; // 將生成的隨機ID存儲到Storage中
        save.createAccount(save.playerID);
        canvas.SetActive(false); // 隱藏登入介面
        }
    }

    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
