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
        GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().Crop = null;// �M�����l�ޥ�
        GetComponent<Farm>().OnThisGrid= null; //�M�����l�W���@���ޥ� 
    }

    public void OnReleased()
    {
        // ��}�ƹ�����즲

        Collider2D NearestTriggerGrid = GetNearestGrid();//�����e�I�����Z���̪񪺮�l
        if (NearestTriggerGrid != null)
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
        else if (NearestTriggerGrid == null)// �S���I���� �������l���Z���̪񪺪Ů�l
        {
            Collider2D NearestEmptyGrid = NocolliderGetNearestGrid();

            MoveToGrid(NearestEmptyGrid);
            NearestEmptyGrid.GetComponent<GridCell>().Crop = gameObject;
            
        }
        //Debug.Log($"{lastTriggerGrid.GetComponent<GridCell>().Crop.GetComponent<Farm>().CropIndex}");
        isDragging = false;
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
    public void MoveToGrid(Collider2D grid)
    {
        // ���ʨ��l����
        //GetComponent<Farm>().OnThisGrid.GetComponent<GridCell>().Crop = null;
        transform.position = grid.bounds.center;
        GetComponent<Farm>().OnThisGrid = grid.gameObject;
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
