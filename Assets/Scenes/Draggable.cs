using UnityEngine;
using System.Collections.Generic;
//拖移作物
public class Draggable : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    //private Collider2D lastTriggerGrid; // 記錄最後接觸的格子
    public List<Collider2D> collidingGrids= new List<Collider2D>(); // 用來存放所有碰撞的物件

    public GameObject GridManager; // 用來存放格子管理器
    private GameObject[,] GridPrefabs;
    public GameObject BoxPrefabs;
    void Start()
    {
        //GridPrefabs = GridManager.GetComponent<GridmManager>().GridPrefabs;
        /*
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {

            if (col is BoxCollider2D box)
            {
                Debug.Log($"{box.transform.position}");
            }
        }
        else { Debug.Log($"NON"); }*/

    }
    public void OnPressed()
    {
        // 點下去時開始拖曳
        isDragging = true;
        //Debug.Log($"clicked");
        // 計算點下去的 offset（滑鼠和物件之間的差距）
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = transform.position.z; // 維持 Z 軸不變
        offset = transform.position - mouseWorldPos;



        //lastTriggerGrid = GetComponent<Farm>().OnThisGrid.GetComponent<Collider2D>(); // 更新最後接觸的格子
        /*if (GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>() == null) 
        {
            Debug.Log($"該作物沒有在格子上");
        }*/
        if (GetComponent<Farm>().OnThisGrid != null)
        {
            GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().status = 0;
            GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>()    .level = 0;
            GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().Crop = null;// 清除原格子引用
            GetComponent<Farm>().OnThisGrid = null; //清除原格子上的作物引用 
        }
    }

    public void OnReleased()
    {
        // 放開滑鼠停止拖曳

        Collider2D NearestTriggerGrid = GetNearestGrid();//獲取當前碰直撞距離最近的格子
        if (NearestTriggerGrid != null)
        {
            if (NearestTriggerGrid.GetComponent<GridCell>().Crop != null && NearestTriggerGrid.GetComponent<GridCell>().status == GetComponent<Farm>().CropIndex && NearestTriggerGrid.GetComponent<GridCell>().level == GetComponent<Farm>().CropLevel && NearestTriggerGrid.GetComponent<GridCell>().level<3)//同樣物品進行合成
            {
                Queue<GameObject> SameCropCells = new Queue<GameObject>()  ;
                SameCropCells = GridManager.GetComponent<GridmManager>().SearchSameCrop(SameCropCells, NearestTriggerGrid.GetComponent<GridCell>().x, NearestTriggerGrid.GetComponent<GridCell>().y);//, new HashSet<(int, int)>()); // 搜索同樣作物

                if (SameCropCells.Count >= 2)
                {
                    int LevelingAmount = (SameCropCells.Count+1) / 5 * 2 + ((SameCropCells.Count+1)%5)/ 3;
                    int LeaveAmount = (SameCropCells.Count+1)%5%3;
                    Debug.Log($"升級數量:{LevelingAmount}, 剩餘數量{LeaveAmount}");
                    foreach (var item in SameCropCells)
                    {
                        //Debug.Log($"result:{item.GetComponent<GridCell>().x},{item.GetComponent<GridCell>().y}");
                        DestroyCrop(item);
                    }
                    GridManager.GetComponent<GridmManager>().CropAmount = GridManager.GetComponent<GridmManager>().CropAmount - (SameCropCells.Count + 1);

                    MoveToGrid(NearestTriggerGrid);
                    NearestTriggerGrid.GetComponent<GridCell>().Crop = gameObject;
                    for (int i = 0; i < LeaveAmount; i++)
                    {
                        Collider2D NearestEmptyGrid = NocolliderGetNearestGrid();


                        SpawnSpecifyCrop(NearestEmptyGrid.GetComponent<GridCell>().x, NearestEmptyGrid.GetComponent<GridCell>().y, GetComponent<Farm>().CropIndex, GetComponent<Farm>().CropLevel);

                    }

                    GetComponent<Farm>().CropLevel++; // 合成後等級提升
                    GetComponent<Farm>().ChangeSprite(); // 更新圖片

                    for (int i = 0; i < LevelingAmount-1; i++)
                    {
                        Collider2D NearestEmptyGrid = NocolliderGetNearestGrid();

                        SpawnSpecifyCrop(NearestEmptyGrid.GetComponent<GridCell>().x, NearestEmptyGrid.GetComponent<GridCell>().y, GetComponent<Farm>().CropIndex,GetComponent<Farm>().CropLevel);
                    }
                    
                    //GridManager.GetComponent<GridmManager>().CropAmount = GridManager.GetComponent<GridmManager>().CropAmount - (SameCropCells.Count+1) + LevelingAmount+ LeaveAmount;//整體作物數量
                }
                else {
                    GridCell GridCellTmp = NearestTriggerGrid.GetComponent<GridCell>();
                    GridCellTmp.Crop.GetComponent<Farm>().MoveToOtherGrid(GridCellTmp.x, GridCellTmp.y);
                    MoveToGrid(NearestTriggerGrid);
                    NearestTriggerGrid.GetComponent<GridCell>().Crop = gameObject;
                }

                //NearestTriggerGrid.GetComponent<GridCell>().GridCropMergeSearch(0); // 合成格子上的作物
                //NearestTriggerGrid.GetComponent<GridCell>().GridCropMergeSearch();
                //Debug.Log($"{NearestTriggerGrid.GetComponent<GridCell>().status},{ NearestTriggerGrid.GetComponent<GridCell>().level}");
            }
            else
            {
                if (NearestTriggerGrid.GetComponent<GridCell>().Crop != null)
                {


                    //Debug.Log($"Print:{lastTriggerGrid.GetComponent<GridCell>().Crop.GetComponent<Farm>().CropIndex}");
                    GridCell GridCellTmp = NearestTriggerGrid.GetComponent<GridCell>();
                    //Debug.Log($"GridCell 座標為：({GridCellTmp.x}, {GridCellTmp.y})");
                    GridCellTmp.Crop.GetComponent<Farm>().MoveToOtherGrid(GridCellTmp.x, GridCellTmp.y);

                }
                /*  else
                  {
                      Debug.Log($"lastTriggerGrid.Crop is NULL");
                  }
          */

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
        isDragging = false;
    }

    public void SpawnSpecifyCrop(int x, int y, int status, int level)
    {

        GameObject spawned = Instantiate(BoxPrefabs, GridPrefabs[x, y].transform.position, Quaternion.identity);
        //spawnedB.transform.rotation = Quaternion.Euler(0, 0, 0);

        GridPrefabs[x, y].GetComponent<GridCell>().Crop = spawned;

        spawned.GetComponent<Farm>().OnThisGrid = GridPrefabs[x, y];
        spawned.GetComponent<Farm>().CropIndex = status;
        spawned.GetComponent<Farm>().CropLevel = level;
        spawned.GetComponent<Farm>().ChangeSprite();
        GridManager.GetComponent<GridmManager>().CropAmount++;
        return;
    }

    private void DestroyCrop(GameObject Grid)//刪除作物
    {
        Grid.GetComponent<GridCell>().status = 0;
        Grid.GetComponent<GridCell>().level = 0;
        Destroy(Grid.GetComponent<GridCell>().Crop); // 刪除作物實體
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
                if (GridPrefabs[i, j].GetComponent<GridCell>().Crop == null)
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
                    Debug.Log($"沒有空位");
                }

            }
        }
        return nearestGrid;
    }

    public int merge()
    {
        return 0; 
    }

    public void MoveToGrid(Collider2D grid)
    {
        // 移動到格子中心
        //GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().Crop = null;
        transform.position = grid.bounds.center;
        GetComponent<Farm>().OnThisGrid = grid.gameObject;
        GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().status =  GetComponent<Farm>().CropIndex;
        GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().level = GetComponent<Farm>().CropLevel;
    }
    //碰撞


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Grid"))
        {
            collidingGrids.Add(other);
        }
           
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Grid"))
        {
            collidingGrids.Remove(other);
        }
    }
    /*
    void OnTriggerStay2D(Collider2D other)
    {
       
        //Debug.Log($"({other.name})");
        if (other.CompareTag("Grid"))
        {
            lastTriggerGrid = other;
         
        }
    }*/

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
