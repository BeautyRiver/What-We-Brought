using UnityEngine;

public class BasicObject : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log(gameObject.name + "와 상호작용 성공!");
        // 여기에 아이템 획득, 문 열기 등 로직 추가
    }
}