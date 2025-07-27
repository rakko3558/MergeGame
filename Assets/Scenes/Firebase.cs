using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System; //
using System.Linq;
public class FirebaseTest : MonoBehaviour
{
    public Storage save;
    public GameObject createAccPage;
    public GameObject loginPage;
    /*
    public void Start()
    {
        // 寫入資料
        StartCoroutine(WriteData("0000", "{\"score\":100}"));

        // 讀取資料
        StartCoroutine(ReadData("0000"));
    }
    */
    public IEnumerator WriteData(string key, string json)
    {
        string url = $"https://merge-3ac49-default-rtdb.firebaseio.com/{key}.json";
        UnityWebRequest request = UnityWebRequest.Put(url, json);
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            Debug.Log("寫入成功：" + request.downloadHandler.text);
        else
            Debug.LogError($"寫入失敗：{request.responseCode} {request.error}");
    }

    public IEnumerator checkExist(string key)
    {
        

        string url = $"https://merge-3ac49-default-rtdb.firebaseio.com/{key}.json";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success && request.downloadHandler.text!="null")
        {
            loginPage.GetComponent<login>().canvas.SetActive(false); // 隱藏登入介面return;
            StartCoroutine(ReadData(key));
            //save
        }
        else
        {
            loginPage.GetComponent<login>().info.SetActive(true); // 顯示錯誤信息            
        }
    }
    public IEnumerator generateID()
    {
        string random = UnityEngine.Random.Range(0, 9).ToString() + UnityEngine.Random.Range(0, 9).ToString() + UnityEngine.Random.Range(0, 9).ToString() + UnityEngine.Random.Range(0, 9).ToString(); // 生成隨機用戶ID

        string url = $"https://merge-3ac49-default-rtdb.firebaseio.com/{random}.json";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();



        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;

            // Firebase 若該 key 不存在，會回傳 "null"
            if (json == "null")
            {
                createAccPage.GetComponent<CreatAccount>().randomID.text = random;
            }
            else
            {

                StartCoroutine(generateID());
            }
        }
        else
        {
            Debug.LogError($"讀取失敗：{request.responseCode} {request.error}");
        }

       
          
        
    }

    public IEnumerator ReadData(string key)
    {
        string url = $"https://merge-3ac49-default-rtdb.firebaseio.com/{key}.json";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        
        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            Debug.Log("讀取成功：" + json);

            // 將 JSON 字串轉成 C# 物件
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);

            // 賦值給變數

            save.playerID = key;
            save.data.Boxs = playerData.Boxs;
            save.data.money = playerData.money;
            save.data.playerLevel = playerData.playerLevel;
            save.data.cropExp = playerData.cropExp; // 初始化 CropLevel 陣列
            save.data.cropLevel = playerData.cropLevel; // 初始化 CropLevel 陣列
            if (save.data.cropLevel == null)
            {
                save.data.cropLevel = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }; // 確保陣列不為 null，避免後續操作出錯
            }
            if (save.data.cropExp == null)
            {
                save.data.cropExp = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; // 確保陣列不為 null，避免後續操作出錯
            }
                //string loginDate = playerData.logingDate;

            save.data.Lands = playerData.Lands;
            save.data.date = playerData.date;
            if (save.data.date == null || save.data.date == "")
            {
                save.data.date = System.DateTime.Now.ToString("yyyyMMdd");
            }

            save.data.GridStatus = playerData.GridStatus;
            save.data.GridLevel = playerData.GridLevel;
            save.data.CropCoin = playerData.CropCoin;
            if (save.data.GridStatus == null)
            {
                save.data.GridStatus = Enumerable.Repeat(-1, 120).ToArray();
                save.data.GridLevel = new int[120];
                save.data.CropCoin = new int[120];
            }
            //寫回去 以免資料不完整 寫入有缺誤 下次讀取變null值

            save.data.questFalicity= playerData.questFalicity ;
            save.data.questCropIndex=playerData.questCropIndex;
            save.data.questMoney=    playerData.questMoney;
            save.data.questExp= playerData.questExp ;
            save.data.builds = playerData.builds;
            StartCoroutine(WriteData(save.playerID, JsonUtility.ToJson(save.data)));

            save.LoginRefreshValue();
            // 測試用印出
            //  Debug.Log($"BoxNum: {box}, loginDate: {loginDate}, playerLevel: {playerLevel}");
        }
        else
        {
            Debug.LogError("讀取失敗：" + request.error);
        }
    }

    public IEnumerator UpdateData(string path, string value, System.Action<bool> onComplete = null)
    {
        string url = $"https://merge-3ac49-default-rtdb.firebaseio.com/{path}.json";
        string json = value; // 直接數字，不用引號
        UnityWebRequest request = UnityWebRequest.Put(url, json);
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            onComplete?.Invoke(true);
        }
        else
        {
            onComplete?.Invoke(false);
        }
    }
}
[System.Serializable]
public class PlayerData
{
    public int money;// 玩家金錢
    public int playerLevel;
    public int Lands;
    public int Boxs;// 剩餘箱子數量
    public int[] cropExp;
    public int[] cropLevel; // 作物等級，預設為 1 等級
    public string date; // 登入日期
    public int[] GridStatus;
     public int[] GridLevel;
    public int[] CropCoin;
    public int questFalicity; // 任務設施
    public int questCropIndex;
    public int questMoney;
    public int questExp;
    public int builds;
}