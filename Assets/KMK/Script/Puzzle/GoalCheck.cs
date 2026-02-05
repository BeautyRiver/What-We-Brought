using UnityEngine;

public class GoalCheck : MonoBehaviour
{
    [SerializeField] private FieldRatHole fieldRatHole;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Goal"))
        {            
            this.gameObject.SetActive(false);
            gameObject.GetComponentInParent<RatPuzzle>().StartSwitchCamera(false);
            GoalEvent();
        }
    }

    public void GoalEvent()
    {
        fieldRatHole.ShowObejct();
        PuzzleGameManager.instance.ClearPuzzle(fieldRatHole.PuzzleID);
    }
}
