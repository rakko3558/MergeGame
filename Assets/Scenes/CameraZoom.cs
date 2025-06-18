using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float zoomSpeed = 5f;         // 簎近は莱硉
    public float minZoom = 3f;           // 程跌à程┰
    public float maxZoom = 5f;          // 程跌à程┰环

    public float PhoneMinZoom = 5f;           // 程跌à程┰
    public float PhoneMaxZoom = 20f;          // 程跌à程┰环

    private Camera cam;
    Vector2 ZerodeltaPosition;
    Vector2 OnedeltaPosition;
    private bool isZooming = false; // 琌タ罽
    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel"); // 眔簎近禸

        if (scroll != 0f)
        {
            cam.orthographicSize -= scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }

        if (Input.touchCount == 2 && isZooming==false)
        {
            ZerodeltaPosition= Vector2.zero;
            OnedeltaPosition = Vector2.zero;
            isZooming = true;
           
        }
        if (Input.touchCount != 2)
        {
            isZooming=false; // 狦ぃ琌ㄢ牟北玥氨ゎ罽
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

            cam.orthographicSize -= difference * 0.01f;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, PhoneMinZoom, PhoneMaxZoom);

            ZerodeltaPosition = touchZeroPrevPos;
            OnedeltaPosition = touchOnePrevPos;
        }
    }
}