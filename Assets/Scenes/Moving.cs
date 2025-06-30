using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour
{
    private bool isMoving = false;
    public float speed = 50f; // ���ʳt��
    public Vector3 targetPosition; // �ؼЦ�m
    public float stopDistance = 0.01f; // ����ʪ��Z���H��
    public bool willDestroy = false;

    // Start is called before the first frame update
    void Start()
    {
        //this.enabled=false; // �T�β��ʸ}��
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving)
        {
           
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);// ���ʨ�ؼЦ�m
            if (Vector3.Distance(transform.position, targetPosition) < stopDistance)
            {
                if (willDestroy)
                {
                    Destroy(gameObject);
                }
                isMoving = false; // ��F�ؼЦ�m�A�����
                this.enabled = false;
            } 
        }
      

    }

    public void StartMoving(bool Destroy, Vector3 newPosition)
    {
        willDestroy=Destroy; // �]�w�O�_�|�P������
        this.enabled = true; // �T�β��ʸ}��
        targetPosition = newPosition; // �]�w�s���ؼЦ�m
        speed = Vector2.Distance(targetPosition , transform.position)*10;
        isMoving = true; // �}�l����
        Debug.Log($"Moving: {gameObject.name} to {targetPosition}");
    }
}
