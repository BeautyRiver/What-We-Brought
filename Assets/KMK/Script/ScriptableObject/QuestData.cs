using UnityEngine;

[CreateAssetMenu(fileName = "Quest Data", menuName = "Quest System/Quest Data")]
public class QuestData : ScriptableObject
{
    [Header("기본 정보")]
    public int questID;
    public string title;
    [TextArea] public string guideText;

    [Header("목표 설정")]
    public int targetEnemyID; // 허수아비의 ID (예: 100)
    public int targetCount;   // 목표 마리 수 (예: 5)

    [Header("대화")]
    public DialogueData startDialogue;
    public DialogueData progressDialogue;
    public DialogueData completeDialogue;
}