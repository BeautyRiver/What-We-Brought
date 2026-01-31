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
            // ⭐ UIManager를 통해 상태 확인 및 명령
            if (UIManager.instance.IsTalkPanelTyping)
            {
                UIManager.instance.CompleteTalkText();
            }
            else
            {
                DisplayNextSentence();
            }
        }
    }

    public void StartDialogue(DialogueData data, Action endCallback = null)
    {
        isDialogueActive = false;
        onDialogueEnd = endCallback;
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
        yield return null; // 한 프레임 대기
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

        // ⭐ UI 매니저야, 텍스트 좀 갱신해줘!
        UIManager.instance.UpdateTalkText(dialogueLine.speakerName, dialogueLine.text);
    }

    public void EndDialogue()
    {
        isDialogueActive = false;

        // ⭐ UI 매니저야, 대화창 닫아줘!
        UIManager.instance.HideTalkPanel();

        if (onDialogueEnd != null)
        {
            onDialogueEnd.Invoke();
            onDialogueEnd = null;
        }
    }
}