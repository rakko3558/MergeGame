using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowExhibit : MonoBehaviour
{
    public TextMeshProUGUI describe;
    public TextMeshProUGUI coin;
    public TextMeshProUGUI exp;
    public Image roleImage;

    public GameObject Bonus;
    public TextMeshProUGUI BonusMoney;
    public TextMeshProUGUI BonusExp;


    public float fadeDuration = 5.0f;
    private CanvasGroup canvasGroup;
    private float timer = 5.0f;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
    }

    void Update()
    {
        if (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            if (timer < 0.5)
            {
                
                canvasGroup.alpha = Mathf.Clamp01(timer*2);
                return;
            }
            else if (timer < 4.5)
            {
                return;
            }
            else 
            {
                canvasGroup.alpha = Mathf.Clamp01((5-timer)*2);
                return;
            }
        }
    }

    public void showEventExhibit(string message, string imageName,int Exp, int Coin)
    {
        describe.text = message;
        coin.text= "+"+Coin.ToString();
        exp.text = "+" + Exp.ToString();
        roleImage.sprite = Resources.Load<Sprite>("Source/" + imageName);
        Bonus.SetActive(false);
        timer = 0f; // Reset timer for fade-in effect
    }
    public void showBonus(int money, int exp)
    {
        Bonus.SetActive(true);
        BonusMoney.text = "+" + money.ToString();
        BonusExp.text = "+" + exp.ToString();
    }

}
