using System.Collections;
using UnityEngine;

public class FieldTooltip : MonoBehaviour, IInteractable
{
    [SerializeField] private string content;
    public void Interact()
    {
        UIManager.instance.ShowTooltip(content, transform, 2f);
    }
    

}
