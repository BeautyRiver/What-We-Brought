using UnityEngine;

public class CutsceneTrigger : EventTrigger
{
    [Header("¿¬°áÇÒ ÄÆ½Å")]

    public CutsceneDirector targetCutscene;

    protected override void StartEvent()
    {
        if (targetCutscene != null)
        {
            targetCutscene.PlayCutscene();
            gameObject.SetActive(false);
        }
    }
}
