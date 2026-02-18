using UnityEngine;

public class TutorialClearTrigger : EventTrigger
{
    protected override void StartEvent(Collider other)
    {
        if (DataManager.instance.currentData.isTutorialCleared)
            return;

        DataManager.instance.SetTutorialCleared();
    }
}
