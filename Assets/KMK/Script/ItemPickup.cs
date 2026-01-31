using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    [Header("아이템 정보")]
    public Item item;

    // 인터페이스 구현 (필수!)
    public void Interact()
    {
        Debug.Log($"🎒 아이템 습득: {item.itemName}");

        // 1. 인벤토리에 추가
        // InventoryManager.Instance.AddItem(this);

        // 2. 획득 효과음 재생
        // SoundManager.Play("PickUp");

        // 3. 필드에서 삭제
        Destroy(gameObject);
    }
}