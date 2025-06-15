using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.EventSystems;
//複製格子
public class GridmManager : MonoBehaviour
{

    public int width = 20 ;
    public int height = 20 ;
    public GameObject cellPrefab;
    public float cellSpacing = 1f;
    public float originPosition = 0f;
    public GameObject Buttom;

    public GameObject OpenLandButtom;
    public Storage save;
    public GameObject[,] GridPrefabs;
    //public List<GameObject> CropdPrefabs; //記錄所有在場作物
    public int CropAmount = 0; // 作物數量
    //public List<GameObject> GridPrefabs;
    // Start is called before the first frame update
    public int Lands=0;
    public TextMeshPro OpenLandText;

    public LayerMask clickableLayer;
    public GameObject PressedObject;

    public Facilitys[] facilityArray;

    void Start()
    {
        
        GenerateGrid();
     
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject())
        // 滑鼠左鍵點擊時執行
        if (Input.GetMouseButtonDown(0))
        {
            // 取得滑鼠位置，轉為世界座標
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 射出一條 Raycast (只有偵測設定的 Layer)
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, clickableLayer);

            if (hit.collider != null)
            {
               // Debug.Log("點擊到：" + hit.collider.gameObject.name);

                // 你可以根據 Tag 或元件類型來做事情
                //if (hit.collider.CompareTag("Clickable"))
                //{
                    // 做作物的點擊處理
                 hit.collider.GetComponent<Draggable>()?.OnPressed(); // 假設你有 OnPressed()
                PressedObject= hit.collider.gameObject; // 儲存被點擊的物件
                //}
            }
        }
        // 滑鼠左鍵放開時執行
        if (Input.GetMouseButtonUp(0))
        {
          
            // 將滑鼠位置轉成世界座標
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 射出 Raycast，偵測滑鼠放開時指向的物件
            //RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, clickableLayer);

            if (PressedObject != null)
            {
                //Debug.Log("滑鼠放開點到：" + PressedObject.name);

                // 假設有 Crop Tag
                //if (hit.collider.CompareTag("Clickable"))
                //{
                PressedObject.GetComponent<Draggable>()?.OnReleased();
                PressedObject = null;
                //}
            }
        }


    }


    void GenerateGrid()
    {
        Lands= save.Lands;
        int openLands = Lands;

        GridPrefabs = new GameObject[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {

                Vector3 pos = new Vector3(((x - y)-(x/4)*4) * 0.75f, ((x + y)+(x/4*12)) * 0.5f, 1);// * cellSpacing;

                //Vector2 pos = new Vector2(x-originPosition, y-originPosition) * cellSpacing;
                GameObject cell = Instantiate(cellPrefab, pos, Quaternion.identity, this.transform);
                cell.transform.rotation = Quaternion.Euler(0, 0, 0);
                GridCell gridCell = cell.GetComponent<GridCell>();
                gridCell.SetCoordinates(width, height,x, y);
                if (openLands > 0)
                {
                    gridCell.isOpen = true;
                    cell.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Source/Rectangle");
                    cell.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f); // 白色 + 半透明

                    openLands--;
                }

                ButtonOnClick Script = Buttom.GetComponent<ButtonOnClick>();
                if (Script.GridPrefabs == null)
                {
                    Script.GridPrefabs = new List<GameObject>();
                }
                //Debug.Log($"{x * (height - 1) + y}");
                Script.GridPrefabs.Add(cell);
                //GridPrefabs.Add(cell);
                GridPrefabs[x, y] = cell;

               
            }
        }

        Destroy(cellPrefab);

        //設定開地按鈕
        if (Lands < width * height)
        {
            OpenLandButtom.transform.position = GridPrefabs[Lands / 10,Lands % 10].transform.position;
        }
        
    }

    //獲取鄰近一樣的作物格子
    public Queue<GameObject> SearchSameCrop(Queue<GameObject> CellsQueue, int x, int y)//, HashSet<(int, int)> visited)
    {
        
        CellsQueue.Enqueue(GridPrefabs[x, y]);
        if (IsValid(x - 1, y)&&(x/4)==((x-1)/4))
            if (FindQueue(CellsQueue, GridPrefabs[x-1, y]) == false)
                    if ( GridPrefabs[x , y].GetComponent<GridCell>().status == GridPrefabs[x - 1, y].GetComponent<GridCell>().status && GridPrefabs[x , y].GetComponent<GridCell>().level == GridPrefabs[x - 1, y].GetComponent<GridCell>().level)
                CellsQueue=SearchSameCrop(CellsQueue,x - 1, y);
        if (IsValid(x + 1, y) && ((x+1)/4) == (x/4))
            if (FindQueue(CellsQueue, GridPrefabs[x + 1, y]) == false)
                if ( GridPrefabs[x , y].GetComponent<GridCell>().status == GridPrefabs[x + 1, y].GetComponent<GridCell>().status && GridPrefabs[x , y].GetComponent<GridCell>().level == GridPrefabs[x + 1, y].GetComponent<GridCell>().level)
                    CellsQueue=SearchSameCrop(CellsQueue,x + 1, y);
        if (IsValid(x , y - 1))
            if (FindQueue(CellsQueue, GridPrefabs[x , y - 1]) == false)
                if (IsValid(x ,y-1) && GridPrefabs[x , y].GetComponent<GridCell>().status == GridPrefabs[x, y - 1].GetComponent<GridCell>().status && GridPrefabs[x, y ].GetComponent<GridCell>().level == GridPrefabs[x, y - 1].GetComponent<GridCell>().level)
                        CellsQueue =SearchSameCrop(CellsQueue, x, y - 1);
        if (IsValid(x , y+1))
            if (FindQueue(CellsQueue, GridPrefabs[x , y + 1]) == false)
                if (IsValid(x , y+1) && GridPrefabs[x , y].GetComponent<GridCell>().status == GridPrefabs[x, y + 1].GetComponent<GridCell>().status && GridPrefabs[x, y ].GetComponent<GridCell>().level == GridPrefabs[x, y + 1].GetComponent<GridCell>().level)
                        CellsQueue = SearchSameCrop(CellsQueue, x, y + 1);
                
              

        return CellsQueue; 
    }
    private bool IsValid(int w,int h)
    {
        if (w >= 0 && h >= 0 && w < width && h < height)
               return true;
        return false;
    }
    private Queue<GameObject> AddQueue(Queue<GameObject> CellsQueue, Queue<GameObject> NewQueue)
    {
        foreach (var item in NewQueue)
        {
            CellsQueue.Enqueue(item);
        }
        NewQueue.Clear();
        return CellsQueue;
    }

    private bool FindQueue(Queue<GameObject> myQueue, GameObject find)
    {
        GameObject found = null;

        foreach (var item in myQueue)
        {
            if (item == find)
            {
                found = item;
                break;
            }
        }

        if (found != null)
        {
            return true; // 找到就返回 true
        }
        else
        {
            return false; // 沒找到就返回 false
        }
    }

    public void ChargeBank(int amount)
    {
        // 假設有一個 Bank 類別來處理金錢
      
        if (save != null)
        {
            save.AddMoney(amount);
        }
    }
    public void ChargeCropExp(int facility, int CropIndex, int CropLevel)
    {
        // 假設有一個 Bank 類別來處理金錢
        if (save != null)
        {
            save.AddExp(facility, CropIndex, CropLevel);
        }
    }

    public bool OpenGridCell()
    {
        if (Lands < width * height)
        {
            GameObject cell = GridPrefabs[Lands / 10, Lands % 10];
            cell.GetComponent<GridCell>().isOpen = true;//啟用地
            cell.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Source/Rectangle");
            cell.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f); // 白色 + 半透明
            Lands++;
            save.Lands = Lands;
            if (Lands < width * height)
            {
                //OpenLandButtom.GetComponentInChildren<TextMeshProUGUI>().text=new string($"開地({Lands*10} Coins)");
                OpenLandText.text = new string($"開地\n{Lands*5} Coins");
                OpenLandButtom.transform.position = GridPrefabs[Lands / 10, Lands % 10].transform.position;
               

            }
            else if (Lands == width * height)
                OpenLandButtom.transform.position = new Vector3(0f,0f,-10f);
            return true;
        }
        return false;
        
    }
}
