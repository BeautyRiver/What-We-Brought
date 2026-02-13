using UnityEngine;

public class MarketEventTrigger : EventTrigger
{
    [Header("엿듣기 대화 내용")]
    [TextArea] public string dialogueContent; // 인스펙터에서 대사 입력

    // 플레이어가 통 앞에 도착하면(Trigger Enter) 부모가 이 함수를 실행시켜 줍니다.
    protected override void StartEvent()
    {
        Debug.Log("시장 이벤트 발생: 엿듣기 시작");

        // [기능 구현] 튜토리얼 때처럼 UI 매니저를 불러서 대사를 띄웁니다.
        // (나중에 대화창 시스템이 따로 생기면 그 함수로 바꾸면 됩니다)
        if (UIManager.instance != null)
        {
            UIManager.instance.ShowInfoPanel(dialogueContent);
        }
    }

    // 플레이어가 통 앞을 떠나면(Trigger Exit) 실행됩니다.
    protected override void EndEvent()
    {
        Debug.Log("엿듣기 종료");

        // 대화창 끄기
        if (UIManager.instance != null)
        {
            UIManager.instance.HideInfoPanel();
        }
    }
}