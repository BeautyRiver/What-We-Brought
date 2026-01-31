using UnityEngine;

public abstract class Item : ScriptableObject
{
    [Header("공통 데이터")]
    public string itemName;
    public Sprite itemImage;
    [TextArea] public string description;

    // 반환값 bool: true면 사용 후 삭제(소모), false면 유지
    public abstract bool Use();
}