using UnityEngine;

public class TestItemGet : MonoBehaviour
{
    [Header("카메라")]
    public Camera cam;          // CinemachineBrain 달린 실제 메인 카메라
    public float yOffset = 0f;  // 2.5D 보정용 Y 오프셋

    [Header("인벤토리")]
    public Inventory inventory;

    // 아이템이 놓인 Z 평면 (전부 0이면 0)
    public float itemZ = 0f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 screenPos = Input.mousePosition;

            // 카메라에서 아이템 평면까지의 거리
            float zDist = itemZ - cam.transform.position.z;
            screenPos.z = zDist;

            // 3D에서 마우스가 가리키는 월드 좌표
            Vector3 worldPos3 = cam.ScreenToWorldPoint(screenPos);

            // 2.5D 카메라 기울기 때문에 어긋나는 Y를 보정
            worldPos3.y += yOffset;

            Vector2 clickPos = new Vector2(worldPos3.x, worldPos3.y);

            Collider2D col = Physics2D.OverlapPoint(clickPos);
            if (col != null)
            {
                IObjectItem clickInterface = col.GetComponent<IObjectItem>();
                if (clickInterface != null)
                {
                    Item item = clickInterface.ClickItem();
                    inventory.AddItem(item);
                }
            }
        }
    }
}