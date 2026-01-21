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
        if (item == null || item.itemType != ItemType.Placeable || item.placePrefab == null)
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

        // 필요하면 offset 사용
        mouseWorld += (Vector3)item.placeOffset;

        ghostInstance.transform.position = mouseWorld;
    }

    void HandlePlace()
    {
        if (equipmentManager == null)
            return;

        var item = equipmentManager.equippedItem;
        if (item == null || item.itemType != ItemType.Placeable || item.placePrefab == null)
            return;

        // 좌클릭으로 설치 확정 (원하면 다른 키로 변경)
        if (Input.GetMouseButtonDown(0))
        {
            if (ghostInstance == null)
                return;

            Vector3 placePos = ghostInstance.transform.position;
            placePos.z = 0f;

            // 실제 설치는 Item.Use가 담당
            equipmentManager.UseEquippedAt(placePos);
        }
    }

    void MakeGhost(GameObject obj)
    {
        // 스프라이트 투명하게
        var sprites = obj.GetComponentsInChildren<SpriteRenderer>();
        foreach (var s in sprites)
        {
            Color c = s.color;
            c.a = ghostAlpha;
            s.color = c;
        }

        // 고스트는 충돌 안 하게
        var colliders = obj.GetComponentsInChildren<Collider2D>();
        foreach (var col in colliders)
        {
            col.enabled = false;
        }
    }
}