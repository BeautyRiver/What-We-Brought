using UnityEngine;

public class CutsceneTrigger : EventTrigger
{
    [Header("¿¬°áÇÒ ÄÆ½Å")]

    public CutsceneDirector targetCutscene;

    protected override void StartEvent(Collider other)
    {
        if (targetCutscene != null)
        {
            targetCutscene.PlayCutscene();            
        }
    }

}
