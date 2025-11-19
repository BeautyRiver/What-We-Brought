using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using VInspector;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;
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

    [SerializeField] private GameObject talkPanel;
    [SerializeField] private float talkPanelUpYPos;
    [SerializeField] private float talkPanelDownYPos;

    [SerializeField] private TextMeshProUGUI talkText;
    private RectTransform talkPanelRect;

    [SerializeField] private float talkTextInterval;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        talkPanelRect = talkPanel.GetComponent<RectTransform>();
        talkPanelRect.anchoredPosition = new Vector2(0, talkPanelDownYPos);
        talkText.text = "";
    }

    public void ShowTalkPanel(string text)
    {
        talkText.text = "";

        // null이 아니면 NPC란 뜻 이미지 띄워줘야함      
        talkPanelRect.DOAnchorPos(new Vector2(0, talkPanelUpYPos), 0.2f).SetEase(Ease.OutQuad);
        float totalInterval = talkTextInterval * text.Length;
        talkText.DOText(text, totalInterval).SetEase(Ease.Linear);
    }

    public void HideTalkPanel()
    {
        talkText.text = "";
        talkPanelRect.DOAnchorPos(new Vector2(0, talkPanelDownYPos), 0.2f).SetEase(Ease.InQuad);
    }

    [SerializeField] private bool _debugbool;
    [Button]
    public void DebugTest(string text)
    {
        if (!_debugbool)
        {
            if (text == null)
                ShowTalkPanel("Debug Text Debug Text Debug Text Debug Text Debug Text " +
                    "Debug Text Debug Text Debug Text Debug Text Debug Text " +
                    "Debug Text Debug Text Debug Text Debug Text Debug Text " +
                    "Debug Text Debug Text Debug Text Debug Text Debug Text");
            else
                ShowTalkPanel(text);
        }
        else
        {
            HideTalkPanel();
        }

        _debugbool = !_debugbool;
    }
}
