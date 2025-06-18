using UnityEngine;
using UnityEngine.EventSystems;
//視角移動
public class CameraDrag : MonoBehaviour
{
    private Vector3 lastMousePosition;
    private bool isDragging = false;

    public float minX = -10.0f;
    public float maxX = 10.0f;
    public float minY = -10.0f;
    public float maxY = 10.0f;
    private Vector2 mouseWorldPos;
    private Collider2D hit;
    private Vector3 currentMousePosition;

    void Update()
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
            mouseWorldPos= Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 newPosition = transform.position + lastMousePosition - (Vector3)mouseWorldPos;
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

            transform.position = newPosition;

        }



    }
}
