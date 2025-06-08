using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//�@��
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
    public int CropValue = 1;//�w�] 1�� 1����
    public GameObject GridsManager; // �o�O�Ψ���ܧ@���Ϥ��� UI ����
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
        OnThisGrid.GetComponent<GridCell>().status = CropIndex; // �]�w��l���A�����@��
        OnThisGrid.GetComponent<GridCell>().level = CropLevel;
        Sprite firstSprite = Resources.Load<Sprite>("Source/"+ cropNames[CropIndex,CropLevel]);
        SpriteRenderer U_Sprite = GetComponentInChildren<SpriteRenderer>();
        U_Sprite.sprite = firstSprite;

        if (CropLevel == 3 && CropIndex!=0)
        {
            HaveCoin=true;
            U_Sprite.color = new Color(1f, 0.9f, 0.5f, 1f);
            Debug.Log($"�ĥ|��{HaveCoin}");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToOtherGrid(int x, int y)
    {
        GridmManager Grids = GridsManager.GetComponent<GridmManager>();
        // ���ʨ�s����m
        //int max=Grids.width>Grids.height? Grids.width : Grids.height;
        //�������Ҧ��Ů�l
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
            Debug.Log("�S���Ů�l�i�H��m���~�F�I");//���i�H�o��
        }*/
        //GridsManager.getComponent<GridmManager>().GridPrefabs;

    }

    /*
    public void SpawnRandomCrop()
    {
        // ��l�Ϥ�
        string firstCrop = GetRandomCrop();
        Sprite firstSprite = Resources.Load<Sprite>("Crops/" + firstCrop);
        if (firstSprite == null)
        {
            Debug.LogError("�䤣��Ϥ��G" + firstCrop);
            return;
        }

        // �إ� UI �Ϥ�
        GameObject cropUI = Instantiate(Resources.Load<GameObject>("Prefabs/CropUI"), spawnParent);
        SpriteRender image = image.GetComponent<SpriteRender>();
        image.sprite = firstSprite;

        // �Ұʨ�{�G���𴫹�
        StartCoroutine(ChangeImageAfterDelay(image));
    }
    
    private IEnumerator ChangeImageAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);

        // ���@�i���P���Ϥ�
        string newCrop;
        Sprite newSprite;
        do
        {
            newCrop = GetRandomCrop();
            newSprite = Resources.Load<Sprite>("Crops/" + newCrop);
        }
        while (newSprite == null || newSprite == image.sprite); // �קK���Ӥ@��

        image.sprite = newSprite;
    }
    */
    public void GetRandomCrop()
    {
        CropIndex = Random.Range(1, PlayerLevel+1);// cropNames.GetLength(0));
    }
}
