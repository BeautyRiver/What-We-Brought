using UnityEngine;

[CreateAssetMenu(fileName = "StoryQuest", menuName = "Quest System/Story Quest")]
public class QuestData : ScriptableObject
{
    [Header("퀘스트 식별")]
    public string questID;      

    [Header("UI 표시용")]
    public string title;         // 예: "시장 정보 수집"
    [TextArea] public string description; // 예: "창문을 엿듣고 쪽지를 찾아라."
}