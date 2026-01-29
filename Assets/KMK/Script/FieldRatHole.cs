using DG.Tweening;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class FieldRatHole : MonoBehaviour, IInteractable
{    
    public void Interact()
    {
        GameManager.instance.StartSwitchCamera(true);
    }    
    
    
}
