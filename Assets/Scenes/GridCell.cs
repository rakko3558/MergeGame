using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��l�}��
public class GridCell : MonoBehaviour
{

    public int x;
    public int y;
    public int status=-1; //[0:empty, 1:Box, 2:...]
    public bool isOpen = false; //����}�L���g�a
    public int level = 0;
    private SpriteRenderer sr;
    public GameObject Crop; // �ΨӦs���l�W���@������
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
        sr.sortingOrder = width * height - (x* height + y) ; // �]�w��V���ǡA�T�O��l�b���T���h�ŤW
        //gameObject.name = $"Cell ({x},{y})"; // ��W�٤�K�����d��
    }

    /*
    public int GridCropMergeSearch()
    {
        GridCropMergeSearch()
        return 0;
    }*/

}
