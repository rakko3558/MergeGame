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

    void Update()
    {
        //Debug.Log($"CameraDrag: {EventSystem.current.IsPointerOverGameObject()}");
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 檢查是否點到任何 Collider2D
        Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);

        if (hit != null)
        {
            //Debug.Log("有點到物件：" + hit.name);
            return; // 👉 有碰到就 return
        }

        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 delta = lastMousePosition - currentMousePosition;

            Vector3 newPosition = transform.position + delta;
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

            transform.position = newPosition;

            // ✅ 每幀更新位置，防止抖動
            //lastMousePosition = currentMousePosition;
        }
    }
}
