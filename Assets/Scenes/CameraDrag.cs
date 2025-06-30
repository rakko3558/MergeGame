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

    private Camera cam;
    private bool isZooming = false;
    private Vector2 prevTouchZeroPos;
    private Vector2 prevTouchOnePos;




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

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel"); // 取得滾輪軸值

        if (scroll != 0f)
        {
            cam.orthographicSize -= scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
        if (isZooming == true && Input.touchCount == 0)
        {
            isZooming = false;
        }
        else if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // 第一次觸發縮放時，儲存初始位置
            if (!isZooming)
            {
                prevTouchZeroPos = touchZero.position;
                prevTouchOnePos = touchOne.position;
                isZooming = true;
                return;
            }

            // 計算上一幀與這一幀的距離差
            float prevMagnitude = (prevTouchZeroPos - prevTouchOnePos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            // 縮放處理
            cam.orthographicSize -= difference * 0.01f;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, PhoneMinZoom, PhoneMaxZoom);

            // 更新上一幀位置
            prevTouchZeroPos = touchZero.position;
            prevTouchOnePos = touchOne.position;
        }
        else
        {



            //Debug.Log($"CameraDrag: {EventSystem.current.IsPointerOverGameObject()}");



            /*
            Vector3 mouseScreenPos = Input.mousePosition;
            mouseScreenPos.z = 10f; // 🔧 設定 z 軸為正數（對應 2D 相機）
            */


            /*       if (Input.GetMouseButtonDown(0))
                   {
                       if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                           return;


                       mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                       // 檢查是否點到任何 Collider2D
                       hit = Physics2D.OverlapPoint(mouseWorldPos);

                       if (hit != null)
                           return; //有碰到就 return

                       lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                       isDragging = true;
                   }

                   else if (Input.GetMouseButtonUp(0))
                   {
                       // ✅ 每幀更新位置，防止抖動
                       lastMousePosition = currentMousePosition;
                       isDragging = false;
                   }

                   if (isDragging)
                   {
                       currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                       Vector3 delta = lastMousePosition - currentMousePosition;

                       Vector3 newPosition = transform.position + delta;
                       newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
                       newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

                       transform.position = newPosition;


                   }
               }
            */
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                // 第一次觸發縮放時，儲存初始位置
                if (!isZooming)
                {
                    prevTouchZeroPos = touchZero.position;
                    prevTouchOnePos = touchOne.position;
                    isZooming = true;
                    return;
                }

                // 計算上一幀與這一幀的距離差
                float prevMagnitude = (prevTouchZeroPos - prevTouchOnePos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;

                // 縮放處理
                cam.orthographicSize -= difference * 0.01f;
                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, PhoneMinZoom, PhoneMaxZoom);

                // 更新上一幀位置
                prevTouchZeroPos = touchZero.position;
                prevTouchOnePos = touchOne.position;
            }
            else
            {
                isZooming = false;
            }


            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                    return;

                Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);

                if (hit == null) // 沒碰到東西才能拖曳
                {
                    lastMousePosition = mouseWorldPos;
                    //currentMousePosition = mouseWorldPos;
                    //lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    isDragging = true;
                }
            }

            else if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }

            if (isDragging)
            {
                mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 newPosition = transform.position + lastMousePosition - (Vector3)mouseWorldPos;
                newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
                newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

                transform.position = newPosition;

            }

            if (DragCrop)
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
            }

        }


    }
}
