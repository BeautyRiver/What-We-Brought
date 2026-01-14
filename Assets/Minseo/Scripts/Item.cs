using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;

    [Header("설치형 아이템 설정")]
    public GameObject placePrefab;
    public Vector2 placeOffset;
}
