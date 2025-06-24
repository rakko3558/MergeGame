using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Storage : MonoBehaviour
{
    public int money = 1200000;// ���a����
    public TextMeshProUGUI txt_money;
    public TextMeshProUGUI[] txt_level= new TextMeshProUGUI[11];
    public GameObject panel_TextArea;
    public TextMeshProUGUI TextPerfab;

    public TextMeshProUGUI UnlockCharacter;

    public ButtonOnClick ButtonOnClickScript;

    //���Ȭ���
    public TextMeshProUGUI questDepiction;
    public TextMeshProUGUI questReward;
    public int questIndex = -1; // �ثe���ȯ��ޡA-1 �N��S������ 0:�Ȧ�A1:���B�A2:�X�u�A3:�t�۷|
    public int questCharacter = -1; // �ثe���ȯ��ޡA-1 �N��S������ 0:���A1:�Ȯh�A2:....
    public int questExp = 0; // ���ȸg���
    public int questMoney = 0; // ���Ȫ������y
    public int playerLevel = 1;

    public  int MaxLevel = 50;

    public int[] cropExp = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public int[] cropLevel = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }; // �@�����šA�w�]�� 1 ����
    public GridmManager GridManager;
    public int Lands = 20;


    public Facilitys[] facilityArray;
    //private int facility = 0; // 0:�Ȧ�A1:���B�A2:�X�u�A3:�t�۷|

    private string[] cropName = new string[] { "����", "�Ȯh", "�Ϧ��l", "���Z", "Taki", "+0", "SC", "Riku", "�֥d�C", "�i�N", "���D" };
    public GameObject[] CharaterIndex;
    // Start is called before the first frame update
    void Start()
    {
        checkFacility();

        for (int i = 1; i < cropLevel.Length; i++) 
        {

            CheckLevelUp(cropLevel[i]);
        }

        RandomQuest(); // ��l���H������ //�θ��J���e������
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

        // �ҥ� GameObject�]�p�G template �O hidden ���ܡ^
        
        //GameObject clonedTextGO = Instantiate(TextPerfab.gameObject);
        //clonedTextGO.GetComponent<TextMeshProUGUI>().text = message;
        //clonedTextGO.transform.SetParent(panel_TextArea.transform, false); // �]�w������
        clonedTextGO.SetActive(true); // �ҥ� GameObject�]�p�G template �O hidden ���ܡ^
    }

    public void AddMoney(int amount) //�Mfor �Ȧ�
    {
       
        string message = $"{amount}���s�J�F���׻Ȧ�";
        showTextMessage(message);

        AddMoneyCompute(amount);
        //GameObject clonedTextGO = Instantiate(TextPerfab.gameObject);
        //clonedTextGO.GetComponent<TextMeshProUGUI>().text = $"{amount}���s�J�F���׻Ȧ�";
        Debug.Log($"���a�����W�[�G{amount}�A�ثe�����G{money}");
    }
    public void AddMoneyCompute(int amount)
    {
        money = money + amount;
        txt_money.text = money.ToString();
    }
    public void AddExp(int facility ,int CropIndex, int CropExp)//CropExp�O�@���i�ƶ��q //�W�r������ �o�Mfor�]�I
    {

        //�p��g���
        if (cropLevel[CropIndex] == MaxLevel)
        {
            Debug.Log($"�w�����I�ثe���šG{cropLevel[CropIndex]}");
            return;
        }
        
        GameObject clonedTextGO = Instantiate(TextPerfab.gameObject);
        //��ܤ�r����
        int encreaseExp = 0;
        switch (facility)
        {
            case 0: // �Ȧ�
                int[] CropExpLevel = new int[] { 1, 3, 10, 50 };
                encreaseExp = CropExpLevel[CropExp];
                AddExpCompute(CropIndex,encreaseExp); // �p��g���
                clonedTextGO.GetComponent<TextMeshProUGUI>().text = $"�N" +
                    $"{cropName[CropIndex]}�s�J�F���׻Ȧ�I(EXP+{encreaseExp})";
                break;
            case 1: // ���B
                encreaseExp = facilityArray[1].expAmount;
                AddExpCompute(CropIndex, encreaseExp); // �p��g���
                string[] marriageNames = { "�Ȯh", "�Ϧ��l", "���Z", "Taki", "+0", "SC", "Riku", "�i�N", "���D" };
                clonedTextGO.GetComponent<TextMeshProUGUI>().text =
                            $"����{cropName[CropIndex]}��{marriageNames[Random.Range(0, marriageNames.Length)]}���B�F�I(EXP+{encreaseExp})";
                break;
            case 2: // �X�u
                encreaseExp = facilityArray[2].expAmount;
                AddExpCompute(CropIndex, encreaseExp); // �p��g���
                string CPName1;
                string CPName2;
                int a = Random.Range(0, 1);
                if (a==0)
                {
                    string[] CPNames = { "�֩`", "�R��", "�p��", "��", "�n�@", "���l", "���", "���a", "�ߧ�", "�O" };
                    CPName1 = CPNames[Random.Range(0, CPNames.Length)];
                    CPName2 = CPNames[Random.Range(0, CPNames.Length)];
                    while (CPName2 == CPName1)
                    {
                        CPName2 = CPNames[Random.Range(0, CPNames.Length)];
                    }
                }

                else
                {
                    string[] CPNames = {  "�筻", "��", "����", "Rupa", "��" };
                    CPName1 = CPNames[Random.Range(0, CPNames.Length)];
                    CPName2 = CPNames[Random.Range(0, CPNames.Length)];
                    while (CPName2 == CPName1)
                    {
                        CPName2 = CPNames[Random.Range(0, CPNames.Length)];
                    }
                }
                clonedTextGO.GetComponent<TextMeshProUGUI>().text =
                            $"{cropName[CropIndex]}�X�u�楻({CPName1} x {CPName2})�I(EXP+{encreaseExp})";
                break;
            case 3: // �t�۷|
                encreaseExp = facilityArray[3].expAmount;
                AddExpCompute(CropIndex, encreaseExp); // �p��g���
                string[] bandname = { "��������", "Ave Mujica", "MyGO!!!!!", "����L��" };//, "Poppin'Party", "Roselia", "Afterglow", "Pastel*Palettes", "Hello, Happy World!","RAISE A SUILEN", "Morfonica", "�ڭ��jMewType",""};



                clonedTextGO.GetComponent<TextMeshProUGUI>().text = $"{cropName[CropIndex]}�h�ݤF{bandname[Random.Range(0, bandname.Length)]}���t�۷|�I(EXP+{encreaseExp})";
                break;
            default:
                Debug.LogWarning("�������]�I�����I");
                return;
        }
        clonedTextGO.transform.SetParent(panel_TextArea.transform, false); 

        // �ҥ� GameObject�]�p�G template �O hidden ���ܡ^
        clonedTextGO.SetActive(true);
        //�T�{����
        CheckQuestComplete(facility, CropIndex); // �ˬd���ȬO�_����
        CheckLevelUp(CropIndex);

        Debug.Log($"�@��{CropIndex}�g��ȼW�[�G{CropExp * 50}�A�ثe�g��ȡG{cropExp[CropIndex]}�A���šG{cropLevel[CropIndex]}");
    }

    public void AddExpCompute(int CropIndex, int encreaseExp)
    {
        cropExp[CropIndex] += encreaseExp;
    }

    // �P�_�O�_�ɯ�
    private void CheckLevelUp(int CropIndex)
    {
        txt_level[CropIndex].text = cropLevel[CropIndex].ToString();
        Debug.Log($"{cropExp[CropIndex]}/{ExpToNextLevel(cropLevel[CropIndex])}");
        txt_level[CropIndex].GetComponentInChildren<Slider>().value = cropExp[CropIndex] / ExpToNextLevel(cropLevel[CropIndex]);
        txt_level[CropIndex].GetComponentInChildren<Slider>().GetComponentInChildren<TextMeshProUGUI>().text = $"{cropExp[CropIndex]}/{ExpToNextLevel(cropLevel[CropIndex])}";

        while (cropExp[CropIndex] >= ExpToNextLevel(cropLevel[CropIndex]))
        {
            
            cropExp[CropIndex] -= ExpToNextLevel(cropLevel[CropIndex]);
            cropLevel[CropIndex]++;
            
            txt_level[CropIndex].text = cropLevel[CropIndex].ToString();
            txt_level[CropIndex].GetComponentInChildren<Slider>().value = cropExp[CropIndex]/ ExpToNextLevel(cropLevel[CropIndex]);
            txt_level[CropIndex].GetComponentInChildren<Slider>().GetComponentInChildren<TextMeshProUGUI>().text =$"{cropExp[CropIndex]}/{ExpToNextLevel(cropLevel[CropIndex])}";
            Debug.Log($"�ɯšI�ثe���šG{cropLevel[CropIndex]}");

            // TODO: �i�[�ޯ��I�ơB���y�B���ꪫ�~��
        }
        
    }
    private int ExpToNextLevel(int level)
    {
        return 100 + (level - 1) * 50; // ����1��100�B2��150�B3��200...
    }

    public void buyLand()
    {
        int LandPrice = Lands * 5; // �C�g������
        if (money < LandPrice)
        {

            showTextMessage($"�s�ڤ���");
            return;
        }
        if (GridManager.OpenGridCell())
        {
            money = money - LandPrice;
            txt_money.text = money.ToString();
            string message = $"��o�s�g�a(-{LandPrice} Coins)";
            showTextMessage(message);
        }

    }

    public void buyChatacter()
    {
        int price = playerLevel*1000; // �C�Ө��⪺����
        if (money < price)
        {
           
            showTextMessage($"�s�ڤ���");
            return;
        }
        if (SetPlayerLevel(1))
        {
            
            money = money - price;// �ȩw�C�Ө���100��
            txt_money.text = money.ToString();
            string message = $"��o�s����(-{price} Coins)";
            showTextMessage(message);

            UnlockCharacter.text = $"���ꨤ��\n({playerLevel * 1000} Coins)";
            

             //GameObject clonedTextGO = Instantiate(TextPerfab.gameObject);
             //clonedTextGO.GetComponent<TextMeshProUGUI>().text = $"{amount}���s�J�F���׻Ȧ�";
             checkFacility();
             CheckLevelUp(playerLevel);
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
                CharaterIndex[playerLevel].SetActive(false);//���Ѫ��� ���ø�����s
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

    public void RandomQuest()//
    {
        questIndex = Random.Range(0,(playerLevel / 3)+1); // ���m���ȯ���
        questCharacter = Random.Range(0,playerLevel+1); // ���m���Ȩ������
        int[] expList = new int[] { 500, 600, 700, 800 }; // ���ȸg��ȦC��

        questExp = expList[Random.Range(0,expList.Length)]; // ���m���ȸg���

        int[] moneyList = new int[]{500, 600, 700, 800,900,1000,2000}; // ���Ȫ������y�C��
        questMoney = moneyList[Random.Range(0,moneyList.Length)]; // ���m���Ȫ������y
        Debug.Log($"����{playerLevel}/{questCharacter} /{questExp} /{questMoney}");

        switch (questIndex)//�H������
        {
            case 0:
                questDepiction.text = $"�N{cropName[questCharacter]}�s�J����׻Ȧ�";
                questReward.text = $"EXP +{questExp}\nCoin +{questMoney}";
                break;

            case 1:
                // 1�ť���
                questDepiction.text = $"��{cropName[questCharacter]}�i�浲�B";
                questReward.text = $"EXP +{questExp}\nCoin +{questMoney}";
                break;

            case 2:
                // 2�ť���
                questDepiction.text = $"��{cropName[questCharacter]}�e���X�u";
                questReward.text = $"EXP +{questExp}\nCoin +{questMoney}";
                break;
            case 3:
                // 3�ť���
                questDepiction.text = $"��{cropName[questCharacter]}�ݺt�۷|";
                questReward.text = $"EXP +{questExp}\nCoin +{questMoney}";
                break;

        }

        
    }

    public void CheckQuestComplete(int falicity, int cropIndex) // �ˬd���ȬO�_����
    {
        if(questIndex == falicity && questCharacter == cropIndex)
        {

            AddExpCompute(cropIndex, questExp); // �p��g���
            AddMoneyCompute(questMoney); // �p��������y

            showTextMessage($"�������ȡG{questDepiction.text}");
            RandomQuest(); // ���m�H������
            
        }
        return;
    }
}
