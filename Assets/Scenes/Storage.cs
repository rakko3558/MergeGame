using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System; //
using System.Linq; // 一定要加在檔案最上方！
public class Storage : MonoBehaviour
{
    public string playerID = "0000";// 玩家金錢
    //public int money = 1200000;// 玩家金錢
    public TextMeshProUGUI txt_money;
    public TextMeshProUGUI[] txt_level = new TextMeshProUGUI[11];
    public GameObject panel_TextArea;
    public TextMeshProUGUI TextPerfab;
    public FirebaseTest Database;
    public GameObject panel_TextAreaMoney;
    public TextMeshProUGUI TextPerfabMoney;

    public TextMeshProUGUI UnlockCharacterHint;
    public TextMeshProUGUI UnlockCharacter;

    public ButtonOnClick ButtonOnClickScript;

    //任務相關
    public TextMeshProUGUI questDepiction;
    public TextMeshProUGUI questReward;
    /*
    public int questIndex = -1; // 目前任務索引，-1 代表沒有任務 0:銀行，1:結婚，2:出攤，3:演唱會
    public int questCharacter = -1; // 目前任務索引，-1 代表沒有任務 0:錢，1:紙屑，2:....
    public int questExp = 0; // 任務經驗值
    public int questMoney = 0; // 任務金錢獎勵
    */
    //public int playerLevel = 1;//開啟的角色數量
    //public int Boxs = 100;// 剩餘箱子數量
    public ShowExhibit exhibit; // 顯示展覽的腳本

    public int MaxLevel = 50;

    //public int[] cropExp   = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    //public int[] cropLevel = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }; // 作物等級，預設為 1 等級
    public GridmManager GridManager;
    //public int Lands = 20;

    [System.Serializable]
    public class PlayerData
    {
        public int money;// 玩家金錢
        public int playerLevel;
        public int Lands;
        public int Boxs;// 剩餘箱子數量

        public int[] cropExp;
        public int[] cropLevel; // 作物等級，預設為 1 等級

        public string date;// 剩餘箱子數量

        public int[] GridStatus; // 用來存放格子狀態，0:空格，1:有作物，2:有箱子
        public int[] GridLevel; // 用來存放格子等級，0:無等級，1:有等級1的作物，2:有等級2的作物，3:有等級3的作物
        public int[] CropCoin;  // 用來存放格子作物的金錢值，0:無作物，1:有作物1的金錢值，2:有作物2的金錢值，3:有作物3的金錢值

        public int questFalicity;
        public int questCropIndex;
        public int questMoney;
        public int questExp;
        public int builds;

    }

    public PlayerData data = new PlayerData//是不是應該放在 createAccount 裡面？
    {
        money = 1200,// 玩家金錢
        playerLevel = 1,
        Lands = 20,
        Boxs = 300,// 剩餘箱子數量
        cropExp = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        cropLevel = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, // 作物等級，預設為 1 等級
        date = "0",
        GridStatus = Enumerable.Repeat(-1, 120).ToArray(),
        GridLevel =new int[120],
        CropCoin=new int[120],

        questFalicity=0,
        questCropIndex=1,
        questMoney=50,
        questExp=50,
        builds=1
    };

    public Facilitys[] facilityArray;
    //private int facility = 0; // 0:銀行，1:結婚，2:出攤，3:演唱會

    private string[] cropName = new string[] { "", "紙屑", "羊肉爐", "+0", "波吉", "公主" , "SC", "Riku", "Taki", "阿鵝", "皮卡丘"};
    private static string[,] cropNames = new string[,]
        {
           { "coin_0","coin_1","coin_2","coin_3","coin_4"},
           { "PaperW_00", "PaperW_01", "PaperW_02", "PaperW_03",""},
           { "Sheep_00", "Sheep_01", "Sheep_02", "Sheep_03",""},          
           { "Plus0_00", "Plus0_01", "Plus0_02", "Plus0_03",""},
           { "Pochi_00", "Pochi_01", "Pochi_02", "Pochi_03",""},
           { "Princess_00", "Princess_01", "Princess_02", "Princess_03",""},
           { "SC_00", "SC_01", "SC_02", "SC_03",""},
           { "rik0", "rik1", "rik2", "rik3",""},
           { "tk0", "tk1", "tk2", "tk3",""},
           { "uu0", "uu1", "uu2", "uu3",""},
           { "pkc0", "pkc1", "pkc2", "pkc3",""}
        };
    public GameObject[] CharaterIndex;
    // Start is called before the first frame update

    void Start()
    {

        //checkFacility();

        //RandomQuest(); // 初始化隨機任務 //或載入之前的任務
    }

    public void createAccount(string playerID)
    {
        data.date=System.DateTime.Now.ToString("yyyyMMdd");//須重置日期 否則會是PUBLIC設定的值
        string cropExpString = JsonUtility.ToJson(data.cropExp);
        string newPlayerData = JsonUtility.ToJson(data);
        Debug.Log(newPlayerData);
        Database.StartCoroutine(Database.WriteData(playerID, newPlayerData));

        LoginRefreshValue();
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

    private void showTextMessageMoney(int message)
    {
        GameObject clonedTextGO = Instantiate(TextPerfabMoney.gameObject);

        if (message >= 0)
        {
            clonedTextGO.GetComponent<TextMeshProUGUI>().text = "+"+message.ToString();
        }
        else if (message<0)
        {
            clonedTextGO.GetComponent<TextMeshProUGUI>().text = message.ToString();
        }
        clonedTextGO.transform.SetParent(panel_TextAreaMoney.transform, false);
        clonedTextGO.SetActive(true); // 啟用 GameObject（如果 template 是 hidden 的話）
    }


    public void AddMoney(int amount) //專for 銀行
    {
       
        string message = $"{amount}元存入了匯豐銀行";
        showTextMessage(message);
        //showTextMessageMoney(amount);
        AddMoneyCompute(amount);

        //GameObject clonedTextGO = Instantiate(TextPerfab.gameObject);
        //clonedTextGO.GetComponent<TextMeshProUGUI>().text = $"{amount}元存入了匯豐銀行";
        //Debug.Log($"玩家金錢增加：{amount}，目前金錢：{money}");
    }
    public void AddMoneyCompute(int amount)
    {
        data.money = data.money + amount;
        txt_money.text = data.money.ToString();

        showTextMessageMoney(amount);
        StartCoroutine(Database.UpdateData($"{playerID}/money", data.money.ToString()));
    }
    public void AddExp(int facility ,int CropIndex, int CropExp)//CropExp是作物進化階段 //名字取的爛 這專for設施
    {

        //計算經驗值
        if (data.cropLevel[CropIndex] == MaxLevel)
        {
            //Debug.Log($"已滿等！目前等級：{cropLevel[CropIndex]}");
            return;
        }
        
        //GameObject clonedTextGO = Instantiate(TextPerfab.gameObject);
        //顯示文字活動
        int encreaseExp = 0;
        string description;
        switch (facility)
        {
            case 0: // 銀行
                int[] CropExpLevel = new int[] { 1, 3, 10, 50 };
                encreaseExp = CropExpLevel[CropExp];
                AddExpCompute(CropIndex,encreaseExp); // 計算經驗值

                description = $"將{cropName[CropIndex]}存入了匯豐銀行！(EXP+{encreaseExp})";

                //clonedTextGO.GetComponent<TextMeshProUGUI>().text = description;


                description= $"將{cropName[CropIndex]}\n存入了匯豐銀行！";
                exhibit.showEventExhibit(description, cropNames[CropIndex, CropExp],encreaseExp, 0);
                break;
            case 1: // 結婚
                
                encreaseExp = facilityArray[1].expAmount;
                AddExpCompute(CropIndex, encreaseExp); // 計算經驗值
                //string[] marriageNames = { "紙屑", "羊肉爐", "阿鵝", "Taki", "+0", "SC", "Riku", "波吉", "公主" };
                //string marrayName;
                if (CropIndex == 1)
                {
                    //clonedTextGO.GetComponent<TextMeshProUGUI>().text = $"恭喜紙屑跟容老大結婚了！EXP +{encreaseExp}";
                    description = $"恭喜\n紙屑跟容老大結婚了！";
                }
                
                else
                {
                    int randomIndex = UnityEngine.Random.Range(2, data.playerLevel+1);
                    if (randomIndex == CropIndex)
                    {
                        if (data.playerLevel <= 3)//只有1跟23作物 就看老大跟紙屑結婚
                        {
                            //clonedTextGO.GetComponent<TextMeshProUGUI>().text = $"{cropName[CropIndex]}參加了紙屑跟容老大的婚禮！EXP +{encreaseExp}";
                            description = $"{cropName[CropIndex]}參加了\n紙屑跟容老大的婚禮！";
                        }
                        else//骰到自己  看別人跟別人結婚
                        {
                            int marrayIndex = UnityEngine.Random.Range(2, data.playerLevel+1);
                            int marrayIndex2 = UnityEngine.Random.Range(2, data.playerLevel+1);
                            while (marrayIndex == CropIndex || marrayIndex2 == CropIndex || marrayIndex == marrayIndex2)//不能跟自己結婚
                            {
                                marrayIndex = UnityEngine.Random.Range(2, data.playerLevel+1);
                                marrayIndex2 = UnityEngine.Random.Range(2, data.playerLevel+1);
                            }

                            
                            //clonedTextGO.GetComponent<TextMeshProUGUI>().text = $"{cropName[CropIndex]}參加了{cropName[marrayIndex]}跟{cropName[marrayIndex2]}的婚禮！EXP +{encreaseExp}";
                            description = $"{cropName[CropIndex]}參加了\n{cropName[marrayIndex]}跟{cropName[marrayIndex2]}的婚禮！";
                        }
                            
                    }
                    else//跟人結婚
                    {
                        //clonedTextGO.GetComponent<TextMeshProUGUI>().text = $"恭喜{cropName[CropIndex]}跟{cropName[randomIndex]}結婚了！EXP +{encreaseExp}";
                        description = $"恭喜\n{cropName[CropIndex]}跟{cropName[randomIndex]}結婚了！";
                    }
                }
                exhibit.showEventExhibit(description, cropNames[CropIndex, 3], encreaseExp, 0);
                break;

            case 2: // 出攤
                encreaseExp = facilityArray[2].expAmount;
                AddExpCompute(CropIndex, encreaseExp); // 計算經驗值
                string CPName1;
                string CPName2;
                int a = UnityEngine.Random.Range(0, 1);
                if (a==0)
                {
                    string[] CPNames = { "樂奈", "愛音", "喵夢", "睦", "爽世", "祥子", "初華", "海鈴", "立希", "燈" };
                    CPName1 = CPNames[UnityEngine.Random.Range(0, CPNames.Length)];
                    CPName2 = CPNames[UnityEngine.Random.Range(0, CPNames.Length)];
                    while (CPName2 == CPName1)
                    {
                        CPName2 = CPNames[UnityEngine.Random.Range(0, CPNames.Length)];
                    }
                }

                else
                {
                    string[] CPNames = {  "桃香", "昴", "仁菜", "Rupa", "智" };
                    CPName1 = CPNames[UnityEngine.Random.Range(0, CPNames.Length)];
                    CPName2 = CPNames[UnityEngine.Random.Range(0, CPNames.Length)];
                    while (CPName2 == CPName1)
                    {
                        CPName2 = CPNames[UnityEngine.Random.Range(0, CPNames.Length)];
                    }
                }
                //clonedTextGO.GetComponent<TextMeshProUGUI>().text =$"{cropName[CropIndex]}出攤賣本({CPName1} x {CPName2})！(EXP+{encreaseExp})";
                break;
            case 3: // 演唱會
                encreaseExp = facilityArray[3].expAmount;
                AddExpCompute(CropIndex, encreaseExp); // 計算經驗值
                string[] bandname = { "約束樂團", "Ave Mujica", "MyGO!!!!!", "有刺無刺" };//, "Poppin'Party", "Roselia", "Afterglow", "Pastel*Palettes", "Hello, Happy World!","RAISE A SUILEN", "Morfonica", "夢限大MewType",""};



                //clonedTextGO.GetComponent<TextMeshProUGUI>().text = $"{cropName[CropIndex]}去看了{bandname[UnityEngine.Random.Range(0, bandname.Length)]}的演唱會！(EXP+{encreaseExp})";
                break;
            default:
                //Debug.LogWarning("未知的設施類型！");
                return;
        }
        //clonedTextGO.transform.SetParent(panel_TextArea.transform, false); 

        // 啟用 GameObject（如果 template 是 hidden 的話）
        //clonedTextGO.SetActive(true);
        //確認等級
        CheckQuestComplete(facility, CropIndex); // 檢查任務是否完成
        CheckLevelUp(CropIndex);//不能擺進AddExpCompute裡面 因為會重複更新
        StartCoroutine(Database.UpdateData($"{playerID}/cropExp/{CropIndex}", data.cropExp[CropIndex].ToString()));
        StartCoroutine(Database.UpdateData($"{playerID}/cropLevel/{CropIndex}", data.cropLevel[CropIndex].ToString()));
        

        //Debug.Log($"作物{CropIndex}經驗值增加：{CropExp * 50}，目前經驗值：{cropExp[CropIndex]}，等級：{cropLevel[CropIndex]}");
    }

    public void AddExpCompute(int CropIndex, int encreaseExp)
    {
        data.cropExp[CropIndex] += encreaseExp;
       //StartCoroutine(Database.ReadData(playerID.ToString()));
    }

    // 判斷是否升級
    private void CheckLevelUp(int CropIndex)
    {
        Debug.Log($"{CropIndex}");
        txt_level[CropIndex].text = data.cropLevel[CropIndex].ToString();
        txt_level[CropIndex].GetComponentInChildren<Slider>().value = (float)data.cropExp[CropIndex] / (float)ExpToNextLevel(data.cropLevel[CropIndex]);
     

        Debug.Log($"EXP{txt_level[CropIndex].GetComponentInChildren<Slider>().value}");
        txt_level[CropIndex].GetComponentInChildren<Slider>().GetComponentInChildren<TextMeshProUGUI>().text = $"{data.cropExp[CropIndex]}/{ExpToNextLevel(data.cropLevel[CropIndex])}";

        while (data.cropExp[CropIndex] >= ExpToNextLevel(data.cropLevel[CropIndex]))
        {
           
            data.cropExp[CropIndex] -= ExpToNextLevel(data.cropLevel[CropIndex]);
            data.cropLevel[CropIndex]++;
            
            txt_level[CropIndex].text = data.cropLevel[CropIndex].ToString();
            txt_level[CropIndex].GetComponentInChildren<Slider>().value = (float)data.cropExp[CropIndex]/ (float)ExpToNextLevel(data.cropLevel[CropIndex]);
            txt_level[CropIndex].GetComponentInChildren<Slider>().GetComponentInChildren<TextMeshProUGUI>().text =$"{data.cropExp[CropIndex]}/{ExpToNextLevel(data.cropLevel[CropIndex])}";
            
            //Debug.Log($"升級！目前等級：{cropLevel[CropIndex]}");

            // TODO: 可加技能點數、獎勵、解鎖物品等
        }
        unlockCharacter();//查看前1角等級
    }
    private int  ExpToNextLevel(int level)
    {
        return 100 + (level - 1) * 50; // 等級1需100、2需150、3需200...
    }
    public void constructBuilding()
    {
        int buildPrice = data.builds * 4000;
        if (data.money < buildPrice)
        {

            showTextMessage($"存款不足");
            return;
        }
        if (GridManager.OpenBuild())
        {
            //data.money = data.money - LandPrice;
            AddMoneyCompute(-buildPrice);
            //txt_money.text = data.money.ToString();
            string message = $"獲得新建設 (-{buildPrice} Coins)";
            showTextMessage(message);
            if(data.builds<facilityArray.Length)
                facilityArray[data.builds].showButtom(data.builds*2000);
            //showTextMessageMoney(LandPrice*-1);
            StartCoroutine(Database.UpdateData($"{playerID}/builds", data.builds.ToString()));

        }
    }
    public void buyLand()
    {
        int LandPrice = data.Lands * 5; // 每土的價格
        if (data.money < LandPrice)
        {

            showTextMessage($"存款不足");
            return;
        }
        if (GridManager.OpenGridCell())
        {
            //data.money = data.money - LandPrice;
            AddMoneyCompute(-LandPrice);
            //txt_money.text = data.money.ToString();
            string message = $"獲得新土地(-{LandPrice} Coins)";
            showTextMessage(message);

            //showTextMessageMoney(LandPrice*-1);
            StartCoroutine(Database.UpdateData($"{playerID}/Lands", data.Lands.ToString()));

        }

    }

    public void buyChatacter()//按鈕解鎖
    {
        int price = data.playerLevel *1000; // 每個角色的價格
        if (data.money < price)
        {
           
            showTextMessage($"存款不足");
            return;
        }
        if (SetPlayerLevel())
        {

            //money = money - price;// 暫定每個角色100元
            //txt_money.text = money.ToString();
            string message = $"獲得新角色-{price} Coins)";
            showTextMessage(message);
            AddMoneyCompute(-price);
            //showTextMessageMoney(price * -1);
            UnlockCharacter.text = $"解鎖角色\n({data.playerLevel * 1000} Coins)";
            

             //GameObject clonedTextGO = Instantiate(TextPerfab.gameObject);
             //clonedTextGO.GetComponent<TextMeshProUGUI>().text = $"{amount}元存入了匯豐銀行";
             //checkFacility();
             CheckLevelUp(data.playerLevel);
        }
    }

    public bool SetPlayerLevel()
    {
        if (data.playerLevel < cropName.Length)
        {
            AddPlayerLevel();
            CharaterIndex[data.playerLevel -1].SetActive(true);
            /*
            if (data.playerLevel == cropName.Length-1)
            {
                CharaterIndex[data.playerLevel].SetActive(false);//都解玩完 隱藏解鎖按鈕
            }
            */
            if (data.playerLevel == cropName.Length - 1)
            {
                CharaterIndex[data.playerLevel].SetActive(false);//都解玩完 隱藏解鎖按鈕
            }
            return true;
        }
        return false;
       
    }
    public void unlockCharacter()
    {
        if(data.cropLevel[data.playerLevel] >= 5 && data.playerLevel<10)
        if (SetPlayerLevel())
        {

            //money = money - price;// 暫定每個角色100元
            //txt_money.text = money.ToString();
            string message = $"獲得新角色 - {cropName[data.playerLevel]}";
            UnlockCharacterHint.text = $"當{cropName[data.playerLevel]}達到等級5時解鎖新角色";
            showTextMessage(message);
            //showTextMessageMoney(price * -1);
            //UnlockCharacter.text = $"解鎖角色\n({data.playerLevel * 1000} Coins)";


            //GameObject clonedTextGO = Instantiate(TextPerfab.gameObject);
            //clonedTextGO.GetComponent<TextMeshProUGUI>().text = $"{amount}元存入了匯豐銀行";
            //checkFacility();
            CheckLevelUp(data.playerLevel);
        }
    }
    public void checkFacility()
    {
        for (int i = 0; i < data.builds; i++)
        {
           
            if (facilityArray[i].isOpen == false)
            {
                facilityArray[i].Open();
                if (i+1 < facilityArray.Length)
                    facilityArray[i+1].showButtom((i+1)* 2000);
            }
            
            //Debug.Log($"{playerLevel / 2},{i},{facilityArray[i].isOpen}");
        }
    }

    public void RandomQuest()
    {
        data.questFalicity = UnityEngine.Random.Range(0,data.builds); // 重置任務索引
        data.questCropIndex= UnityEngine.Random.Range(1, data.playerLevel +1); // 重置任務角色索引
        int[] expList = new int[] { 50, 50, 50, 50, 50, 100, 100, 200, 300 }; // 任務經驗值列表

        data.questExp = expList[UnityEngine.Random.Range(0,expList.Length)]; // 重置任務經驗值

        int[] moneyList = new int[]{50, 50, 50, 50, 50, 100, 100, 200,300}; // 任務金錢獎勵列表
        data.questMoney = moneyList[UnityEngine.Random.Range(0,moneyList.Length)]; // 重置任務金錢獎勵
                                                                                   //Debug.Log($"任務{playerLevel}/{questCharacter} /{questExp} /{questMoney}");

        StartCoroutine(Database.UpdateData($"{playerID}/questFalicity", data.questFalicity.ToString()));
        StartCoroutine(Database.UpdateData($"{playerID}/questCropIndex", data.questCropIndex.ToString()));
        StartCoroutine(Database.UpdateData($"{playerID}/questExp", data.questExp.ToString()));
        StartCoroutine(Database.UpdateData($"{playerID}/questMoney", data.questMoney.ToString()));
        showQuestDepiction(); // 顯示任務描述



    }
    private void showQuestDepiction()
    {
        switch (data.questFalicity)//隨機任務
        {
            case 0:
                questDepiction.text = $"將{cropName[data.questCropIndex]}存入匯豐銀行";
                questReward.text = $"EXP +{data.questExp}\nCoin +{data.questMoney}";

                break;

            case 1:
                // 1級任務
                questDepiction.text = $"讓{cropName[data.questCropIndex]}參加婚禮";
                questReward.text = $"EXP +{data.questExp}\nCoin +{data.questMoney}";

                break;

            case 2:
                // 2級任務
                questDepiction.text = $"讓{cropName[data.questCropIndex]}畫本出攤";
                questReward.text = $"EXP +{data.questExp}\nCoin +{data.questMoney}";

                break;
            case 3:
                // 3級任務
                questDepiction.text = $"讓{cropName[data.questCropIndex]}看演唱會";
                questReward.text = $"EXP +{data.questExp}\nCoin +{data.questMoney}";

                break;

        }
    }
    public void CheckQuestComplete(int falicity, int cropIndex) // 檢查任務是否完成
    {
        if(data.questFalicity == falicity && data.questCropIndex == cropIndex)
        {

            AddExpCompute(cropIndex, data.questExp); // 計算經驗值
            AddMoneyCompute(data.questMoney); // 計算金錢獎勵
            exhibit.showBonus(data.questMoney, data.questExp);
            //showTextMessage($"完成任務：{questDepiction.text}");

            RandomQuest(); // 重置隨機任務
            
        }
        return;
    }

    public void AddBoxs(int i) 
    {
        data.Boxs = data.Boxs + i;
        StartCoroutine(Database.UpdateData($"{playerID}/Boxs", data.Boxs.ToString()));
    }
    public void AddPlayerLevel()
    {
        data.playerLevel++;
        StartCoroutine(Database.UpdateData($"{playerID}/playerLevel", data.playerLevel.ToString()));
    }
    public void UpdateCrop(int Position, int Status,int Level,int coin)
    {
        data.GridStatus[Position] = Status;
        data.GridLevel[Position] = Level; // 原本位置的等級
        StartCoroutine(Database.UpdateData($"{playerID}/GridStatus/{Position}", data.GridStatus[Position].ToString()));
        StartCoroutine(Database.UpdateData($"{playerID}/GridLevel/{Position}", data.GridLevel[Position].ToString()));
        UpdateCropCoin(Position, coin);
    }
    public void UpdateCropPosition(int prevPosition,int newPosition)
    {
        //作物到新位置
        UpdateCrop(newPosition, data.GridStatus[prevPosition], data.GridLevel[prevPosition], data.CropCoin[prevPosition]);
        UpdateCrop(prevPosition, -1, 0, 0); // 清除原本位置的作物
    }
    public void UpdateCropCoin(int Position, int RemainCoin)
    {
        data.CropCoin[Position] = RemainCoin;
        StartCoroutine(Database.UpdateData($"{playerID}/CropCoin/{Position}", data.CropCoin[Position].ToString()));
    }

    public void LoginRefreshValue()
    {
        ButtonOnClickScript.showBox.text = data.Boxs.ToString();
        txt_money.text = data.money.ToString();
        if(data.playerLevel< cropName.Length)
            UnlockCharacterHint.text = $"當{cropName[data.playerLevel]}達到等級5時解鎖新角色";
        if(data.playerLevel == cropName.Length)
            UnlockCharacterHint.text = $"Comming Soon ...";
        for (int i = 0; i < data.cropExp.Length-2; i++)
        {
            CheckLevelUp(i+1);//更新等級 經驗值顯示                
            if(i<data.playerLevel)
                CharaterIndex[i].SetActive(true);  
        }
        checkFacility();

        for (int i=20;i<data.Lands;i++)
        {
            GridManager.ShowGridCell(i);
        }
        string today = System.DateTime.Now.ToString("yyyyMMdd");
        Debug.Log($"今天日期：{today}，登入日期：{data.date}");
        if (int.Parse(data.date) < int.Parse(today))
        { 
            AddBoxs(300); // 每天登入獎勵10個箱子
            ButtonOnClickScript.showBox.text = data.Boxs.ToString();
            StartCoroutine(Database.UpdateData($"{playerID}/Boxs", data.Boxs.ToString()));
            data.date = today; // 更新日期
            StartCoroutine(Database.UpdateData($"{playerID}/date", data.date));
        }

        for (int i = 0; i < data.Lands; i++)
        {         
            if (data.GridStatus[i] != -1)// 如果格子狀態為-1，則跳過
                GridManager.SpawnSpecifyCrop(i/10, i%10, i / 10, i % 10, data.GridStatus[i], data.GridLevel[i], data.CropCoin[i]); // 生成指定位置的作物
        }

        showQuestDepiction();
    }
}
