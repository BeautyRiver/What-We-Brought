using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using VInspector;
using UnityEditor.Rendering.LookDev;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("UI 연결")]
    public TalkPanel talkPanel;
    public NotePanel notePanel;
    public InfoPanel infoPanel;
    public TooltipPanel tooltipPanel;
    public GameObject itemPanel;

    public CanvasGroup fadeCanvasGroup;

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
    
    // ------------------ Info UI --------------------------
    public void ShowInfoPanel(string content = null)
    {
        infoPanel.gameObject.SetActive(true);

        if (content != null)
            infoPanel.ShowInfo(content);
    }

    public void HideInfoPanel()
    {
        infoPanel.HideInfo();
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

    // ------------------ 아이템 UI --------------------------
    public void ShowItemPanel()
    {
        itemPanel.gameObject.SetActive(true);
    }
    public void HideItemPanel()
    {
        itemPanel.gameObject.SetActive(false);
    
    }
    // ------------------ 툴팁 UI --------------------------
    public void ShowTooltip(string content)
    {
        if (tooltipPanel != null)
            tooltipPanel.Show(content, 0f); // 0f = 안 사라짐
    }

    // 특정 시간 동안만 보여줌 (클릭 알림 등)
    public void ShowTooltip(string content, float duration)
    {
        if (tooltipPanel != null)
            tooltipPanel.Show(content, duration);
    }

    public void HideTooltip()
    {
        if (tooltipPanel != null)
            tooltipPanel.Hide();
    }

    // ------------------ 페이드 인 아웃 UI --------------------------
    public void FadeInOut(bool isFadeIn, float duration, Action onComplete = null)
    {
        float targetAlpha = isFadeIn ? 0f : 1f;

        fadeCanvasGroup.blocksRaycasts = true;

        fadeCanvasGroup.DOFade(targetAlpha, duration)
        .SetEase(Ease.Linear)
        .OnComplete(() =>
        {
            // 콜백
            onComplete?.Invoke();

            // 페이드 인 효과면 터치 가능하게 
            if (isFadeIn)
            {
                fadeCanvasGroup.blocksRaycasts = false;
            }
        });

    }

    
}
