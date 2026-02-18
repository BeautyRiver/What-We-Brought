using UnityEngine;
using DarkTonic.MasterAudio;

public class BasicObject : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log(gameObject.name + "와 상호작용 성공!");

        MasterAudio.PlaySound3DAtTransform("ItemSound", transform);

    }
}