using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//作物
public class Farm : MonoBehaviour
{
     private static string[,] cropNames= new string[,]
     {
           { "coin_0","coin_1","coin_2","coin_3","coin_4"},
           { "PaperW_00", "PaperW_01", "PaperW_02", "PaperW_03",""},
           { "Sheep_00", "Sheep_01", "Sheep_02", "Sheep_03",""},
           { "Plus0_00", "Plus0_01", "Plus0_02", "Plus0_03",""},
           { "Pochi_00", "Pochi_01", "Pochi_02", "Pochi_03",""},
           { "Princess_00", "Princess_01", "Princess_02", "Princess_03",""},
           { "SC_00", "SC_01", "SC_02", "SC_03",""},
           { "Riku_00", "Riku_01", "Riku_02", "Riku_03",""},
           { "Taki_00", "Taki_01", "Taki_02", "Taki_03",""},
           { "Goosey_00", "Goosey_01", "Goosey_02", "Goosey_03",""},
           { "Wolf_00", "Wolf_01", "Wolf_02", "Wolf_03",""}
    };

    public int CropIndex=0;
    public int CropLevel=0;
    public int HaveCoin =0;
    public int CropValue = 1;//預設 1等 1塊錢
    public GridmManager GridsManager; // 這是用來顯示作物圖片的 UI 元件
    public GameObject OnThisGrid;
    private PolygonCollider2D col;
    void Start()
    {
 
    }
    public void ChangeSprite()
    {
        Sprite firstSprite = Resources.Load<Sprite>("Source/" + cropNames[CropIndex, CropLevel]);
        SpriteRenderer U_Sprite = GetComponent<SpriteRenderer>();
        U_Sprite.sprite = firstSprite;

        if (col!=null)
            Destroy(col);
        col = gameObject.AddComponent<PolygonCollider2D>();
        col.isTrigger = true;

        if (CropLevel == 3 && CropIndex != 0 && HaveCoin > 0)
        {

            U_Sprite.color = new Color(1f, 0.9f, 0.5f, 1f);
        }
        if (CropLevel == 3 && CropIndex != 0 && HaveCoin == 0)
        {

            U_Sprite.color = new Color(0.5f, 0.5f, 0.5f, 1f);
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
