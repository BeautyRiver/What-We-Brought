using UnityEngine;

[CreateAssetMenu(fileName = "New Note", menuName = "Item/Readable (Note)")]
public class ReadableItem : Item
{
    [Header("쪽지 내용")]
    [TextArea(5, 10)] public string content;

    public override bool OnUseOnSelf()
    {
        UIManager.instance.ShowNotePanel(content);
        return true; // 사용 성공
    }
}