using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//�@��
public class Farm : MonoBehaviour
{

    private string[] cropNames = { "ts0","ylr0", "uu0", "tk0", "t00","sc0","rik0","pkc0","pj0","gz0"};

    //public GameObject image;
    public int CropIndex;
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
        CropIndex = GetRandomCrop();
        Sprite firstSprite = Resources.Load<Sprite>("Source/"+ cropNames[CropIndex]);
        SpriteRenderer U_Sprite = GetComponentInChildren<SpriteRenderer>();
        U_Sprite.sprite = firstSprite;
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
    private int GetRandomCrop()
    {
        return Random.Range(0, cropNames.Length);
    }
}
