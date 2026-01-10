// EquipmentManager.cs
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance { get; private set; }

    [Header("현재 장착된 아이템")]
    public Item equippedItem;

    [Header("설치 시 기준이 될 Transform (플레이어 등)")]
    public Transform installOrigin;

    private Slot equippedSlot;    // 하이라이트를 줄 슬롯

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Equip(Item item)
    {
        equippedItem = item;
        Debug.Log($"{item.itemName} 장착");
    }

    public void Unequip()
    {
        if (equippedItem != null)
        {
            Debug.Log($"{equippedItem.itemName} 해제");
        }
        equippedItem = null;
    }

    public void SetEquippedSlot(Slot slot)
    {
        // 기존 슬롯 하이라이트 끄기
        if (equippedSlot != null)
            equippedSlot.SetHighlight(false);

        // 새 슬롯 기억
        equippedSlot = slot;

        // 새 슬롯 하이라이트 켜기
        if (equippedSlot != null)
            equippedSlot.SetHighlight(true);
    }

    public void UseEquippedItem()
    {
        if (equippedItem == null)
        {
            Debug.Log("장착된 아이템이 없습니다.");
            return;
        }

        if (equippedItem.placePrefab == null)
        {
            Debug.Log("이 아이템은 설치형이 아닙니다.");
            return;
        }

        // 실제 설치는 아래 2번에서 고스트 오브젝트 확정 위치를 기준으로 하게 바꿀 예정
    }
}
