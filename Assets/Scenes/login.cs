using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class login : MonoBehaviour
{
    public TMP_Text num1;
    public TMP_Text  num2;
    public TMP_Text  num3;
    public TMP_Text num4;
    public string ID = "____"; // 預設的ID
    private int currentNum = 0;
    // Start is called before the first frame update
    public Storage save;
    public GameObject canvas;
    public GameObject info;
    public FirebaseTest Database;
    public void clickNumber(string number)
    {
        currentNum++;
        switch (currentNum)
        {
            case 1:
                num1.text = number;
                break;
            case 2:
                num2.text = number;
                break;
            case 3:
                num3.text = number;
                break;
            case 4:
                num4.text = number;
                break;
            default:
                
            return;
        }
    }


    public void resetNum(int number)
    {
        currentNum = 0;
        num1.text = "_";
        num2.text = "_";
        num3.text = "_";
        num4.text = "_";
    }

    public void loginButton()
    {
        if (currentNum == 4)
        {
            
            ID = num1.text + num2.text + num3.text + num4.text;
           
            StartCoroutine(Database.checkExist(ID)); // 檢查ID是否存在於Firebase中
            
            
        }
        
    }
}
