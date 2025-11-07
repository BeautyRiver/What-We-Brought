using UnityEngine;



public class BasicObject : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("오브젝트와 상호작용 발생!");
    }
}
