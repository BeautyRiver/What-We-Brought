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
    public PausePanel pausePanel;
    public CharacterStatePanel charStatePanel;
    public GameObject itemPanel;
    public CrowCoolTime crowCoolTimeUI;

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

    // ------------------ Info UI --------------------------
    public void ShowInfoPanel(string content)
    {
        infoPanel.ShowInfo(content);
    }

    public void ShowInfoPanel(string content, float showTime)
    {
        infoPanel.ShowInfo(content, showTime);
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
    public void ShowTooltip(string content, Transform target)
    {
        if (tooltipPanel != null)
            tooltipPanel.Show(content, target, 0f); // 0f = 안 사라짐
    }

    // 특정 시간 동안만 보여줌 (클릭 알림 등)
    public void ShowTooltip(string content, Transform target, float duration)
    {
        if (tooltipPanel != null)
            tooltipPanel.Show(content, target, duration);
    }

    public void HideTooltip()
    {
        if (tooltipPanel != null)
            tooltipPanel.Hide();
    }
    // ------------------ Pause UI --------------------------
    public void ShowPausePanel()
    {
        pausePanel.ShowPausePanel();
    }
    public void HidePausePanel()
    {
        pausePanel.HidePausePanel();
    }

    // ------------------ 현재 캐릭터 상태 UI --------------------------
    public void ChangeCharStateSprite(string name)
    {
        charStatePanel.ChangSprite(name);
    }


    // ------------------ 까마귀 쿨타임 UI --------------------------
    public void StartCrowCooldownUI(float activeTime, float coolTime)
    {
        if (crowCoolTimeUI != null)
        {
            crowCoolTimeUI.StartCooldown(activeTime, coolTime);
        }
    }


}
