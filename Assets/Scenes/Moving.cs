using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour
{
    private bool isMoving = false;
    public float speed = 50f; // 移動速度
    public Vector3 targetPosition; // 目標位置
    public float stopDistance = 0.01f; // 停止移動的距離閾值
    public bool willDestroy = false;

    // Start is called before the first frame update
    void Start()
    {
        //this.enabled=false; // 禁用移動腳本
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving)
        {
           
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);// 移動到目標位置
            if (Vector3.Distance(transform.position, targetPosition) < stopDistance)
            {
                if (willDestroy)
                {
                    Destroy(gameObject);
                }
                isMoving = false; // 到達目標位置，停止移動
                this.enabled = false;
            } 
        }
      

    }

    public void StartMoving(bool Destroy, Vector3 newPosition)
    {
        willDestroy=Destroy; // 設定是否會銷毀物件
        this.enabled = true; // 禁用移動腳本
        targetPosition = newPosition; // 設定新的目標位置
        speed = Vector2.Distance(targetPosition , transform.position)*10;
        isMoving = true; // 開始移動
        Debug.Log($"Moving: {gameObject.name} to {targetPosition}");
    }
}
