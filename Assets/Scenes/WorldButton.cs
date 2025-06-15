
using UnityEngine;

public class WorldButton : MonoBehaviour
{
    private SpriteRenderer sr;
    public Color normalColor = Color.white;
    public Color hoverColor = Color.yellow;
    public Storage save;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = normalColor;
    }

    void OnMouseEnter()
    {
        sr.color = hoverColor;  // �ƹ����J���
    }

    void OnMouseExit()
    {
        sr.color = normalColor; // �ƹ����X��_
    }

    void OnMouseDown()
    {
        save.buyLand();
    }
}


