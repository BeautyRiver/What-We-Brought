using UnityEngine;

public class RatTutorialTrigger : TutorialTrigger
{
    [Header("시각 효과")]
    public Highlightable questMouseHole;
    public Material outlineEffectMaterial;

    protected override void StartEvent()
    {
        base.StartEvent();
        TutorialManager.Instance.ShowTutorial(myMessage);
        questMouseHole.Highlight(outlineEffectMaterial);
    }

    protected override void EndEvent()
    {
        base.EndEvent();
        TutorialManager.Instance.HideTutorial();
        questMouseHole.Unhighlight();
    }
}
