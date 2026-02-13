using UnityEngine;

public class FieldItem : MonoBehaviour, IInteractable
{
    public Item itemData;
    public bool isHideWhenStart;
    public bool isDestory = false;

    private void Start()
    {
        if (isHideWhenStart)
            this.gameObject.SetActive(false);
    }
    public void Interact()
    {
        Inventory.instance.AddItem(itemData);        
        if (isDestory)
            Destroy(gameObject);
        else
        {
            this.GetComponent<Collider>().enabled = false;
            
        }
    }   
}
