using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
//拖移作物
public class Draggable : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    //private Collider2D lastTriggerGrid; // 記錄最後接觸的格子
    public List<Collider2D> collidingGrids= new List<Collider2D>(); // 用來存放所有碰撞的物件
    private int TouchIndex = -1; // 用來判斷是否碰撞到銀行
    public CameraDrag TheCamera;
    public GameObject GridManager; // 用來存放格子管理器
    private GameObject[,] GridPrefabs;
    public GameObject BoxPrefabs;
    public PlaySound DragAudio ;


    void Start()
    { DragAudio = GridManager.GetComponent<PlaySound>();
    }  
    public void OnPressed()
    {
        DragAudio.Sound(0);
        isDragging = true;
        TheCamera.DragCrop = true;
        TheCamera.Crop = gameObject;

        if ( GetComponent<Farm>().CropIndex != 0 && GetComponent<Farm>().CropLevel == 3 && GetComponent<Farm>().HaveCoin != 0)
            {
            if (GetComponent<Farm>().HaveCoin > 0)
            {
                while (GetComponent<Farm>().HaveCoin > 0)
                {
                    Collider2D NearestEmptyGrid = NocolliderGetNearestGrid();
                    if (NearestEmptyGrid != null)
                    {
                        GetComponent<Farm>().HaveCoin--;
                        //Collider2D NearestEmptyGrid = NocolliderGetNearestGrid();
                        int x = NearestEmptyGrid.GetComponent<GridCell>().x;
                        int y = NearestEmptyGrid.GetComponent<GridCell>().y;
                        int x1 = GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().x;
                        int y1 = GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().y;
                        GridManager.GetComponent<GridmManager>().SpawnSpecifyCrop(x, y,x1,y1, 0, 0, 0);

                        //GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                    }
                    else
                    {
                        break;
                    }

                }
                GridManager.GetComponent<GridmManager>().save.UpdateCropCoin(GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().x*10+ GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().y, GetComponent<Farm>().HaveCoin);
            }
            if (GetComponent<Farm>().HaveCoin <= 0)
            {

                Destroy(GetComponent<Farm>().thisSparkle);
                GetComponentInChildren<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
            }
        }

        // 計算點下去的 offset（滑鼠和物件之間的差距）
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = transform.position.z; // 維持 Z 軸不變
        offset = transform.position - mouseWorldPos;



        if (GetComponent<Farm>().OnThisGrid != null)
        {
            GridManager.GetComponent<GridmManager>().save.UpdateCrop(GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().x * 10 + GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().y, -1, 0, 0);

            GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().status = -1;
            GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().level = 0;
            GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().Crop = null;// 清除原格子引用
            GetComponent<Farm>().OnThisGrid = null; //清除原格子上的作物引用 

        }
    }
    public void OnReleased()
    {
        isDragging = false;
        TheCamera.DragCrop = false;
        TheCamera.Crop = null;

        if (TouchIndex!=-1)//兌換價值
        {
            if ( GetComponent<Farm>().CropIndex == 0 && TouchIndex == 0)
            {
                exchangeValue(TouchIndex);
                return;
            }
            if (GetComponent<Farm>().CropLevel == 3 && GetComponent<Farm>().CropIndex != 0)
            {
                exchangeValue(TouchIndex);
                return;
            }
            if (TouchIndex == 999&& GetComponent<Farm>().CropIndex != 0)
            {
                exchangeValue(TouchIndex);
                return;
            }
            if (TouchIndex == 888 && GetComponent<Farm>().CropIndex == 1)
            {
                exchangeValue(TouchIndex);
                return;
            }
            if(TouchIndex != 888)
               if (GetComponent<Farm>().CropIndex != 0 && GetComponent<Farm>().CropLevel < 3)
                GridManager.GetComponent<GridmManager>().ShowFacilityNotify(TouchIndex);
        }
        
        Collider2D NearestTriggerGrid = GetNearestGrid();//獲取當前碰直撞距離最近的格子(物件)


        if (NearestTriggerGrid != null)//放上去的格子有東西
        {
            if (NearestTriggerGrid.GetComponent<GridCell>().Crop != null && NearestTriggerGrid.GetComponent<GridCell>().status == GetComponent<Farm>().CropIndex && NearestTriggerGrid.GetComponent<GridCell>().level == GetComponent<Farm>().CropLevel && (NearestTriggerGrid.GetComponent<GridCell>().level < 3 || (NearestTriggerGrid.GetComponent<GridCell>().status == 0 && NearestTriggerGrid.GetComponent<GridCell>().level < 4)))//同樣物品進行合成
            {
                Queue<GameObject> SameCropCells = new Queue<GameObject>();
                SameCropCells = GridManager.GetComponent<GridmManager>().SearchSameCrop(SameCropCells, NearestTriggerGrid.GetComponent<GridCell>().x, NearestTriggerGrid.GetComponent<GridCell>().y);//, new HashSet<(int, int)>()); // 搜索同樣作物
                //Debug.Log($"數量:{SameCropCells.Count}");
                if (SameCropCells.Count >= 2) //同物件 數量3個以上 融合
                {
                    int LevelingAmount = (SameCropCells.Count + 1) / 5 * 2 + ((SameCropCells.Count + 1) % 5) / 3;
                    int LeaveAmount = (SameCropCells.Count + 1) % 5 % 3;
                    //Debug.Log($"升級數量:{LevelingAmount}, 剩餘數量{LeaveAmount}");
                    foreach (var item in SameCropCells)
                    {
                        //Debug.Log($"result:{item.GetComponent<GridCell>().x},{item.GetComponent<GridCell>().y}");

                        item.GetComponent<GridCell>().Crop.GetComponent<Moving>().StartMoving(true, transform.position);
                        DestroyCrop(item);
                    }
                    GridManager.GetComponent<GridmManager>().CropAmount = GridManager.GetComponent<GridmManager>().CropAmount - (SameCropCells.Count + 1);





                    GetComponent<Farm>().CropLevel++; // 合成後等級提升
                    if (GetComponent<Farm>().CropLevel == 3 && GetComponent<Farm>().CropIndex != 0)
                    {
                        GetComponent<Farm>().HaveCoin = GridManager.GetComponent<GridmManager>().save.data.cropLevel[GetComponent<Farm>().CropIndex];
                    }
                    GetComponent<Farm>().ChangeSprite(); // 更新圖片

                    MoveToGrid(NearestTriggerGrid);//連著以上更新的資訊一起存檔
                    NearestTriggerGrid.GetComponent<GridCell>().Crop = gameObject;
                    for (int i = 0; i < LevelingAmount - 1; i++)
                    {

                        Collider2D NearestEmptyGrid = NocolliderGetNearestGrid();
                        int x = NearestEmptyGrid.GetComponent<GridCell>().x;
                        int y = NearestEmptyGrid.GetComponent<GridCell>().y;
                        int x1 = GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().x;
                        int y1 = GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().y;
                        GridManager.GetComponent<GridmManager>().SpawnSpecifyCrop(x, y, x1, y1, GetComponent<Farm>().CropIndex, GetComponent<Farm>().CropLevel, GetComponent<Farm>().HaveCoin);
                    }

                    for (int i = 0; i < LeaveAmount; i++)
                    {
                        Collider2D NearestEmptyGrid = NocolliderGetNearestGrid();

                        int x = NearestEmptyGrid.GetComponent<GridCell>().x;
                        int y = NearestEmptyGrid.GetComponent<GridCell>().y;
                        int x1 = GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().x;
                        int y1 = GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().y;

                        GridManager.GetComponent<GridmManager>().SpawnSpecifyCrop(x, y, x1, y1, GetComponent<Farm>().CropIndex, GetComponent<Farm>().CropLevel - 1, GetComponent<Farm>().HaveCoin);
                    }

                }
                else
                {//同物件 數量不夠 互換位置
                    GridCell GridCellTmp = NearestTriggerGrid.GetComponent<GridCell>();//要移動上去的那一塊地
                    GameObject TmpCrop = GridCellTmp.Crop;//要移動上去的那一塊地上原本的作物
                    Collider2D NearestTmpEmptyGrid = TmpCrop.GetComponent<Draggable>().NocolliderGetNearestGrid();//要移動上去的那一塊地上原本的作物 離他最近的其他空地

                    TmpCrop.GetComponent<Draggable>().MoveToGrid(NearestTmpEmptyGrid);//該作物移去該空地
                    NearestTmpEmptyGrid.GetComponent<GridCell>().Crop = TmpCrop;


                    MoveToGrid(NearestTriggerGrid);
                    GridCellTmp.Crop = gameObject;
                }
                
            }
            else
            {
                if (NearestTriggerGrid.GetComponent<GridCell>().Crop != null)
                {
                    GridCell GridCellTmp = NearestTriggerGrid.GetComponent<GridCell>();//要移動上去的那一塊地
                    GameObject TmpCrop = GridCellTmp.Crop;//要移動上去的那一塊地上原本的作物
                    Collider2D NearestTmpEmptyGrid = TmpCrop.GetComponent<Draggable>().NocolliderGetNearestGrid();//要移動上去的那一塊地上原本的作物 離他最近的其他空地

                    TmpCrop.GetComponent<Draggable>().MoveToGrid(NearestTmpEmptyGrid);//該作物移去該空地
                    NearestTmpEmptyGrid.GetComponent<GridCell>().Crop = TmpCrop;


                }

                MoveToGrid(NearestTriggerGrid);
                NearestTriggerGrid.GetComponent<GridCell>().Crop = gameObject;
            }
        }
        else if (NearestTriggerGrid == null)// 沒有碰撞時 獲取的格子中距離最近的空格子 //放上去的格子沒東西
            {
                Collider2D NearestEmptyGrid = NocolliderGetNearestGrid();

                MoveToGrid(NearestEmptyGrid);
                NearestEmptyGrid.GetComponent<GridCell>().Crop = gameObject;

        }
        
    }
    private void exchangeValue(int facility)
    {
        Farm Crop = GetComponent<Farm>();
        if (Crop.CropIndex == 0 && facility==0)
        {
            int initailChange = 1;
            for (int i = 0; i < Crop.CropLevel; i++)
            {
                initailChange *= 5;
            }
            GridManager.GetComponent<GridmManager>().ChargeBank(initailChange);
            GridManager.GetComponent<GridmManager>().CropAmount--;
            Destroy(gameObject); // 刪除作物

            return; //如果是初始作物，直接換算錢錢
        }
        else if (Crop.CropIndex != 0 && Crop.CropLevel==3 && facility != 888 && facility != 999)
        {
            GridManager.GetComponent<GridmManager>().ChargeCropExp(facility,Crop.CropIndex, Crop.CropLevel);

            GridManager.GetComponent<GridmManager>().CropAmount--;
           
            Destroy(gameObject); // 刪除作物
            return; //如果是初始作物，直接換算錢錢
        }
        else if (Crop.CropIndex != 0 && facility == 999)
        {
            for (int i = 0; i < GridManager.GetComponent<GridmManager>().save.data.Lands; i++)
            {
                GridCell Script = GridManager.GetComponent<GridmManager>().GridPrefabs[i / 10, i % 10].GetComponent<GridCell>();
                if (Script.Crop == null)
                {
                    GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                    GridManager.GetComponent<GridmManager>().GridPrefabs[i / 10, i % 10].GetComponent<GridCell>().Crop = gameObject;
                    GetComponent<Moving>().StartMoving(false, GridManager.GetComponent<GridmManager>().GridPrefabs[i / 10, i % 10].transform.position);
                    GetComponent<Farm>().OnThisGrid = GridManager.GetComponent<GridmManager>().GridPrefabs[i / 10, i % 10];
                    GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().status = 0;
                    GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().level = 0;
                    GetComponent<Farm>().CropIndex = 0;
                    GetComponent<Farm>().CropLevel = 0;
                    GetComponent<Farm>().HaveCoin = 0;
                    GetComponent<Farm>().ChangeSprite();
                    GridManager.GetComponent<GridmManager>().save.UpdateCrop(i / 10 + i, 0, 0, 0);
                    break; 
                }
            }

            return;
        }
        else if (Crop.CropIndex ==1 && facility == 888)
        {
            for (int i = 0; i < GridManager.GetComponent<GridmManager>().save.data.Lands; i++)
            {
                GridCell Script = GridManager.GetComponent<GridmManager>().GridPrefabs[i / 10, i % 10].GetComponent<GridCell>();
                if (Script.Crop == null)
                {
                    GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                    GridManager.GetComponent<GridmManager>().GridPrefabs[i / 10, i % 10].GetComponent<GridCell>().Crop = gameObject;
                    GetComponent<Moving>().StartMoving(false, GridManager.GetComponent<GridmManager>().GridPrefabs[i / 10, i % 10].transform.position);
                    GetComponent<Farm>().OnThisGrid = GridManager.GetComponent<GridmManager>().GridPrefabs[i / 10, i % 10];
                    GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().status = 0;
                    GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().level = 2;
                    GetComponent<Farm>().CropIndex = 0;
                    GetComponent<Farm>().CropLevel = 2;
                    GetComponent<Farm>().HaveCoin = 0;
                    GetComponent<Farm>().ChangeSprite();
                    GridManager.GetComponent<GridmManager>().save.UpdateCrop(i / 10 + i, 0, 2, 0);
                    break;
                }
            }

            return;
        }
    }
    private void DestroyCrop(GameObject Grid)//刪除作物
    {    
        GridManager.GetComponent<GridmManager>().save.UpdateCrop(Grid.GetComponent<GridCell>().x*10 + Grid.GetComponent<GridCell>().y,- 1,0,0);
 
        Grid.GetComponent<GridCell>().status = -1;
        Grid.GetComponent<GridCell>().level = 0;
        Grid.GetComponent<GridCell>().Crop = null;
       }
    // 獲取當前碰撞的格子中距離最近的格子
    private Collider2D GetNearestGrid()
    {
        float nearestDistance = float.MaxValue;
        Collider2D nearestGrid = null;

        for (int i = 0; i < collidingGrids.Count; i++)
        {
           
            Collider2D grid = collidingGrids[i];
            float distance = Vector2.Distance(transform.position, grid.bounds.center);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestGrid = grid;
            }
            
        }
        return nearestGrid;
    }
    // 沒有碰撞時 獲取的格子中距離最近的空格子
    private Collider2D NocolliderGetNearestGrid()
    {
        GridPrefabs = GridManager.GetComponent<GridmManager>().GridPrefabs;
        float nearestDistance = float.MaxValue;
        Collider2D nearestGrid = null;
        int width = GridManager.GetComponent<GridmManager>().width;
        int height = GridManager.GetComponent<GridmManager>().height;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (GridPrefabs[i, j].GetComponent<GridCell>().Crop == null && GridPrefabs[i, j].GetComponent<GridCell>().isOpen==true)
                {
                    Collider2D grid = GridPrefabs[i, j].GetComponent<Collider2D>();
                    float distance = Vector2.Distance(transform.position, grid.bounds.center);
                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestGrid = grid;
                    }
                }
                else 
                {
                    //Debug.Log($"沒有空位");
                }

            }
        }
        return nearestGrid;
    }
    public void MoveToGrid(Collider2D grid)
    {
        GetComponent<Moving>().StartMoving(false,grid.bounds.center);
        GetComponent<Farm>().OnThisGrid = grid.gameObject;
        GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().status =  GetComponent<Farm>().CropIndex;
        GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().level = GetComponent<Farm>().CropLevel;
        GridManager.GetComponent<GridmManager>().save.UpdateCrop(GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().x * 10 + GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().y, GetComponent<Farm>().CropIndex, GetComponent<Farm>().CropLevel, GetComponent<Farm>().HaveCoin);


    }
    void OnTriggerEnter2D(Collider2D other)
    { 
        if (other.CompareTag("bank"))
        {
            TouchIndex = 0;
        }
        if (other.CompareTag("marry"))
        {
            TouchIndex = 1;
        }
        if (other.CompareTag("stall"))
        {
            TouchIndex = 2;
        }
        if (other.CompareTag("concert"))
        {
            TouchIndex = 3;
        }
        if (other.CompareTag("guagua"))
        {
            TouchIndex = 999;


        }
        if (other.CompareTag("rong"))
        {
            TouchIndex = 888;


        }
        if (other.CompareTag("ACow"))
        {
            TouchIndex = 4;
        }
        if (other.CompareTag("WolfGang"))
        {
            TouchIndex = 5;
        }
        if (other.CompareTag("Kaede"))
        {
            TouchIndex = 6;
        }
        if (other.CompareTag("Grid"))
        {
            if (other.GetComponent<GridCell>().isOpen == true)
            {
                collidingGrids.Add(other);
            }
        }

        Debug.Log($"{GetComponent<Farm>().CropIndex}碰到{TouchIndex}！！");

    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Grid"))
        {
            collidingGrids.Remove(other);
           
        }

        if (other.CompareTag("bank"))
        {
            TouchIndex = -1;
        }
        if (other.CompareTag("marry"))
        {
            TouchIndex = -1;
        }
        if (other.CompareTag("stall"))
        {
            TouchIndex = -1;
        }
        if (other.CompareTag("concert"))
        {
            TouchIndex = -1;
        }
        if (other.CompareTag("guagua"))
        {
            TouchIndex = -1;
        }
        if (other.CompareTag("ACow"))
        {
            TouchIndex = -1;
        }
        if (other.CompareTag("rong"))
        {
            TouchIndex = -1;


        }
        if (other.CompareTag("WolfGang"))
        {
            TouchIndex = -1;
        }
        if (other.CompareTag("Kaede"))
        {
            TouchIndex =-1;
        }

    }
    void Update()
    {
       

        if (isDragging)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = transform.position.z; // 維持原來 Z
            transform.position = mouseWorldPos + offset;

        }
    }
}


