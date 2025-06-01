using UnityEngine;
using System.Collections.Generic;
//�첾�@��
public class Draggable : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    //private Collider2D lastTriggerGrid; // �O���̫ᱵĲ����l
    public List<Collider2D> collidingGrids= new List<Collider2D>(); // �ΨӦs��Ҧ��I��������

    public GameObject GridManager; // �ΨӦs���l�޲z��
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
        // �I�U�h�ɶ}�l�즲
        isDragging = true;
        //Debug.Log($"clicked");
        // �p���I�U�h�� offset�]�ƹ��M���󤧶����t�Z�^
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = transform.position.z; // ���� Z �b����
        offset = transform.position - mouseWorldPos;



        //lastTriggerGrid = GetComponent<Farm>().OnThisGrid.GetComponent<Collider2D>(); // ��s�̫ᱵĲ����l
        /*if (GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>() == null) 
        {
            Debug.Log($"�ӧ@���S���b��l�W");
        }*/
        if (GetComponent<Farm>().OnThisGrid != null)
        {
            GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().status = 0;
            GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>()    .level = 0;
            GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().Crop = null;// �M�����l�ޥ�
            GetComponent<Farm>().OnThisGrid = null; //�M�����l�W���@���ޥ� 
        }
    }

    public void OnReleased()
    {
        // ��}�ƹ�����즲

        Collider2D NearestTriggerGrid = GetNearestGrid();//�����e�I�����Z���̪񪺮�l
        if (NearestTriggerGrid != null)
        {
            if (NearestTriggerGrid.GetComponent<GridCell>().Crop != null && NearestTriggerGrid.GetComponent<GridCell>().status == GetComponent<Farm>().CropIndex && NearestTriggerGrid.GetComponent<GridCell>().level == GetComponent<Farm>().CropLevel && NearestTriggerGrid.GetComponent<GridCell>().level<3)//�P�˪��~�i��X��
            {
                Queue<GameObject> SameCropCells = new Queue<GameObject>()  ;
                SameCropCells = GridManager.GetComponent<GridmManager>().SearchSameCrop(SameCropCells, NearestTriggerGrid.GetComponent<GridCell>().x, NearestTriggerGrid.GetComponent<GridCell>().y);//, new HashSet<(int, int)>()); // �j���P�˧@��

                if (SameCropCells.Count >= 2)
                {
                    int LevelingAmount = (SameCropCells.Count+1) / 5 * 2 + ((SameCropCells.Count+1)%5)/ 3;
                    int LeaveAmount = (SameCropCells.Count+1)%5%3;
                    Debug.Log($"�ɯżƶq:{LevelingAmount}, �Ѿl�ƶq{LeaveAmount}");
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

                    GetComponent<Farm>().CropLevel++; // �X���ᵥ�Ŵ���
                    GetComponent<Farm>().ChangeSprite(); // ��s�Ϥ�

                    for (int i = 0; i < LevelingAmount-1; i++)
                    {
                        Collider2D NearestEmptyGrid = NocolliderGetNearestGrid();

                        SpawnSpecifyCrop(NearestEmptyGrid.GetComponent<GridCell>().x, NearestEmptyGrid.GetComponent<GridCell>().y, GetComponent<Farm>().CropIndex,GetComponent<Farm>().CropLevel);
                    }
                    
                    //GridManager.GetComponent<GridmManager>().CropAmount = GridManager.GetComponent<GridmManager>().CropAmount - (SameCropCells.Count+1) + LevelingAmount+ LeaveAmount;//����@���ƶq
                }
                else {
                    GridCell GridCellTmp = NearestTriggerGrid.GetComponent<GridCell>();
                    GridCellTmp.Crop.GetComponent<Farm>().MoveToOtherGrid(GridCellTmp.x, GridCellTmp.y);
                    MoveToGrid(NearestTriggerGrid);
                    NearestTriggerGrid.GetComponent<GridCell>().Crop = gameObject;
                }

                //NearestTriggerGrid.GetComponent<GridCell>().GridCropMergeSearch(0); // �X����l�W���@��
                //NearestTriggerGrid.GetComponent<GridCell>().GridCropMergeSearch();
                //Debug.Log($"{NearestTriggerGrid.GetComponent<GridCell>().status},{ NearestTriggerGrid.GetComponent<GridCell>().level}");
            }
            else
            {
                if (NearestTriggerGrid.GetComponent<GridCell>().Crop != null)
                {


                    //Debug.Log($"Print:{lastTriggerGrid.GetComponent<GridCell>().Crop.GetComponent<Farm>().CropIndex}");
                    GridCell GridCellTmp = NearestTriggerGrid.GetComponent<GridCell>();
                    //Debug.Log($"GridCell �y�Ь��G({GridCellTmp.x}, {GridCellTmp.y})");
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
        else if (NearestTriggerGrid == null)// �S���I���� �������l���Z���̪񪺪Ů�l
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

    private void DestroyCrop(GameObject Grid)//�R���@��
    {
        Grid.GetComponent<GridCell>().status = 0;
        Grid.GetComponent<GridCell>().level = 0;
        Destroy(Grid.GetComponent<GridCell>().Crop); // �R���@������
        Grid.GetComponent<GridCell>().Crop = null;
    }
    // �����e�I������l���Z���̪񪺮�l
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

    // �S���I���� �������l���Z���̪񪺪Ů�l
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
                    Debug.Log($"�S���Ŧ�");
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
        // ���ʨ��l����
        //GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().Crop = null;
        transform.position = grid.bounds.center;
        GetComponent<Farm>().OnThisGrid = grid.gameObject;
        GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().status =  GetComponent<Farm>().CropIndex;
        GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().level = GetComponent<Farm>().CropLevel;
    }
    //�I��


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
            mouseWorldPos.z = transform.position.z; // ������� Z
            transform.position = mouseWorldPos + offset;
            }
    }
}
