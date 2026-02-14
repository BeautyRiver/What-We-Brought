using UnityEngine;
using VInspector;

// 모든 NPC의 조상님 (기본 기능)
public class Npc : MonoBehaviour, IInteractable
{
    [Header("💬 대화 설정 (기본)")]
    public bool isInteractable = true;  // 대화 가능 여부 체크박스

    [ShowIf("isInteractable")]
    public DialogueData dialogueData;   // 대화 내용

    // 자식들이 쓸 수 있게 protected 가상 함수로 만듦
    public virtual void Interact()
    {
        // 1. 대화 불가능 상태면 무시
        if (!isInteractable)
        {
            // (예: "지금은 바빠 보입니다." 띄우기)
            Debug.Log("대화할 수 없는 상태입니다.");
            return;
        }

        // 2. 대화 시작
        if (dialogueData != null)
        {
            // 대화 매니저 호출 (대화 끝날 때 실행할 함수도 같이 전달)
            DialogueManager.instance.StartDialogue(dialogueData, OnDialogueEnd);

            // 대화 시작됐을 때 자식들에게 알림 (멈추게 하거나 쳐다보게 하기 위함)
            OnDialogueStart();
        }
    }

    // 자식 클래스에서 "대화 시작/종료" 시점에 뭔가 하고 싶다면 이걸 오버라이드 하면 됨
    protected virtual void OnDialogueStart() { }
    protected virtual void OnDialogueEnd() { }
}