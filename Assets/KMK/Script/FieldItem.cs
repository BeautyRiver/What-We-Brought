using UnityEngine;

public class FieldItem : MonoBehaviour, IInteractable
{
    public Item itemData;
    public void Interact()
    {
        Inventory.instance.AddItem(itemData);        
        Destroy(gameObject);
    }   
}
