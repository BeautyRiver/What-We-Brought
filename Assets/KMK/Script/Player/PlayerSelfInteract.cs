using UnityEngine;

public class PlayerSelfInteract : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        // 현재 손에 든 아이템 가져오기
        Item currentItem = EquipmentManager.Instance.equippedItem;

        // 아무것도 안 들고 있으면 반응 X
        if (currentItem == null) return;

        if (currentItem is ReadableItem)
        {
            currentItem.Use();
            EquipmentManager.Instance.Unequip();
        }
        else
        {            
            Debug.Log("이건 나한테 사용할 수 없어."); // UI로 띄워도 ㄱㅊ을듯
        }
    }
}