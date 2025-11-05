using UnityEngine;

public class Npc : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("NPC와 상호작용 발생!");
    }
}
