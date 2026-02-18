using UnityEngine;

public class FieldItem : MonoBehaviour, IInteractable
{
    public Item itemData;
    public bool isDisableWhenStart;
    public bool isEquipAfterDestory = false;

    private SpriteRenderer spr;

    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        if (isDisableWhenStart)
            this.gameObject.SetActive(false);

        if (DataManager.instance.HasItem(itemData.itemID))
        {
            Debug.Log(itemData.name + "<< 아이템을 획득한 적이 있습니다.");
            this.gameObject.SetActive(false);
        }
    }
    public void Interact()
    {
        Inventory.instance.AddItem(itemData);
        SoundManager.instance.PlaySound("ItemSound");

        if (isEquipAfterDestory)
            Destroy(gameObject);
        else
        {
            this.GetComponent<Collider>().enabled = false;            
        }
    }   
}
