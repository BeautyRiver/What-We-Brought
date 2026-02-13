using UnityEngine;

public abstract class Item : ScriptableObject
{
    [Header("공통 데이터")]
    public string itemName;
    public Sprite itemImage;
    [TextArea] public string description;

    [Header("사운드 설정")]
    public string useSoundName;


    // 사용하면 사라지는가
    public bool isConsumable = false;

    // 사용 시도 성공했는지 
    public virtual bool OnUseOnSelf()
    {
        return false;
    }
}