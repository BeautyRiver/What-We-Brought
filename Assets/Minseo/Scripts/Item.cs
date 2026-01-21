using UnityEngine;

public enum ItemType
{
    Placeable,   // 설치형
    Consumable,  // 소모품 (나중용)
    Etc          // 기타
}

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;

    [Header("기본 정보")]
    public ItemType itemType;

    [Header("설치형 아이템 설정")]
    public GameObject placePrefab;   // 설치할 실제 프리팹
    public Vector2 placeOffset;      // 필요하면 사용 (지금은 고스트가 처리하므로 0이어도 됨)

    // 아이템 사용 진입점
    public void Use(Vector3 usePosition)
    {
        switch (itemType)
        {
            case ItemType.Placeable:
                UsePlaceable(usePosition);
                break;

            default:
                Debug.Log($"{itemName} 은(는) 아직 기능이 없습니다.");
                break;
        }
    }

    void UsePlaceable(Vector3 pos)
    {
        if (placePrefab == null)
        {
            Debug.LogWarning($"{itemName} : placePrefab 이 비어 있습니다.");
            return;
        }

        pos.z = 0f;
        Instantiate(placePrefab, pos, Quaternion.identity);
        Debug.Log($"{itemName} 설치 완료");
    }
}