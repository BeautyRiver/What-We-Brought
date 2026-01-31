using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance; 
    public List<Item> items;

    [SerializeField] private Transform slotParent;
    [SerializeField] private Slot[] slots;
#if UNITY_EDITOR
    private void OnValidate()
    {
        slots = slotParent.GetComponentsInChildren<Slot>();
    }
#endif
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);

        FreshSlot();
    }

    public void FreshSlot()
    {
        int i = 0;
        for (; i < items.Count && i < slots.Length; i++)
        {
            slots[i].item = items[i];
        }
        for (; i < slots.Length; i++)
        {
            slots[i].item = null;
        }
    }

    public void AddItem(Item item)
    {
        if (items.Count < slots.Length)
        {
            items.Add(item);
            FreshSlot();
        }
        else
        {
            print("슬롯이 가득 차 있습니다.");
        }
    }

    public void RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            FreshSlot(); // 슬롯 갱신

            // 만약 들고 있던 아이템이 사라졌으면 장착 해제
            if (EquipmentManager.Instance.equippedItem == item)
            {
                EquipmentManager.Instance.Unequip();
            }
        }
    }
}
