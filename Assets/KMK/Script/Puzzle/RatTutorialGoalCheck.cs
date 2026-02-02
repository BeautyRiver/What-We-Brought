using UnityEngine;

public class RatTutorialGoalCheck : GoalCheck
{
    protected override void GoalEvent()
    {
        fieldRatHole.ShowObejct();
        PuzzleGameManager.instance.ClearPuzzle(fieldRatHole.PuzzleID);
    }
}
