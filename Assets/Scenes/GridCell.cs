using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//格子腳本
public class GridCell : MonoBehaviour
{

    public int x;
    public int y;
    public int status=0; //[0:empty, 1:Box, 2:...]
    private SpriteRenderer sr;
    public GameObject Crop; // 用來存放格子上的作物物件
    // Start is called before the first frame update
    void Start()
    {

    }


    public void SetCoordinates(int x, int y)
    {
        this.x = x;
        this.y = y;
        //gameObject.name = $"Cell ({x},{y})"; // 改名稱方便場景查看
    }


}
