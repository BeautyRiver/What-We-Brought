using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance { get; private set; }
    [Header("UI 연결")]
    public GhostItemUI ghostUI;

    [Header("현재 장착된 아이템")]
    public Item equippedItem;
    public Slot equippedSlot { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    // 장착
    public void Equip(Item item)
    {
        equippedItem = item;
        if (ghostUI != null)
        {
            ghostUI.SetItemSprite(item); // 이미지 교체
            ghostUI.Hide();              // 일단 숨김
        }

        Debug.Log($"손에 듦: {item.itemName}");
    }

    // 해제
    public void Unequip()
    {
        equippedItem = null;
        if (equippedSlot != null)
        {
            equippedSlot.SetHighlight(false);
            equippedSlot = null;
        }

        if (ghostUI != null) ghostUI.Hide();

        Debug.Log("손 비움");
    }

    // 슬롯 UI 갱신용
    public void SetEquippedSlot(Slot slot)
    {
        if (equippedSlot != null) equippedSlot.SetHighlight(false);
        equippedSlot = slot;
        if (equippedSlot != null) equippedSlot.SetHighlight(true);
    }

    private void Update()
    {
        // 마우스 왼쪽 클릭
        if (Input.GetMouseButtonDown(0))
        {
            // 1. 아이템을 들고 있지 않으면 패스
            if (equippedItem == null) return;

            // 2. UI(인벤토리 등)를 클릭했다면 해제하지 않음 (슬롯 클릭 로직과 충돌 방지)
            if (EventSystem.current.IsPointerOverGameObject()) return;

            // 3. 월드의 상호작용 오브젝트를 클릭했는지 확인은 PlayerInteraction에서 처리함.
            // 여기서는 "아무것도 아닌 허공"을 클릭했을 때 취소하는 로직이 필요함.
            // 하지만 PlayerInteraction이 먼저 실행되어 Interact를 시도하고, 
            // 실패했을 때 Unequip을 부르는 것이 구조상 깔끔함.
            // 일단 여기서는 비워두고 PlayerInteraction에서 처리하는 것을 추천.
        }
    }
}