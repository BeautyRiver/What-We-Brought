using UnityEngine;

public class ObjectItem : MonoBehaviour, IObjectItem
{
    [Header("아이템")]
    public Item item;
    [Header("아이템 이미지")]
    public SpriteRenderer itemImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemImage.sprite = item.itemImage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Item ClickItem()
    {
        return this.item;
    }

}
