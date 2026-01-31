using System;
using TMPro;
using UnityEngine;

public class NotePanel : MonoBehaviour
{
    [Header("UI 요소 연결")]
    public TextMeshProUGUI noteText;

    public void OpenPanel(string content)
    {
        GameManager.instance.SetGameState(GameState.Dialogue);
        noteText.text = content;
    }

    public void ClosePanel()
    {
        GameManager.instance.SetGameState(GameState.Playing);
        gameObject.SetActive(false);
    }
}
