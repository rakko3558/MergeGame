using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
//�첾�@��
public class Draggable : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    //private Collider2D lastTriggerGrid; // �O���̫ᱵĲ����l
    public List<Collider2D> collidingGrids= new List<Collider2D>(); // �ΨӦs��Ҧ��I��������
    private int TouchIndex = -1; // �ΨӧP�_�O�_�I����Ȧ�

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
   
        // ��}�ƹ�����즲
        if ( GetComponent<Farm>().CropIndex != 0 && GetComponent<Farm>().CropLevel == 3 && GetComponent<Farm>().HaveCoin == true)
            {

            Storage save = FindObjectOfType<Storage>();
            int CropValue= save.cropLevel[GetComponent<Farm>().CropIndex];
            while (CropValue > 0)
            {
                Collider2D NearestEmptyGrid = NocolliderGetNearestGrid();
                if (NearestEmptyGrid != null)
                {
                    CropValue--;
                    //Collider2D NearestEmptyGrid = NocolliderGetNearestGrid();

                    SpawnSpecifyCrop(NearestEmptyGrid.GetComponent<GridCell>().x, NearestEmptyGrid.GetComponent<GridCell>().y, 0, 0);

                    //GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                }
                else {
                    break;
                }
                  
            }
            if (CropValue <= 0)
            {
                GetComponent<Farm>().HaveCoin = false;

                GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            }
        }
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
            GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().status = -1;
            GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>()    .level = 0;
            GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().Crop = null;// �M�����l�ޥ�
            GetComponent<Farm>().OnThisGrid = null; //�M�����l�W���@���ޥ� 
        }
    }

    public void OnReleased()
    {
        if (TouchIndex!=-1)//�I������
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
        }
        
        Collider2D NearestTriggerGrid = GetNearestGrid();//�����e�I�����Z���̪񪺮�l(����)


        if (NearestTriggerGrid != null)
        {
            Debug.Log($"NearestTriggerGrid Not null");
            if (NearestTriggerGrid.GetComponent<GridCell>().Crop != null && NearestTriggerGrid.GetComponent<GridCell>().status == GetComponent<Farm>().CropIndex && NearestTriggerGrid.GetComponent<GridCell>().level == GetComponent<Farm>().CropLevel &&( NearestTriggerGrid.GetComponent<GridCell>().level<3 || (NearestTriggerGrid.GetComponent<GridCell>().status == 0 && NearestTriggerGrid.GetComponent<GridCell>().level < 5)))//�P�˪��~�i��X��
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
            Destroy(gameObject); // �R���@��
            return; //�p�G�O��l�@���A�����������
        }
        else if (Crop.CropIndex != 0)
        {
            GridManager.GetComponent<GridmManager>().ChargeCropExp(facility,Crop.CropIndex, Crop.CropLevel);

            GridManager.GetComponent<GridmManager>().CropAmount--;
            Destroy(gameObject); // �R���@��
            return; //�p�G�O��l�@���A�����������
        }
    }
    public void SpawnSpecifyCrop(int x, int y, int status, int level)
    {

        GameObject spawned = Instantiate(BoxPrefabs, GridPrefabs[x, y].transform.position, Quaternion.identity);
        //spawnedB.transform.rotation = Quaternion.Euler(0, 0, 0);
        spawned.GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
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
        Grid.GetComponent<GridCell>().status = -1;
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
                    //Debug.Log($"�S���Ŧ�");
                }

            }
        }
        return nearestGrid;
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
            collidingGrids.Add(other);
        }
           
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

    }


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
