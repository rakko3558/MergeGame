
using UnityEngine;

public class WorldButton : MonoBehaviour
{
    private SpriteRenderer sr;
    public Color normalColor = Color.white;
    public Color hoverColor = Color.yellow;
    public Storage save;

    public GameObject HintUI;
    public int BtnEvent=0;//0 買地 , 1 買建設
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = normalColor;
    }

    void OnMouseEnter()
    {
        //HintUI.SetActive(true);
        sr.color = hoverColor;  // 滑鼠移入改色
    }

    void OnMouseExit()
    {
        //HintUI.SetActive(false); // 滑鼠移出隱藏提示
        sr.color = normalColor; // 滑鼠移出恢復
    }

    void OnMouseDown()
    {
        if(BtnEvent==0)
            save.buyLand();
        if(BtnEvent==1)
            save.constructBuilding();
    }
}


