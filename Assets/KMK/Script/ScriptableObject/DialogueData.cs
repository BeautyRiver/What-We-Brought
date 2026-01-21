using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DialogueLine
{
    public string speakerName; // 화자 이름
    [TextArea(3, 5)] public string text; // 대사 내용
}

[CreateAssetMenu(fileName = "DialogueData", menuName = "Quest System/DialogueData")]
public class DialogueData : ScriptableObject
{
    
    public List<DialogueLine> textLines; // 대사 리스트
}
