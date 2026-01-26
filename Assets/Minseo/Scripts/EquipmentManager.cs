// EquipmentManager.cs
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static EquipmentManager Instance { get; private set; }

    [Header("현재 장착된 아이템")]
    public Item equippedItem;          // 지금 선택/장착된 아이템

    [Header("설치 시 기준이 될 Transform (플레이어 등)")]
    public Transform installOrigin;    // 필요하면 나중에 사용할 기준점

    // 현재 하이라이트(선택) 중인 슬롯
    public Slot equippedSlot { get; private set; }

    private void Awake()
    {
        // 싱글톤 초기화
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // 아이템 장착
    public void Equip(Item item)
    {
        equippedItem = item;
        Debug.Log($"{item.itemName} 장착");
    }

    // 아이템 해제
    public void Unequip()
    {
        if (equippedItem != null)
        {
            Debug.Log($"{equippedItem.itemName} 해제");
        }
        equippedItem = null;
    }

    // 어떤 슬롯이 선택/장착 상태인지 갱신 + 하이라이트 On/Off
    public void SetEquippedSlot(Slot slot)
    {
        // 이전 슬롯 테두리 끄기
        if (equippedSlot != null)
            equippedSlot.SetHighlight(false);

        // 새 슬롯 기억
        equippedSlot = slot;

        // 새 슬롯 테두리 켜기
        if (equippedSlot != null)
            equippedSlot.SetHighlight(true);
    }

    // 고스트가 알려 준 위치로 아이템 사용
    public void UseEquippedAt(Vector3 position)
    {
        if (equippedItem == null)
        {
            Debug.Log("장착된 아이템이 없습니다.");
            return;
        }

        equippedItem.Use(position);
    }
}
