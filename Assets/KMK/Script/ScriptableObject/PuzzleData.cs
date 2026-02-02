using UnityEngine;

[CreateAssetMenu(fileName = "New Puzzle Data", menuName = "Puzzle/Puzzle Data")]
public class PuzzleData : ScriptableObject
{
    [Header("고유 ID (파일 이름과 동일하게 관리 추천)")]
    public string puzzleID;

    [Header("퍼즐 정보 (나중에 확장 가능)")]
    public string displayName; // 게임 내 표시될 이름
    [TextArea] public string description; // 설명    
}
