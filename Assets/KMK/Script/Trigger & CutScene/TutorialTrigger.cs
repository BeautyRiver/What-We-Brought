using UnityEngine;

public class TutorialTrigger : EventTrigger
{
    [Header("이 구역 멘트")]
    [TextArea] public string content;

    protected override void StartEvent()
    {
        UIManager.instance.ShowInfoPanel(content);
    }

    protected override void EndEvent()
    {
        UIManager.instance.HideInfoPanel();
    }
}