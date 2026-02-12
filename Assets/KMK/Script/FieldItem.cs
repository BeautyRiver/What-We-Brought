using DarkTonic.MasterAudio;
using UnityEngine;
using DarkTonic.MasterAudio;

public class FieldItem : MonoBehaviour, IInteractable
{
    public Item itemData;
    public void Interact()
    {
        if (Inventory.instance != null)
            Inventory.instance.AddItem(itemData);

        MasterAudio.PlaySound3DAtVector3("ItemSound", transform.position);
        Destroy(gameObject);
      
    }   
}
