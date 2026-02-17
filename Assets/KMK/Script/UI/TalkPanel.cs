using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class TalkPanel : MonoBehaviour
{
    [Header("UI 요소 연결")]
    public TextMeshProUGUI talkText;
    public TextMeshProUGUI nameText;
    private RectTransform talkPanelRect;

    [SerializeField] private float talkTextInterval;
    [SerializeField] private float talkPanelUpYPos;
    [SerializeField] private float talkPanelDownYPos;

    public bool IsTyping { get; private set; }
    private string currentFullText; 

    private void Awake()
    {
        talkPanelRect = gameObject.GetComponent<RectTransform>();
        talkPanelRect.transform.position = new Vector3(talkPanelRect.transform.position.x, talkPanelDownYPos, talkPanelRect.transform.position.z);
    }
    public void OpenPanel()
    {
        gameObject.SetActive(true);

        if (talkPanelRect != null)
        talkPanelRect.DOAnchorPos(new Vector2(0, talkPanelUpYPos), 0.2f).SetEase(Ease.OutQuad);

        talkText.text = "";
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
        talkText.text = "";
        talkPanelRect.DOAnchorPos(new Vector2(0, talkPanelDownYPos), 0.2f).SetEase(Ease.InQuad);
    }

    public void SetText(string name, string content)
    {
        talkText.DOKill();

        nameText.text = name;
        talkText.text = "";
        currentFullText = content;
        IsTyping = true;

        float totalInterval = talkTextInterval * content.Length;

        talkText.DOText(content, totalInterval).SetEase(Ease.Linear).OnComplete(() => IsTyping = false);
    }

    public void CompleteText()
    {
        talkText.DOKill();
        talkText.text = currentFullText;
        IsTyping = false;
    }
}
