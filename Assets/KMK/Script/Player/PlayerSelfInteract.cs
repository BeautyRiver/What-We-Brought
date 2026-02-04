using UnityEngine;

public class PlayerSelfInteract : MonoBehaviour, IInteractable
{    
    [SerializeField] private string content = "사용할 수 없을 것 같다.";
    public void Interact()
    {
        Item currentItem = EquipmentManager.Instance.equippedItem;
        if (currentItem == null) return;

        // 아이템 사용 시도 (성공 여부 확인)        
        if (currentItem.OnUseOnSelf()) // 바로 사용이 가능한 아이템들 (쪽지...)
        {    
            EquipmentManager.Instance.Unequip();
            if (currentItem.isConsumable)
            {
                // 인벤토리에서 삭제 & 손에서 없애기
                Inventory.instance.RemoveItem(currentItem);
            }
        }
        else
        {
            // 사용 실패 
            UIManager.instance.ShowTooltip(content, transform, 2f);
        }
    }
}