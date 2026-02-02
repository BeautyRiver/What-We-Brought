using UnityEngine;

// [핵심] EventTrigger 상속 (부모 클래스의 기능을 물려받음)
public class GuardTutorialTrigger : EventTrigger
{
    [Header("경비병 안내 문구")]
    [TextArea]
    public string dialogueContent = "이 구역은 경비병이 존재합니다. 경비병은 쥐도 경계합니다."; // 기본 문구 설정

    // 플레이어가 영역에 들어오면(Trigger Enter) 실행
    protected override void StartEvent()
    {
        Debug.Log("경비병 튜토리얼 팝업");

        if (UIManager.instance != null)
        {
            UIManager.instance.ShowInfoPanel(dialogueContent);
        }
    }

    // 플레이어가 영역에서 나가면(Trigger Exit) 실행
    protected override void EndEvent()
    {
        Debug.Log("경비병 튜토리얼 종료");

        if (UIManager.instance != null)
        {
            UIManager.instance.HideInfoPanel();
        }
    }
}