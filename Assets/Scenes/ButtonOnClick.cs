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

    public AudioSource audio_put;
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
        if (GridManager.save.data.Lands == GridManager.CropAmount)
        {
            //Debug.Log("沒有空格子可以放置物品了！！");
            return;
        }
        //int index = Random.Range(0, GridPrefabs.Count);
        for (int i = 0; i < GridManager.save.data.Lands; i++)
        {

            GridCell Script = GridPrefabs[i].GetComponent<GridCell>();
            if (Script.Crop == null)
            {
                int Index = Random.Range(1, GridManager.save.data.playerLevel + 1);// cropNames.GetLength(0));
                int[] levelProbability = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 2 }; // 每個等級的機率百分比
                int Level = levelProbability[Random.Range(0, levelProbability.Length)]; // 0-3 隨機等級

                GridManager.SpawnSpecifyCrop(i/10,i%10, i / 10, i % 10, Index, Level,0);

                //GameObject chosenA = GridPrefabs[i];
                //spawnPoint = chosenA.transform;
                //GameObject spawnedB = Instantiate(BoxPrefabs, spawnPoint.position, Quaternion.identity);
                //Script.Crop = spawnedB;
                //Farm NewCrop = spawnedB.GetComponent<Farm>();

                //NewCrop.OnThisGrid = GridPrefabs[i];//位置
                //NewCrop.GetRandomCrop(save.data.playerLevel);
                //NewCrop.ChangeSprite();//詳細作物資訊
                               
                //GridManager.CropAmount++;
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

