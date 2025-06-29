using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//作物
public class Farm : MonoBehaviour
{

     private static string[,] cropNames= new string[,]
        {
           { "coin0","coin1","coin2","coin3","coin4","coin5","coin6","coin7","coin_C", "coin_C5", "coin_S", "coin_S5","coin_G","coin_G5"},
           { "Paper_00", "Paper_01", "Paper_02", "Paper_03","","","","","","","","","",""},
           { "ylr0", "ylr1", "ylr2", "ylr3","","","","","","","","","",""},
           { "uu0", "uu1", "uu2", "uu3","","","","","","","","","",""},
           { "tk0", "tk1", "tk2", "tk3","","","","","","","","","",""},
           { "Plus0_00", "Plus0_01", "Plus0_02", "Plus0_03","","","","","","","","","",""},
           { "sc0", "sc1", "sc2", "sc3","","","","","","","","","",""},
           { "rik0", "rik1", "rik2", "rik3","","","","","","","","","",""},
           { "pkc0", "pkc1", "pkc2", "pkc3","","","","","","","","","",""},
           { "pj0", "pj1", "pj2", "pj3","","","","","","","","","",""},
           { "Princess_00", "Princess_01", "Princess_02", "Princess_03","","","","","","","","","",""}
        };

    //public GameObject image;

    //public int PlayerLevel = 1;
    public int CropIndex=0;
    public int CropLevel=0;
    public int HaveCoin =0;
    public int CropValue = 1;//預設 1等 1塊錢
    public GridmManager GridsManager; // 這是用來顯示作物圖片的 UI 元件
    public GameObject OnThisGrid;
    void Start()
    {
 
    }
    public void ChangeSprite()
    {
        OnThisGrid.GetComponent<GridCell>().status = CropIndex; // 設定格子狀態為有作物
        OnThisGrid.GetComponent<GridCell>().level = CropLevel;
        Sprite firstSprite = Resources.Load<Sprite>("Source/"+ cropNames[CropIndex,CropLevel]);
        SpriteRenderer U_Sprite = GetComponentInChildren<SpriteRenderer>();
        U_Sprite.sprite = firstSprite;

        if (CropLevel == 3 && CropIndex!=0)
        {
            HaveCoin= GridsManager.save.cropLevel[CropIndex];
           
            U_Sprite.color = new Color(1f, 0.9f, 0.5f, 1f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToOtherGrid(int x, int y)
    {
        int NearstX= int.MaxValue;
        int NearstY= int.MaxValue;
        float NearstDistance =float.MaxValue;

        for (int i = 0; i < GridsManager.width; i++)
        {
            for (int j=0;j< GridsManager.height; j ++)
            {
                if (GridsManager.GridPrefabs[i, j].GetComponent<GridCell>().isOpen==true && GridsManager.GridPrefabs[i,j].GetComponent<GridCell>().Crop == null)
                {
                    float distance = Vector2.Distance(new Vector2(x, y), new Vector2(i, j));
                    if (NearstDistance > distance)
                    {
                        NearstDistance = distance;
                        NearstX = i;
                        NearstY = j;
                    }
                }
            }
        }
        if (NearstDistance != -1)
        {
            OnThisGrid.GetComponent<GridCell>().Crop = null;
            this.GetComponent<Draggable>().MoveToGrid(GridsManager.GridPrefabs[NearstX, NearstY].GetComponent<Collider2D>());
            GridsManager.GridPrefabs[NearstX, NearstY].GetComponent<GridCell>().Crop = gameObject;

            
        }

    }

  
    public void GetRandomCrop(int PlayerLevel)
    {
        CropIndex = Random.Range(1, PlayerLevel+1);// cropNames.GetLength(0));
        int[] levelProbability= new int[] { 0,0,0,0,0,0,0,0,0,0,0, 0, 1,1,2}; // 每個等級的機率百分比
        CropLevel = levelProbability[Random.Range(0, levelProbability.Length)]; // 0-3 隨機等級
    }
}
