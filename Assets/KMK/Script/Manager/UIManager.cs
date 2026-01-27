using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using VInspector;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("UI 연결")]
    public TalkPanel talkPanel;
    public NotePanel notePanel;

    [Header("기본 커서 설정")]
    public Texture2D defaultInteractCursor;
   
    public bool IsTalkPanelTyping => talkPanel.IsTyping; // 현재 타이핑 중인지 확인 (Getter)

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    private void Start()
    {
        if (talkPanel != null) talkPanel.ClosePanel();
    }

    // ------------------ 대화창 UI --------------------------
    public void ShowTalkPanel()
    {
        // (확장성) 만약 인벤토리가 켜져 있다면 여기서 끌 수 있음
        // if (inventoryPanel.activeSelf) CloseInventory();

        talkPanel.OpenPanel();
    }

    public void HideTalkPanel()
    {
        talkPanel.ClosePanel();
    }

    public void UpdateTalkText(string name, string content)
    {
        talkPanel.SetText(name, content);
    }

    // 타이핑 스킵용
    public void CompleteTalkText()
    {
        talkPanel.CompleteText();
    }

    // ------------------ 쪽지 UI --------------------------
    public void ShowNotePanel(string content)
    {
        notePanel.gameObject.SetActive(true);
        notePanel.OpenPanel(content);
    }

    public void HideNotePanel()
    {
        notePanel.ClosePanel();
    }
}
