using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��l�}��
public class GridCell : MonoBehaviour
{

    public int x;
    public int y;
    public int status=0; //[0:empty, 1:Box, 2:...]
    private SpriteRenderer sr;
    public GameObject Crop; // �ΨӦs���l�W���@������
    // Start is called before the first frame update
    void Start()
    {

    }


    public void SetCoordinates(int x, int y)
    {
        this.x = x;
        this.y = y;
        //gameObject.name = $"Cell ({x},{y})"; // ��W�٤�K�����d��
    }


}
