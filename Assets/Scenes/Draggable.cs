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

    void Start()
    {
     
    }
    public void OnPressed()
    {

        isDragging = true;
        TheCamera.DragCrop = true;
        TheCamera.Crop = gameObject;

        if ( GetComponent<Farm>().CropIndex != 0 && GetComponent<Farm>().CropLevel == 3 && GetComponent<Farm>().HaveCoin != 0)
            {

            while (GetComponent<Farm>().HaveCoin > 0)
            {
                Collider2D NearestEmptyGrid = NocolliderGetNearestGrid();
                if (NearestEmptyGrid != null)
                {
                    GetComponent<Farm>().HaveCoin--;
                    //Collider2D NearestEmptyGrid = NocolliderGetNearestGrid();

                    SpawnSpecifyCrop(NearestEmptyGrid.GetComponent<GridCell>().x, NearestEmptyGrid.GetComponent<GridCell>().y, 0, 0);

                    //GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                }
                else {
                    break;
                }
                  
            }
            if (GetComponent<Farm>().HaveCoin <= 0)
            {
                GetComponentInChildren<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
            }
        }

        // 計算點下去的 offset（滑鼠和物件之間的差距）
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = transform.position.z; // 維持 Z 軸不變
        offset = transform.position - mouseWorldPos;



        if (GetComponent<Farm>().OnThisGrid != null)
        {
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
            if (TouchIndex == 0)
            {
                exchangeValue(TouchIndex);
                return;
            }
            if (TouchIndex != 0 && GetComponent<Farm>().CropLevel == 3)
            {
                exchangeValue(TouchIndex);
                return;
            }
            GridManager.GetComponent<GridmManager>().ShowFacilityNotify(TouchIndex);
        }
        
        Collider2D NearestTriggerGrid = GetNearestGrid();//獲取當前碰直撞距離最近的格子(物件)


        if (NearestTriggerGrid != null)
        {
            if (NearestTriggerGrid.GetComponent<GridCell>().Crop != null && NearestTriggerGrid.GetComponent<GridCell>().status == GetComponent<Farm>().CropIndex && NearestTriggerGrid.GetComponent<GridCell>().level == GetComponent<Farm>().CropLevel &&( NearestTriggerGrid.GetComponent<GridCell>().level<3 || (NearestTriggerGrid.GetComponent<GridCell>().status == 0 && NearestTriggerGrid.GetComponent<GridCell>().level < 5)))//同樣物品進行合成
            {
                Queue<GameObject> SameCropCells = new Queue<GameObject>()  ;
                SameCropCells = GridManager.GetComponent<GridmManager>().SearchSameCrop(SameCropCells, NearestTriggerGrid.GetComponent<GridCell>().x, NearestTriggerGrid.GetComponent<GridCell>().y);//, new HashSet<(int, int)>()); // 搜索同樣作物
                Debug.Log($"數量:{SameCropCells.Count}");
                if (SameCropCells.Count >= 2)
                {
                    int LevelingAmount = (SameCropCells.Count+1) / 5 * 2 + ((SameCropCells.Count+1)%5)/ 3;
                    int LeaveAmount = (SameCropCells.Count+1)%5%3;
                    Debug.Log($"升級數量:{LevelingAmount}, 剩餘數量{LeaveAmount}");
                    foreach (var item in SameCropCells)
                    {
                        Debug.Log($"result:{item.GetComponent<GridCell>().x},{item.GetComponent<GridCell>().y}");
                        
                        item.GetComponent<GridCell>().Crop.GetComponent<Moving>().StartMoving(true, transform.position);
                        DestroyCrop(item);
                    }
                    GridManager.GetComponent<GridmManager>().CropAmount = GridManager.GetComponent<GridmManager>().CropAmount - (SameCropCells.Count + 1);

                    MoveToGrid(NearestTriggerGrid);
                    NearestTriggerGrid.GetComponent<GridCell>().Crop = gameObject;
                    
                    
                    
                    GetComponent<Farm>().CropLevel++; // 合成後等級提升
                    GetComponent<Farm>().ChangeSprite(); // 更新圖片
                    for (int i = 0; i < LevelingAmount-1; i++)
                    {
                        Collider2D NearestEmptyGrid = NocolliderGetNearestGrid();

                        SpawnSpecifyCrop(NearestEmptyGrid.GetComponent<GridCell>().x, NearestEmptyGrid.GetComponent<GridCell>().y, GetComponent<Farm>().CropIndex,GetComponent<Farm>().CropLevel);
                    }
                    for (int i = 0; i < LeaveAmount; i++)
                    {
                        Collider2D NearestEmptyGrid = NocolliderGetNearestGrid();


                        SpawnSpecifyCrop(NearestEmptyGrid.GetComponent<GridCell>().x, NearestEmptyGrid.GetComponent<GridCell>().y, GetComponent<Farm>().CropIndex, GetComponent<Farm>().CropLevel-1);

                    }
                   
                    //GridManager.GetComponent<GridmManager>().CropAmount = GridManager.GetComponent<GridmManager>().CropAmount - (SameCropCells.Count+1) + LevelingAmount+ LeaveAmount;//整體作物數量
                }
                else {//同物件 數量不夠
                    GridCell GridCellTmp = NearestTriggerGrid.GetComponent<GridCell>();//要移動上去的那一塊地
                    GameObject TmpCrop = GridCellTmp.Crop;//要移動上去的那一塊地上原本的作物
                    Collider2D NearestTmpEmptyGrid = TmpCrop.GetComponent<Draggable>().NocolliderGetNearestGrid();//要移動上去的那一塊地上原本的作物 離他最近的其他空地
                    Debug.Log($"!!!!!!!!!{NearestTmpEmptyGrid}");
                    TmpCrop.GetComponent<Draggable>().MoveToGrid(NearestTmpEmptyGrid);//該作物移去該空地
                    NearestTmpEmptyGrid.GetComponent<GridCell>().Crop = TmpCrop;

                    NearestTriggerGrid.GetComponent<GridCell>().status = -1;
                    NearestTriggerGrid.GetComponent<GridCell>().level = 0;
                    NearestTriggerGrid.GetComponent<GridCell>().Crop = null;
                    //GridCellTmp.Crop.GetComponent<Draggable>().MoveToGrid(GridCellTmpSecond);

                    //GridCellTmp.Crop = null;
                    //GridManager.GetComponent<GridmManager>().GridPrefabs[NearstX, NearstY].GetComponent<GridCell>().Crop = GridCellTmp.Crop;

                    //!GridCellTmp.Crop.GetComponent<Farm>().MoveToOtherGrid(GridCellTmp.x, GridCellTmp.y);
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

                    NearestTriggerGrid.GetComponent<GridCell>().status = -1;
                    NearestTriggerGrid.GetComponent<GridCell>().level = 0;
                    NearestTriggerGrid.GetComponent<GridCell>().Crop = null;
                    //!GridCellTmp.Crop.GetComponent<Farm>().MoveToOtherGrid(GridCellTmp.x, GridCellTmp.y);

                }

                MoveToGrid(NearestTriggerGrid);
                NearestTriggerGrid.GetComponent<GridCell>().Crop = gameObject;
            }
        }
        else if (NearestTriggerGrid == null)// 沒有碰撞時 獲取的格子中距離最近的空格子
        {
            Collider2D NearestEmptyGrid = NocolliderGetNearestGrid();

            MoveToGrid(NearestEmptyGrid);
            NearestEmptyGrid.GetComponent<GridCell>().Crop = gameObject;
            
        }
        //Debug.Log($"{lastTriggerGrid.GetComponent<GridCell>().Crop.GetComponent<Farm>().CropIndex}");
        
    }
    private void exchangeValue(int facility)
    {
        Farm Crop = GetComponent<Farm>();
        if (Crop.CropIndex == 0)
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
        else if (Crop.CropIndex != 0)
        {
            GridManager.GetComponent<GridmManager>().ChargeCropExp(facility,Crop.CropIndex, Crop.CropLevel);

            GridManager.GetComponent<GridmManager>().CropAmount--;

            Destroy(gameObject); // 刪除作物
            return; //如果是初始作物，直接換算錢錢
        }
    }
    public void SpawnSpecifyCrop(int x, int y, int status, int level)
    {

        GameObject spawned = Instantiate(BoxPrefabs, transform.position, Quaternion.identity);

        //spawnedB.transform.rotation = Quaternion.Euler(0, 0, 0);
        spawned.GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        GridPrefabs[x, y].GetComponent<GridCell>().Crop = spawned;
        spawned.GetComponent<Moving>().StartMoving(false, GridPrefabs[x, y].transform.position);
        spawned.GetComponent<Farm>().OnThisGrid = GridPrefabs[x, y];
        spawned.GetComponent<Farm>().CropIndex = status;
        spawned.GetComponent<Farm>().CropLevel = level;
        spawned.GetComponent<Farm>().ChangeSprite();
        GridManager.GetComponent<GridmManager>().CropAmount++;
        return;
    }

    private void DestroyCrop(GameObject Grid)//刪除作物
    {
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
        // 移動到格子中心
        //GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().Crop = null;

        GetComponent<Moving>().StartMoving(false,grid.bounds.center);
        GetComponent<Farm>().OnThisGrid = grid.gameObject;
        GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().status =  GetComponent<Farm>().CropIndex;
        GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().level = GetComponent<Farm>().CropLevel;

       
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

        if (other.CompareTag("Grid"))
        {
            if (other.GetComponent<GridCell>().isOpen == true)
            {
                collidingGrids.Add(other);
                //other.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.3f); // 白色 + 半透明
            }
        }
           
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Grid"))
        {
            collidingGrids.Remove(other);
            //other.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f); // 白色 + 半透明
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
