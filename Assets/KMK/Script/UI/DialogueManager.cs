using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // Action 사용을 위해

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public TalkPanel talkPanel;

    [Header("상태")]
    public bool isDialogueActive = false; // 대화 중인지?

    private Queue<DialogueLine> sentences = new Queue<DialogueLine>(); // 대사 저장소 (큐)
    private Action onDialogueEnd; // 대화 끝났을 때 실행할 행동 (콜백)

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (talkPanel != null) talkPanel.ClosePanel();
    }
    private void Update()
    {
        // 대화 중이 아니면 입력 무시
        if (!isDialogueActive) return;

        // 2. 대화창 상태일 때 E 또는 스페이스바로 다음 대화
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (talkPanel.IsTyping)
                talkPanel.CompeleteText();
            DisplayNextSentence();
        }
    }

    // 대화 시작 (NPC가 호출)
    public void StartDialogue(DialogueData data, Action endCallback = null)
    {
        // 일단 false로 시작 (입력 방지)
        isDialogueActive = false;

        onDialogueEnd = endCallback;
        sentences.Clear();

        if (data == null || data.textLines.Count == 0)
        {
            EndDialogue(); // 데이터 없으면 바로 종료
            return;
        }

        foreach (var line in data.textLines)
        {
            sentences.Enqueue(line);
        }

        talkPanel.OpenPanel();

        DisplayNextSentence();
        StartCoroutine(EnableInputRoutine());
    }

    // 한 프레임 대기하는 코루틴
    IEnumerator EnableInputRoutine()
    {
        yield return null;
        yield return null;

        // 이제부터 입력 허용
        isDialogueActive = true;
    }

    public void DisplayNextSentence()
    {
        // 더 이상 할 말이 없으면 종료
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        var dialogueLine = sentences.Dequeue(); // 큐에서 하나 꺼냄

        talkPanel.SetText(dialogueLine.speakerName, dialogueLine.text);
    }

    // 3. 마지막 대화 나오고 종료
    public void EndDialogue()
    {
        isDialogueActive = false;
        talkPanel.ClosePanel();

        // 콜백 실행 (NPC에게 "나 끝났어" 보고)
        if (onDialogueEnd != null)
        {
            onDialogueEnd.Invoke();
            onDialogueEnd = null;
        }
    }
}