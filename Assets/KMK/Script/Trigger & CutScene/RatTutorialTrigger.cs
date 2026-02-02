using UnityEngine;

public class RatTutorialTrigger : TutorialTrigger
{
    [Header("시각 효과")]
    public Highlightable questMouseHole;
    public Material outlineEffectMaterial;

    protected override void StartEvent()
    {
        base.StartEvent();
        UIManager.instance.ShowInfoPanel(content);
        questMouseHole.Highlight(outlineEffectMaterial);
    }

    protected override void EndEvent()
    {
        base.EndEvent();
        UIManager.instance.HideInfoPanel();
        questMouseHole.Unhighlight();
    }
}
