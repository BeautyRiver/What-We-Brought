using UnityEngine;

[CreateAssetMenu(fileName = "New Note", menuName = "Item/Readable (Note)")]
public class ReadableItem : Item
{
    [Header("쪽지 내용")]
    [TextArea(5, 10)] public string content;

    public override bool Use()
    {
        Debug.Log($"[쪽지 읽기] {itemName}");

        // UIManager에게 쪽지 내용을 띄워달라고 요청
        if (UIManager.instance != null)
        {
            UIManager.instance.ShowNotePanel(content);
        }

        // false를 반환해야 아이템이 사라지지 않음! (쪽지는 읽어도 안 없어지니까)
        return false;
    }
}