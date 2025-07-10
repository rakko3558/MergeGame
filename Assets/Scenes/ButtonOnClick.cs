using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//按鈕事件
public class ButtonOnClick : MonoBehaviour
{
    public Storage save;
    public Button myButton;
    public List<GameObject> GridPrefabs;
    public GameObject BoxPrefabs;
    public Transform spawnPoint;
    public GridmManager GridManager;
    public TMP_Text showBox;
    void Start()
    {
       
        myButton.onClick.AddListener(OnClick); // 直接用程式接上事件

        showBox.text = save.data.Boxs.ToString();   
    }

    void OnClick()
    {
        if (save.data.Boxs <= 0)
        { 
            return; // 如果沒有剩餘的箱子，則不執行任何操作

        }
        if (GridManager.Lands == GridManager.CropAmount)
        {
            //Debug.Log("沒有空格子可以放置物品了！！");
            return;
        }
        //int index = Random.Range(0, GridPrefabs.Count);
        for (int i = 0; i < GridManager.Lands; i++)
        {

            GridCell Script = GridPrefabs[i].GetComponent<GridCell>();
            if (Script.Crop == null)
            {
                GameObject chosenA = GridPrefabs[i];
                spawnPoint = chosenA.transform;
                //Script.status = 1;
                GameObject spawnedB = Instantiate(BoxPrefabs, spawnPoint.position, Quaternion.identity);
                //spawnedB.transform.rotation = Quaternion.Euler(0, 0, 0);
                Script.Crop = spawnedB;
                Farm NewCrop = spawnedB.GetComponent<Farm>();

                NewCrop.OnThisGrid = GridPrefabs[i];
                NewCrop.GetRandomCrop(save.data.playerLevel);
                NewCrop.ChangeSprite();
                GridManager.CropAmount++;
                save.AddBoxs(-1); // 減少剩餘箱子數量
                showBox.text = save.data.Boxs.ToString();
                return;
            }

        }
        //Debug.Log("沒有空格子可以放置物品了！");
        return;
        //Debug.Log($"隨機選到的 A 是：{chosenA.name}");
    }

}

