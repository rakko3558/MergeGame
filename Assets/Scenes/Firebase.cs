using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

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

        Debug.Log(request.result);
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
        string random = Random.Range(0, 9).ToString() + Random.Range(0, 9).ToString() + Random.Range(0, 9).ToString() + Random.Range(0, 9).ToString(); // 生成隨機用戶ID

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
                Debug.Log($"{random}");
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

    IEnumerator ReadData(string key)
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

            //string loginDate = playerData.logingDate;
            
            save.data.Lands = playerData.Lands;


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
        Debug.Log(url);
        UnityWebRequest request = UnityWebRequest.Put(url, json);
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("更新成功");
            onComplete?.Invoke(true);
        }
        else
        {
            Debug.LogError($"更新失敗：{request.responseCode} {request.error}");
            onComplete?.Invoke(false);
        }
    }
}
[System.Serializable]
public class PlayerData
{
    //public string playerID;
    public int Boxs;
    public int money;
    public int playerLevel;

    public string logingDate;
    
    public int Lands;
    


}