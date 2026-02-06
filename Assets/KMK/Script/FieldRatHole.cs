using DG.Tweening;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class FieldRatHole : MonoBehaviour, IInteractable
{
    [Header("퍼즐 연결")]
    [SerializeField] private RatPuzzle ratPuzzle;
    public string PuzzleID => ratPuzzle.PuzzleID;

    [Header("클리어 후 설정")]
    [Tooltip("클리어 후 상호작용 시 띄울 텍스트")]
    private string toolTipContent = "더이상 들어갈 필요가 없을것 같다..";
    [SerializeField] private GameObject rewardItem;

    private void Start()
    {
        ratPuzzle.SetShowObj(rewardItem);
        CheckPuzzleState();
    }

    private void CheckPuzzleState()
    {
        // 이미 깬 퍼즐인가?
        if (PuzzleGameManager.instance.IsPuzzleCleared(PuzzleID))
        {
            // 보상 아이템 처리:
            // 보상 아이템이 있고 + 아직 안 먹었다면(SaveableObject가 켜져있다면) -> 보이게 켜줌
            if (rewardItem != null)
            {
                // SaveableObject가 스스로 꺼지는 로직(Start)이 있으므로, 
                // 여기선 일단 켜주기만 하면 됨 (먹었으면 알아서 꺼짐)
                rewardItem.SetActive(true);
            }
        }
        else
        {
            // 아직 안 깼으면 보상 아이템 숨김
            if (rewardItem != null) rewardItem.SetActive(false);
        }
    }
    public void Interact()
    {
        if (!PuzzleGameManager.instance.IsPuzzleCleared(PuzzleID))         
            ratPuzzle.StartSwitchCamera(true);
        else
        {
            // 깼으면 툴팁만 띄움 (잠김)
            UIManager.instance.ShowTooltip(toolTipContent, transform, 2f);
        }
    }    
}
