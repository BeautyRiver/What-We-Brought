using UnityEngine;

public class TutorialTrigger : EventTrigger
{
    [Header("이 구역 멘트")]
    [TextArea] public string myMessage;

    protected override void StartEvent()
    {
        TutorialManager.Instance.ShowTutorial(myMessage);
    }

    protected override void EndEvent()
    {
        TutorialManager.Instance.HideTutorial();
    }
}