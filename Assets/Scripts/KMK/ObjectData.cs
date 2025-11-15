using UnityEngine;
using UnityEngine.Playables;
using VInspector;

[System.Serializable]
public struct DialogueEntry
{
    public Speaker speaker;
    [TextArea(3, 5)]
    public string dialogueText;

    // 나중에 스프라이트 이미지 필요하면 주석 해제
    //public Sprite sprite;
}

public enum Speaker
{
    Player,
    Npc
}

[System.Serializable]
public struct DialogueContent
{
    // 나중에 스프라이트 이미지 필요하면 주석 해제
    //public Sprite defaultNpcSprite;
    //public Sprite defaultPlayerSprite;
    public DialogueEntry[] dialogues;
}

[CreateAssetMenu(fileName = "ObjectData", menuName = "Scriptable Objects/ObjectData")]
public class ObjectData : ScriptableObject
{
    public int id;
    public string objectName;
    public bool isNpc;
    
    public DialogueContent dialogue;    
}
