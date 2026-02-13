using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [Header("상태")]
    public bool isDialogueActive = false;

    private Queue<DialogueLine> sentences = new Queue<DialogueLine>();
    private Action onDialogueEnd;

    // 대화 종료 후 자동으로 Playing 상태로 갈지 여부
    private bool autoUnlockState = true;

    private void Awake()
    {
        if (instance == null) { instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (!isDialogueActive) return;

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (UIManager.instance.IsTalkPanelTyping)
                UIManager.instance.CompleteTalkText();
            else
                DisplayNextSentence();
        }
    }

    public void StartDialogue(DialogueData data, Action endCallback = null, bool autoUnlock = true)
    {
        GameManager.instance.SetGameState(GameState.Dialogue);

        isDialogueActive = false;
        onDialogueEnd = endCallback;

        // 이번 대화의 설정을 저장
        this.autoUnlockState = autoUnlock;

        sentences.Clear();

        if (data == null || data.textLines.Count == 0)
        {
            EndDialogue();
            return;
        }

        foreach (var line in data.textLines) sentences.Enqueue(line);

        UIManager.instance.ShowTalkPanel();

        DisplayNextSentence();
        StartCoroutine(EnableInputRoutine());
    }

    IEnumerator EnableInputRoutine()
    {
        yield return null;
        isDialogueActive = true;
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        var dialogueLine = sentences.Dequeue();
        UIManager.instance.UpdateTalkText(dialogueLine.speakerName, dialogueLine.text);
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        UIManager.instance.HideTalkPanel();

        // 자동 잠금 해제 옵션이 켜져있을 때만 Playing으로 변경
        if (autoUnlockState)
        {
            if (GameManager.instance.CurrentState == GameState.Dialogue)
                GameManager.instance.SetGameState(GameState.Playing);
        }

        // 콜백 실행
        // (autoUnlockState가 false라면, 여기서 실행되는 콜백 함수가 나중에 직접 Playing으로 바꿔줘야 함)
        if (onDialogueEnd != null)
        {
            Action callback = onDialogueEnd;
            onDialogueEnd = null;
            callback.Invoke();
        }
    }
}