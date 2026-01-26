using UnityEngine;

public class TestItemGet : MonoBehaviour
{
    [Header("카메라 & 줍기 설정")]
    public Camera cam;              // 메인 카메라
    public Transform player;        // 플레이어 Transform
    public float pickupRange = 3f;  // 줍기 최대 거리

    [Header("인벤토리")]
    public Inventory inventory;

    public float itemZ = 0f;        // 아이템이 있는 Z 평면
    public float yOffset = 0f;      // 2.5D 보정용

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryPickupByClick();
        }
    }

    void TryPickupByClick()
    {
        // 1. 마우스 → 월드 좌표
        Vector3 screenPos = Input.mousePosition;
        float zDist = itemZ - cam.transform.position.z;
        screenPos.z = zDist;

        Vector3 worldPos3 = cam.ScreenToWorldPoint(screenPos);
        worldPos3.y += yOffset; // 네가 맞춰 둔 보정값

        Vector2 clickPos = new Vector2(worldPos3.x, worldPos3.y);

        // 2. 클릭 위치에 있는 콜라이더 찾기
        Collider2D col = Physics2D.OverlapPoint(clickPos);
        if (col == null) return;

        // 3. 플레이어와의 거리 제한
        if (player != null)
        {
            float dist = Vector3.Distance(player.position, col.transform.position);
            if (dist > pickupRange)
            {
                // 너무 멀면 줍지 않음
                return;
            }
        }

        // 4. 실제 아이템 획득 처리
        IObjectItem clickInterface = col.GetComponent<IObjectItem>();
        if (clickInterface != null)
        {
            Item item = clickInterface.ClickItem();
            inventory.AddItem(item);
        }
    }
}