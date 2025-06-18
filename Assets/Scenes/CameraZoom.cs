using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float zoomSpeed = 5f;         // �u�������t��
    public float minZoom = 3f;           // �̤p�����]�̩Ԫ�^
    public float maxZoom = 5f;          // �̤j�����]�̩Ի��^

    public float PhoneMinZoom = 5f;           // �̤p�����]�̩Ԫ�^
    public float PhoneMaxZoom = 20f;          // �̤j�����]�̩Ի��^

    private Camera cam;
    Vector2 ZerodeltaPosition;
    Vector2 OnedeltaPosition;
    private bool isZooming = false; // �O�_���b�Y��
    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel"); // ���o�u���b��

        if (scroll != 0f)
        {
            cam.orthographicSize -= scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }

        if (Input.touchCount == 2 && isZooming==false)
        {
            
            isZooming = true;
           ZerodeltaPosition = Input.GetTouch(0).position;
            OnedeltaPosition = Input.GetTouch(1).position;
        }
        if (Input.touchCount != 2)
        {
            isZooming=false; // �p�G���O���Ĳ���A�h�����Y��
        }
        if (isZooming)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - ZerodeltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - OnedeltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            cam.orthographicSize -= difference * 0.001f;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, PhoneMinZoom, PhoneMaxZoom);

            ZerodeltaPosition = touchZero.position;
            OnedeltaPosition = touchOne.position;
        }
    }
}