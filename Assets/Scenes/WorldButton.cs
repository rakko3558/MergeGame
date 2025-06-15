
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
        sr.color = hoverColor;  // ·Æ¹«²¾¤J§ï¦â
    }

    void OnMouseExit()
    {
        sr.color = normalColor; // ·Æ¹«²¾¥X«ì´_
    }

    void OnMouseDown()
    {
        save.buyLand();
    }
}


