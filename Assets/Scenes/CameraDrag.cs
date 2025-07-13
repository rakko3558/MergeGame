using UnityEngine;
using UnityEngine.EventSystems;
//視角移動
public class CameraDrag : MonoBehaviour
{

    public float zoomSpeed = 5f;         // 滾輪反應速度
    public float minZoom = 3f;           // 最小視角（最拉近）
    public float maxZoom = 8f;          // 最大視角（最拉遠）

    public float PhoneMinZoom = 3f;           // 最小視角（最拉近）
    public float PhoneMaxZoom = 25f;          // 最大視角（最拉遠）

    public float perspectiveZoomSpeed = 0.5f;        // The rate of change of the field of view in perspective mode.
    public float orthoZoomSpeed = 0.5f;        // The rate of change of the orthographic size in orthographic mode.

    private Camera cam;
    private bool isZooming = false;
    private Vector2 prevTouchZeroPos;
    private Vector2 prevTouchOnePos;
    private Touch touchZero;
    private Touch touchOne;
    float touchDist = 0;
    float lastDist = 0;


    private Vector3 lastMousePosition;
    private bool isDragging = false;

    public float minX = -10.0f;
    public float maxX = 10.0f;
    public float minY = -10.0f;
    public float maxY = 10.0f;
    private Vector2 mouseWorldPos;
    private Collider2D hit;
    private Vector3 currentMousePosition;

    public bool DragCrop = false;
    public GameObject Crop;
    private float borderThickness = 50.0f;
    private float scrollSpeed = 5f;
    private int touchStatus = 0; // 0: 無觸控, 1: 單指觸控, 2: 雙指觸控
    
    bool mouseisdown = false;
    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // 處理滾輪縮放
        float scroll = Input.GetAxis("Mouse ScrollWheel"); // 取得滾輪軸值

        if (scroll != 0f)
        {
            cam.orthographicSize -= scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
        if (Input.GetMouseButtonDown(0))
        {
            mouseisdown = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            mouseisdown = false;
        }
        if (DragCrop)//如果拖曳作物
        {
            Vector3 pos = transform.position;
            Vector3 DraggingPosition = Camera.main.WorldToScreenPoint(Crop.transform.position);

            if (DraggingPosition.x >= Screen.width - borderThickness)
            {
                pos.x += scrollSpeed * Time.deltaTime;
            }
            else if (DraggingPosition.x <= borderThickness)
            {
                pos.x -= scrollSpeed * Time.deltaTime;
            }
            if (DraggingPosition.y >= Screen.height - borderThickness)
            {
                pos.y += scrollSpeed * Time.deltaTime;
            }
            else if (DraggingPosition.y <= borderThickness)
            {
                pos.y -= scrollSpeed * Time.deltaTime;


            }

            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);

            transform.position = pos;
            return;
        }

       


        if (mouseisdown==false && Input.touchCount == 0 )
            touchStatus = 0;

        if (Input.touchCount == 1 && touchStatus < 2 || mouseisdown && touchStatus < 2)
        {
            if (touchStatus < 1)
            {
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                    return;

                Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);
                if (hit == null) // 沒碰到東西才能拖曳
                {
                    lastMousePosition = mouseWorldPos;
                    touchStatus = 1;
                    return;
                }
            }
        }
        if (Input.touchCount == 2 )
        {

            // 第一次觸發縮放時，儲存初始位置
            if (touchStatus < 2)
            {
                
                prevTouchZeroPos = touchZero.position;
                prevTouchOnePos = touchOne.position;
                touchStatus = 2; // 雙指觸控
                return;
            }
        }
        if (touchStatus==2 && Input.touchCount != 2) 
        {
            touchStatus = 3;
        }

        if (touchStatus ==0)
        {
            //Debug.Log("YYY:{}");
        }

        if (touchStatus==1)
        { 
            if (touchStatus < 1)
            {
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                    return;

                Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);
          
                if (hit == null) // 沒碰到東西才能拖曳
                {
                    lastMousePosition = mouseWorldPos;
                    touchStatus = 1;
                    return;
                }
            }

            mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 newPosition = transform.position + lastMousePosition - (Vector3)mouseWorldPos;
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

            transform.position = newPosition;
        }
       
        if (touchStatus == 2)
        {

            // If there are two touches on the device...
            if (Input.touchCount == 2)
            {
                // Store both touches.
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                // Find the position in the previous frame of each touch.
                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                // Find the magnitude of the vector (the distance) between the touches in each frame.
                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                // Find the difference in the distances between each frame.
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                // If the camera is orthographic...
                if (GetComponent<Camera>().orthographic)
                {
                    // ... change the orthographic size based on the change in distance between the touches.
                    GetComponent<Camera>().orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

                    // Make sure the orthographic size never drops below zero.
                    GetComponent<Camera>().orthographicSize = Mathf.Max(GetComponent<Camera>().orthographicSize, 0.1f);
                }
                else
                {
                    // Otherwise change the field of view based on the change in distance between the touches.
                    GetComponent<Camera>().fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

                    // Clamp the field of view to make sure it's between 0 and 180.
                    GetComponent<Camera>().fieldOfView = Mathf.Clamp(GetComponent<Camera>().fieldOfView, 0.1f, 179.9f);
                }
            }
            /*  touchZero = Input.GetTouch(0);
             touchOne = Input.GetTouch(1);

             // 計算上一幀與這一幀的距離差
             float prevMagnitude = (prevTouchZeroPos - prevTouchOnePos).magnitude;
             float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

             float difference = currentMagnitude - prevMagnitude;

             // 縮放處理
             cam.orthographicSize -= difference * 0.01f; 
             cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, PhoneMinZoom, PhoneMaxZoom);

             // 更新上一幀位置
             prevTouchZeroPos = touchZero.position;
             prevTouchOnePos = touchOne.position; */

        }
       

        /*
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Began && touch2.phase == TouchPhase.Began)
            {
                lastDist = Vector2.Distance(touch1.position, touch2.position);
            }

            if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
            {
                float newDist = Vector2.Distance(touch1.position, touch2.position);
                touchDist = lastDist - newDist;
                lastDist = newDist;

                // Your Code Here
                Camera.main.fieldOfView += touchDist * 0.1f;
            }
        }
        */

    }
}
