using UnityEngine;
using System.Collections.Generic;

public class CloudManager : MonoBehaviour
{
    public GameObject cloudPrefab; // 一個預製的雲物件（要有 SpriteRenderer）
    public float spawnInterval = 3f; // 每幾秒生成一朵雲
    public float moveSpeed = 1f;     // 雲移動速度
    public Vector2 spawnYRange = new Vector2(-2f, 3f); // 雲出現的高度範圍
    public float leftX = -10f;       // 出現位置X
    public float rightX = 10f;       // 離開位置X

    private Sprite[] cloudSprites;
    private List<GameObject> activeClouds = new List<GameObject>();

    void Start()
    {
        cloudSprites = new Sprite[3];
        cloudSprites[0] = Resources.Load<Sprite>("Source/cloud_01");
        cloudSprites[1] = Resources.Load<Sprite>("Source/cloud_02");
        cloudSprites[2] = Resources.Load<Sprite>("Source/cloud_03");

        InvokeRepeating(nameof(SpawnCloud), 0f, spawnInterval);
    }

    void SpawnCloud()
    {
        GameObject cloud = Instantiate(cloudPrefab);
        cloud.transform.position = new Vector3(leftX, Random.Range(spawnYRange.x, spawnYRange.y), 0f);
        SpriteRenderer sr = cloud.GetComponent<SpriteRenderer>();
        sr.sprite = cloudSprites[Random.Range(0, cloudSprites.Length)];
        activeClouds.Add(cloud);
    }

    void Update()
    {
        for (int i = activeClouds.Count - 1; i >= 0; i--)
        {
            GameObject cloud = activeClouds[i];
            cloud.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

            if (cloud.transform.position.x > rightX)
            {
                Destroy(cloud);
                activeClouds.RemoveAt(i);
            }
        }
    }
}
