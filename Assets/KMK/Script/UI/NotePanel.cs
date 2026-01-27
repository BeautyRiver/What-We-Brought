using System;
using TMPro;
using UnityEngine;

public class NotePanel : MonoBehaviour
{
    [Header("UI 요소 연결")]
    public TextMeshProUGUI noteText;

    private void Awake()
    {
        gameObject.SetActive(false);
    }
    public void OpenPanel(string content)
    {
        GameManager.instance.SetPlayerMoveState(false);
        noteText.text = content;
    }

    public void ClosePanel()
    {
        GameManager.instance.SetPlayerMoveState(true);
        gameObject.SetActive(false);
    }
}
