using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//格子腳本
public class GridCell : MonoBehaviour
{

    public int x;
    public int y;
    public int status=-1; //[0:empty, 1:Box, 2:...]
    public bool isOpen = false; //解鎖開過的土地
    public int level = 0;
    private SpriteRenderer sr;
    public GameObject Crop; // 用來存放格子上的作物物件
    // Start is called before the first frame update
    void Start()
    {
       
       // sr.sortingLayerName = "Foreground";
    }


    public void SetCoordinates(int width , int height, int x, int y)
    {
        sr = GetComponent<SpriteRenderer>();
        this.x = x;
        this.y = y;
        sr.sortingOrder = width * height - (x* height + y) ; // 設定渲染順序，確保格子在正確的層級上
        //gameObject.name = $"Cell ({x},{y})"; // 改名稱方便場景查看
    }

    /*
    public int GridCropMergeSearch()
    {
        GridCropMergeSearch()
        return 0;
    }*/

}
