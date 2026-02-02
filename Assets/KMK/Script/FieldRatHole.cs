using DG.Tweening;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class FieldRatHole : MonoBehaviour, IInteractable
{
    public string PuzzleID => ratPuzzle.PuzzleID;

    [SerializeField] private string toolTipContent;
    public RatPuzzle ratPuzzle;
    public GameObject showObject;

    public void Interact()
    {
        if (!PuzzleGameManager.instance.IsPuzzleCleared(PuzzleID))         
            ratPuzzle.StartSwitchCamera(true);
        else
        {
            UIManager.instance.ShowTooltip(toolTipContent, 2f);
        }
    }    
    

    public void ShowObejct()
    {
        showObject.gameObject.SetActive(true);
    }
}
