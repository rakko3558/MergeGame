using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float zoomSpeed = 5f;         // �u�������t��
    public float minZoom = 3f;           // �̤p�����]�̩Ԫ�^
    public float maxZoom = 8f;          // �̤j�����]�̩Ի��^

    public float PhoneMinZoom = 3f;           // �̤p�����]�̩Ԫ�^
    public float PhoneMaxZoom = 25f;          // �̤j�����]�̩Ի��^

    private Camera cam;
    private bool isZooming = false;
    private Vector2 prevTouchZeroPos;
    private Vector2 prevTouchOnePos;

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

        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // �Ĥ@��Ĳ�o�Y��ɡA�x�s��l��m
            if (!isZooming)
            {
                prevTouchZeroPos = touchZero.position;
                prevTouchOnePos = touchOne.position;
                isZooming = true;
                return;
            }

            // �p��W�@�V�P�o�@�V���Z���t
            float prevMagnitude = (prevTouchZeroPos - prevTouchOnePos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            // �Y��B�z
            cam.orthographicSize -= difference * 0.01f;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, PhoneMinZoom, PhoneMaxZoom);

            // ��s�W�@�V��m
            prevTouchZeroPos = touchZero.position;
            prevTouchOnePos = touchOne.position;
        }
        else
        {
            isZooming = false;
        }
    }
}