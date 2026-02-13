using UnityEngine;
using UnityEngine.Events; // ⭐ 이게 필요합니다!

public class FieldScript : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueData dialogueData;
    public bool isDestory = false;

    // 인스펙터에서 함수를 끌어다 놓을 수 있는 이벤트
    [Header("대화 종료 후 실행할 이벤트")]
    public UnityEvent onEndDialogue;

    public void Interact()
    {
        // 대화 시작 (끝나면 EndCutscene 실행)
        DialogueManager.instance.StartDialogue(dialogueData, EndCutscene);
    }

    public void EndCutscene()
    {
        // 여기에 연결된 이벤트들을 싹 실행함
        onEndDialogue?.Invoke();

        // 파괴/비활성화 로직 (대화가 다 끝나고 사라지는게 더 자연스러움)
        if (isDestory)
            Destroy(gameObject);
        else
            this.GetComponent<Collider>().enabled = false;
    }
}