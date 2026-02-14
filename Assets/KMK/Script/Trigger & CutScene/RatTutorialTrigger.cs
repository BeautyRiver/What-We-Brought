using UnityEngine;

public class RatTutorialTrigger : TutorialTrigger
{
    [Header("시각 효과")]
    public Highlightable questMouseHole;
    public Material outlineEffectMaterial;

    protected override void StartEvent(Collider other)
    {
        base.StartEvent(other);
        questMouseHole.Highlight(outlineEffectMaterial);
    }

    protected override void EndEvent(Collider other)
    {
        base.EndEvent(other);
        questMouseHole.Unhighlight();
    }
}
