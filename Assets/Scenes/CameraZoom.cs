using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    /*
    public float zoomSpeed = 5f;         // 簎近は莱硉
    public float minZoom = 3f;           // 程跌à程┰
    public float maxZoom = 8f;          // 程跌à程┰环

    public float PhoneMinZoom = 3f;           // 程跌à程┰
    public float PhoneMaxZoom = 25f;          // 程跌à程┰环

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
        Debug.Log($"CameraZoom Start{cam}");
        float scroll = Input.GetAxis("Mouse ScrollWheel"); // 眔簎近禸

        if (scroll != 0f)
        {
            cam.orthographicSize -= scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }

        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // 材Ω牟祇罽纗﹍竚
            if (!isZooming)
            {
                prevTouchZeroPos = touchZero.position;
                prevTouchOnePos = touchOne.position;
                isZooming = true;
                return;
            }

            // 璸衡碫籔硂碫禯瞒畉
            float prevMagnitude = (prevTouchZeroPos - prevTouchOnePos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            // 罽矪瞶
            cam.orthographicSize -= difference * 0.01f;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, PhoneMinZoom, PhoneMaxZoom);

            // 穝碫竚
            prevTouchZeroPos = touchZero.position;
            prevTouchOnePos = touchOne.position;
        }
        else
        {
            isZooming = false;
        }
    }*/
}