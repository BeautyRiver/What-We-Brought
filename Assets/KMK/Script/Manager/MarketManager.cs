using UnityEngine;

public class MarketManager : MonoBehaviour
{
    public static MarketManager instance;

    [Header("퀘스트 ID 카드")]
    public QuestData mainQuest; 

    [Header("필요한 증거물들")]
    public Item targetNoteItem; // 쪽지 아이템
    public int targetEavesdropCount = 3; // 목표 엿듣기 횟수

    [Header("현재 상황")]
    public int currentEavesdropCount = 0;

    [Header("연결된 문")]
    public GameObject gateObstacle;
    public GameObject nextSceneTrigger;

    private void Awake() { instance = this; }

    private void Start()
    {
        // 이미 깼는지 ID로 확인
        if (QuestManager.instance.IsQuestCleared(mainQuest.questID))
        {
            OpenGateInstant();
        }
    }

    // 창문 엿듣기 완료 시 호출
    public void AddEavesdropProgress()
    {
        currentEavesdropCount++;
        CheckClearCondition();
    }

    // ⭐ 조건 검사 (여기에 복잡한 로직을 마음대로 넣으면 됨)
    public void CheckClearCondition()
    {
        // 조건 1: 쪽지 아이템 보유 여부
        bool hasNote = Inventory.instance.items.Contains(targetNoteItem);

        // 조건 2: 엿듣기 횟수 충족 여부
        bool isEavesdropDone = currentEavesdropCount >= targetEavesdropCount;

        // 최종 판결
        if (hasNote && isEavesdropDone)
        {
            CompleteQuest();
        }
        else
        {
            // 아직 덜 깼으면 힌트 로그
            Debug.Log($"진행 중... 쪽지:{hasNote}, 엿듣기:{currentEavesdropCount}/{targetEavesdropCount}");
        }
    }

    private void CompleteQuest()
    {
        Debug.Log("퀘스트 조건 달성!");

        // 1. 전역 매니저에 ID 등록
        QuestManager.instance.RegisterQuestClear(mainQuest.questID);

        // 2. 문 열기
        if (gateObstacle != null) gateObstacle.SetActive(false);
        if (nextSceneTrigger != null) nextSceneTrigger.SetActive(true);

        UIManager.instance.ShowInfoPanel("빈민가 통로가 열렸습니다.");
    }

    private void OpenGateInstant()
    {
        if (gateObstacle != null) gateObstacle.SetActive(false);
        if (nextSceneTrigger != null) nextSceneTrigger.SetActive(true);
    }
}