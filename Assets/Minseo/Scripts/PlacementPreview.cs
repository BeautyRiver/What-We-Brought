// PlacementPreview.cs
using UnityEngine;

public class PlacementPreview : MonoBehaviour
{
    [Header("장착/사용 관리")]
    public EquipmentManager equipmentManager;

    [Header("고스트 투명도 설정")]
    [Range(0f, 1f)]
    public float ghostAlpha = 0.5f;

    private GameObject ghostInstance;

    void Update()
    {
        UpdateGhost();
        HandlePlace();
    }

    void UpdateGhost()
    {
        if (equipmentManager == null)
            return;

        var item = equipmentManager.equippedItem;

        // 장착된 아이템이 없거나 설치형이 아니면 고스트 제거
        if (item == null || item.placePrefab == null)
        {
            if (ghostInstance != null)
                Destroy(ghostInstance);
            return;
        }

        // 고스트 프리팹이 없으면 생성
        if (ghostInstance == null)
        {
            ghostInstance = Instantiate(item.placePrefab);
            MakeGhost(ghostInstance);
        }

        // 마우스 위치로 이동 (2D 기준)
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        // 필요하면 offset 추가
        mouseWorld += (Vector3)item.placeOffset;

        ghostInstance.transform.position = mouseWorld;
    }

    void HandlePlace()
    {
        if (equipmentManager == null)
            return;

        var item = equipmentManager.equippedItem;
        if (item == null || item.placePrefab == null)
            return;

        // 좌클릭으로 확정 설치 (원하는 키로 바꿔도 됨)
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 placePos = ghostInstance != null
                ? ghostInstance.transform.position
                : Camera.main.ScreenToWorldPoint(Input.mousePosition);

            placePos.z = 0f;

            Instantiate(item.placePrefab, placePos, Quaternion.identity);
            Debug.Log($"{item.itemName} 설치 완료");

            // 한 번 설치 후 해제하고 싶으면:
            // equipmentManager.Unequip();
            // equipmentManager.SetEquippedSlot(null);
            // Destroy(ghostInstance);
        }
    }

    void MakeGhost(GameObject obj)
    {
        // 간단 버전: 모든 SpriteRenderer의 알파를 낮춘다.
        var sprites = obj.GetComponentsInChildren<SpriteRenderer>();
        foreach (var s in sprites)
        {
            Color c = s.color;
            c.a = ghostAlpha;
            s.color = c;
        }

        // 물리 충돌 막으려면 Collider 비활성화
        var colliders = obj.GetComponentsInChildren<Collider2D>();
        foreach (var col in colliders)
        {
            col.enabled = false;
        }
    }
}
