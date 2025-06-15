using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Storage : MonoBehaviour
{
    int money = 1200;// 玩家金錢
    public TextMeshProUGUI txt_money;
    public TextMeshProUGUI[] txt_level= new TextMeshProUGUI[11];
    public GameObject panel_TextArea;
    public TextMeshProUGUI TextPerfab;

    public TextMeshProUGUI UnlockCharacter;

    public ButtonOnClick ButtonOnClickScript;
    public int playerLevel = 1;
    public  int MaxLevel = 50;

    public int[] cropExp = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public int[] cropLevel = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }; // 作物等級，預設為 1 等級
    public GridmManager GridManager;
    public int Lands = 20;


    public Facilitys[] facilityArray;
    //private int facility = 0; // 0:銀行，1:結婚，2:出攤，3:演唱會

    private string[] cropName = new string[] { "錢錢", "紙屑", "羊肉爐", "阿鵝", "Taki", "+0", "SC", "Riku", "皮卡丘", "波吉", "公主" };
    public GameObject[] CharaterIndex;
    // Start is called before the first frame update
    void Start()
    {
        checkFacility();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void showTextMessage(string message)
    {
        GameObject clonedTextGO = Instantiate(TextPerfab.gameObject);
        clonedTextGO.GetComponent<TextMeshProUGUI>().text = message;

        clonedTextGO.transform.SetParent(panel_TextArea.transform, false);

        // 啟用 GameObject（如果 template 是 hidden 的話）
        
        //GameObject clonedTextGO = Instantiate(TextPerfab.gameObject);
        //clonedTextGO.GetComponent<TextMeshProUGUI>().text = message;
        //clonedTextGO.transform.SetParent(panel_TextArea.transform, false); // 設定父物件
        clonedTextGO.SetActive(true); // 啟用 GameObject（如果 template 是 hidden 的話）
    }

    public void AddMoney(int amount)
    {
        money= money+amount;
        txt_money.text = money.ToString();
        string message = $"{amount}元存入了匯豐銀行";
        showTextMessage(message);

        //GameObject clonedTextGO = Instantiate(TextPerfab.gameObject);
        //clonedTextGO.GetComponent<TextMeshProUGUI>().text = $"{amount}元存入了匯豐銀行";
        Debug.Log($"玩家金錢增加：{amount}，目前金錢：{money}");
    }

    public void AddExp(int facility ,int CropIndex, int CropExp)//CropExp是作物進化階段
    {

        //計算經驗值
        if (cropLevel[CropIndex] == MaxLevel)
        {
            Debug.Log($"已滿等！目前等級：{cropLevel[CropIndex]}");
            return;
        }
        
        GameObject clonedTextGO = Instantiate(TextPerfab.gameObject);
        //顯示文字活動
        int encreaseExp = 0;
        switch (facility)
        {
            case 0: // 銀行
                int[] CropExpLevel = new int[] { 1, 3, 10, 50 };
                encreaseExp = CropExpLevel[CropExp];
                cropExp[CropIndex] += encreaseExp;
                clonedTextGO.GetComponent<TextMeshProUGUI>().text = $"將" +
                    $"{cropName[CropIndex]}存入了匯豐銀行！(EXP+{encreaseExp})";
                break;
            case 1: // 結婚
                encreaseExp = facilityArray[1].expAmount;
                cropExp[CropIndex] += encreaseExp;
                string[] marriageNames = { "紙屑", "羊肉爐", "阿鵝", "Taki", "+0", "SC", "Riku", "波吉", "公主" };
                clonedTextGO.GetComponent<TextMeshProUGUI>().text =
                            $"恭喜{cropName[CropIndex]}跟{marriageNames[Random.Range(0, marriageNames.Length)]}結婚了！(EXP+{encreaseExp})";
                break;
            case 2: // 出攤
                encreaseExp = facilityArray[2].expAmount;
                cropExp[CropIndex] += encreaseExp;
                string CPName1;
                string CPName2;
                int a = Random.Range(0, 1);
                if (a==0)
                {
                    string[] CPNames = { "樂奈", "愛音", "喵夢", "睦", "爽世", "祥子", "初華", "海鈴", "立希", "燈" };
                    CPName1 = CPNames[Random.Range(0, CPNames.Length)];
                    CPName2 = CPNames[Random.Range(0, CPNames.Length)];
                    while (CPName2 == CPName1)
                    {
                        CPName2 = CPNames[Random.Range(0, CPNames.Length)];
                    }
                }

                else
                {
                    string[] CPNames = {  "桃香", "昴", "仁菜", "Rupa", "智" };
                    CPName1 = CPNames[Random.Range(0, CPNames.Length)];
                    CPName2 = CPNames[Random.Range(0, CPNames.Length)];
                    while (CPName2 == CPName1)
                    {
                        CPName2 = CPNames[Random.Range(0, CPNames.Length)];
                    }
                }
                clonedTextGO.GetComponent<TextMeshProUGUI>().text =
                            $"{cropName[CropIndex]}出攤賣本({CPName1} x {CPName2})！(EXP+{encreaseExp})";
                break;
            case 3: // 演唱會
                encreaseExp = facilityArray[3].expAmount;
                cropExp[CropIndex] += encreaseExp;
                string[] bandname = { "約束約團", "Ave Mujica", "MyGO!!!!!", "有刺無刺" };//, "Poppin'Party", "Roselia", "Afterglow", "Pastel*Palettes", "Hello, Happy World!","RAISE A SUILEN", "Morfonica", "夢限大MewType",""};



                clonedTextGO.GetComponent<TextMeshProUGUI>().text = $"{cropName[CropIndex]}去看了{bandname[Random.Range(0, bandname.Length)]}的演唱會！(EXP+{encreaseExp})";
                break;
            default:
                Debug.LogWarning("未知的設施類型！");
                return;
        }
        clonedTextGO.transform.SetParent(panel_TextArea.transform, false); 

        // 啟用 GameObject（如果 template 是 hidden 的話）
        clonedTextGO.SetActive(true);
        //確認等級
        CheckLevelUp(CropIndex);

        Debug.Log($"作物{CropIndex}經驗值增加：{CropExp * 50}，目前經驗值：{cropExp[CropIndex]}，等級：{cropLevel[CropIndex]}");
    }
    // 判斷是否升級
    private void CheckLevelUp(int CropIndex)
    {
        
        while (cropExp[CropIndex] >= ExpToNextLevel(cropLevel[CropIndex]))
        {
           
            cropExp[CropIndex] -= ExpToNextLevel(cropLevel[CropIndex]);
            cropLevel[CropIndex]++;
            txt_level[CropIndex].text = cropLevel[CropIndex].ToString();
            Debug.Log($"升級！目前等級：{cropLevel[CropIndex]}");

            // TODO: 可加技能點數、獎勵、解鎖物品等
        }
        
    }
    private int ExpToNextLevel(int level)
    {
        return 100 + (level - 1) * 50; // 等級1需100、2需150、3需200...
    }

    public void buyLand()
    {
        int LandPrice = Lands * 5; // 每土的價格
        if (money < LandPrice)
        {

            showTextMessage($"存款不足");
            return;
        }
        if (GridManager.OpenGridCell())
        {
            money = money - LandPrice;
            txt_money.text = money.ToString();
            string message = $"獲得新土地(-{LandPrice} Coins)";
            showTextMessage(message);
        }

    }

    public void buyChatacter()
    {
        int price = playerLevel*1000; // 每個角色的價格
        if (money < price)
        {
           
            showTextMessage($"存款不足");
            return;
        }
        if (SetPlayerLevel(1))
        {
            
        money = money - price;// 暫定每個角色100元
        txt_money.text = money.ToString();
        string message = $"獲得新角色(-{price} Coins)";
        showTextMessage(message);

            UnlockCharacter.text = $"解鎖角色\n({playerLevel * 1000} Coins)";
            

            //GameObject clonedTextGO = Instantiate(TextPerfab.gameObject);
            //clonedTextGO.GetComponent<TextMeshProUGUI>().text = $"{amount}元存入了匯豐銀行";
         checkFacility();
        }
    }

    public bool SetPlayerLevel(int levelnum)
    {
        if (playerLevel < cropName.Length)
        {
            playerLevel++; 
            ButtonOnClickScript.playerLevel = playerLevel;
            CharaterIndex[playerLevel-1].SetActive(true);
            if (playerLevel == cropName.Length-1)
            {
                CharaterIndex[playerLevel].SetActive(false);//都解玩完 隱藏解鎖按鈕
            }
            return true;
        }
        return false;
       
    }

    public void checkFacility()
    {
        for (int i = 0; i < facilityArray.Length; i++)
        {
            if ((playerLevel / 3)>= i)
            {
                if (facilityArray[i].isOpen == false)
                {
                    facilityArray[i].Open();
                }
            }
            Debug.Log($"{playerLevel / 2},{i},{facilityArray[i].isOpen}");
        }
    }
        
}
