using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//按鈕事件
public class ButtonOnClick : MonoBehaviour
{

    public Button myButton;
    public  List<GameObject> GridPrefabs;
    public GameObject BoxPrefabs;
    public Transform spawnPoint;
    public GridmManager GridManager;
    void Start()
    {
       myButton.onClick.AddListener(OnClick); // 直接用程式接上事件
    }
    
    void OnClick()
    {
        if (GridPrefabs.Count== GridManager.CropAmount)
        {
            Debug.Log("沒有空格子可以放置物品了！！");
            return;
        }
        //int index = Random.Range(0, GridPrefabs.Count);
        for (int i=0;i< GridPrefabs.Count; i++)
        {
           
            GridCell Script = GridPrefabs[i].GetComponent<GridCell>();
            if (Script.Crop == null)
            {
                GameObject chosenA = GridPrefabs[i];
                spawnPoint = chosenA.transform;
                Script.status = 1;
                GameObject spawnedB = Instantiate(BoxPrefabs, spawnPoint.position, Quaternion.identity);
                //spawnedB.transform.rotation = Quaternion.Euler(0, 0, 0);
                Script.Crop = spawnedB;
                Farm NewCrop = spawnedB.GetComponent<Farm>();
                
                NewCrop.OnThisGrid= GridPrefabs[i];
                NewCrop.GetRandomCrop();
                NewCrop.ChangeSprite();
                GridManager.CropAmount++;
                return;
            }
            
        }
        Debug.Log("沒有空格子可以放置物品了！");
        return;
        //Debug.Log($"隨機選到的 A 是：{chosenA.name}");
    }

 
}

