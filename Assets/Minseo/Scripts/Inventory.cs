using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    [Header("아이템 데이터베이스 (게임의 모든 아이템을 여기에 등록!)")]
    public List<Item> allGameItems = new List<Item>(); // ⭐ String -> Item 변환용 도감

    [Header("현재 보유 아이템")]
    public List<Item> items = new List<Item>(); // 실제 인벤토리 내용물

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
    }

    private void Start()
    {
        // 게임 시작 시, 저장된 데이터 불러오기
        LoadInventoryFromData();
    }

    // 📂 저장된 String 리스트를 보고 -> 실제 Item 리스트로 복구
    public void LoadInventoryFromData()
    {
        items.Clear(); // 기존 목록 비우기

        // 1. DataManager에서 저장된 ID 목록 가져오기
        List<string> savedIDs = DataManager.instance.currentData.inventory;

        // 2. ID를 하나씩 꺼내서 실제 아이템으로 변환
        foreach (string id in savedIDs)
        {
            // 도감(allGameItems)에서 ID가 같은 아이템을 찾는다.
            Item foundItem = allGameItems.Find(x => x.itemID == id);

            if (foundItem != null)
            {
                items.Add(foundItem);
            }
            else
            {
                Debug.LogWarning($"아이템 도감에서 ID를 찾을 수 없음: {id}");
            }
        }

        // 3. UI 갱신
        FreshSlot();
    }

    // 🎨 UI 슬롯 갱신 (기존 코드와 동일)
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

    // ➕ 아이템 획득 (UI 갱신 + 데이터 저장)
    // worldObjectID: 맵에 있던 오브젝트 ID (없으면 null)
    public void AddItem(Item item)
    {
        if (items.Count < slots.Length)
        {
            items.Add(item); // 1. 화면(UI)에 추가

            // 2. 데이터(DataManager)에 저장 요청
            // (만약 단순 획득이면 worldObjectID 없이, 필드 습득이면 ID 포함)
            DataManager.instance.AddCollectedItem(item.itemID);
            FreshSlot();
        }
        else
        {
            print("슬롯이 가득 차 있습니다.");
        }
    }

    // ➖ 아이템 삭제/사용 (UI 갱신 + 데이터 저장)
    public void RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item); // 1. 화면(UI)에서 삭제

            // 2. 데이터(DataManager)에서 삭제 요청
            DataManager.instance.UseItem(item.itemID);

            FreshSlot(); // 슬롯 갱신

            // 장착 해제 로직
            if (EquipmentManager.Instance.equippedItem == item)
            {
                EquipmentManager.Instance.Unequip();
            }
        }
    }
}