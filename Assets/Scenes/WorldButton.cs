
using UnityEngine;

public class WorldButton : MonoBehaviour
{
    private SpriteRenderer sr;
    public Color normalColor = Color.white;
    public Color hoverColor = Color.yellow;
    public Storage save;

    public GameObject HintUI;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = normalColor;
    }

    void OnMouseEnter()
    {
        HintUI.SetActive(true);
        sr.color = hoverColor;  // �ƹ����J���
    }

    void OnMouseExit()
    {
        HintUI.SetActive(false); // �ƹ����X���ô���
        sr.color = normalColor; // �ƹ����X��_
    }

    void OnMouseDown()
    {
        save.buyLand();
    }
}


