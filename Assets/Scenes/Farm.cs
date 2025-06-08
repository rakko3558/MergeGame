using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//作物
public class Farm : MonoBehaviour
{

     private static string[,] cropNames= new string[,]
        {
           { "coin0","coin1","coin2","coin3","coin4","coin5","coin6","coin7","coin_C", "coin_C5", "coin_S", "coin_S5","coin_G","coin_G5"},
           { "ts0", "ts1", "ts2", "ts3","","","","","","","","","",""},
           { "ylr0", "ylr1", "ylr2", "ylr3","","","","","","","","","",""},
           { "uu0", "uu1", "uu2", "uu3","","","","","","","","","",""},
           { "tk0", "tk1", "tk2", "tk3","","","","","","","","","",""},
           { "t00", "t01", "t02", "t03","","","","","","","","","",""},
           { "sc0", "sc1", "sc2", "sc3","","","","","","","","","",""},
           { "rik0", "rik1", "rik2", "rik3","","","","","","","","","",""},
           { "pkc0", "pkc1", "pkc2", "pkc3","","","","","","","","","",""},
           { "pj0", "pj1", "pj2", "pj3","","","","","","","","","",""},
           { "gz0", "gz1", "gz2", "gz3","","","","","","","","","",""}
        };

    //public GameObject image;

    public int PlayerLevel = 1;
    public int CropIndex=0;
    public int CropLevel=0;
    public bool HaveCoin=false;
    public int CropValue = 1;//預設 1等 1塊錢
    public GameObject GridsManager; // 這是用來顯示作物圖片的 UI 元件
    public GameObject OnThisGrid;
    void Start()
    {
        /*
        Sprite firstSprite = Resources.Load<Sprite>("Source/Lighting");
        SpriteRenderer U_Sprite = image.GetComponent<SpriteRenderer>();
        U_Sprite.sprite = firstSprite;*/
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
            HaveCoin=true;
            U_Sprite.color = new Color(1f, 0.9f, 0.5f, 1f);
            Debug.Log($"第四階{HaveCoin}");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToOtherGrid(int x, int y)
    {
        GridmManager Grids = GridsManager.GetComponent<GridmManager>();
        // 移動到新的位置
        //int max=Grids.width>Grids.height? Grids.width : Grids.height;
        //先偵測所有空格子
        //List<GridCell> EmptyGrid = new List<GridCell>();
        int NearstX= int.MaxValue;
        int NearstY= int.MaxValue;
        float NearstDistance =float.MaxValue;
        for (int i = 0; i < Grids.width; i++)
        {
            for (int j=0;j< Grids.height; j ++)
            {
                if (Grids.GridPrefabs[i,j].GetComponent<GridCell>().Crop == null)
                {
                    float distance = Vector2.Distance(new Vector2(x, y), new Vector2(i, j));
                    if (NearstDistance > distance)
                    {
                        NearstDistance = distance;
                        NearstX = i;
                        NearstY = j;
                    }
                    //EmptyGrid.Add(Grids.GridPrefabs[i,j].GetComponent<GridCell>());
                }
            }
        }
        //Debug.Log($"Distance:{NearstDistance}");
        if (NearstDistance != -1)
        {
            OnThisGrid.GetComponent<GridCell>().Crop = null;
            this.GetComponent<Draggable>().MoveToGrid(Grids.GridPrefabs[NearstX, NearstY].GetComponent<Collider2D>());
            Grids.GridPrefabs[NearstX, NearstY].GetComponent<GridCell>().Crop = gameObject;

            //Debug.Log($"{Grids.GridPrefabs[NearstX, NearstY].GetComponent<GridCell>().Crop.GetComponent<Farm>().CropIndex}");
        }
        /*else
        { 
            Debug.Log("沒有空格子可以放置物品了！");//不可以發生
        }*/
        //GridsManager.getComponent<GridmManager>().GridPrefabs;

    }

    /*
    public void SpawnRandomCrop()
    {
        // 初始圖片
        string firstCrop = GetRandomCrop();
        Sprite firstSprite = Resources.Load<Sprite>("Crops/" + firstCrop);
        if (firstSprite == null)
        {
            Debug.LogError("找不到圖片：" + firstCrop);
            return;
        }

        // 建立 UI 圖片
        GameObject cropUI = Instantiate(Resources.Load<GameObject>("Prefabs/CropUI"), spawnParent);
        SpriteRender image = image.GetComponent<SpriteRender>();
        image.sprite = firstSprite;

        // 啟動協程：延遲換圖
        StartCoroutine(ChangeImageAfterDelay(image));
    }
    
    private IEnumerator ChangeImageAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);

        // 換一張不同的圖片
        string newCrop;
        Sprite newSprite;
        do
        {
            newCrop = GetRandomCrop();
            newSprite = Resources.Load<Sprite>("Crops/" + newCrop);
        }
        while (newSprite == null || newSprite == image.sprite); // 避免跟原來一樣

        image.sprite = newSprite;
    }
    */
    public void GetRandomCrop()
    {
        CropIndex = Random.Range(1, PlayerLevel+1);// cropNames.GetLength(0));
    }
}
