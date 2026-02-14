using UnityEngine;

public class GoalCheck : MonoBehaviour
{
    [SerializeField] private FieldRatHole fieldRatHole;
    private RatPuzzle ratPuzzle;

    private void Awake()
    {
        ratPuzzle = GetComponentInParent<RatPuzzle>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Goal"))
        {            
            this.gameObject.SetActive(false);
            ratPuzzle.StartSwitchCamera(false);
            GoalEvent();
        }
    }

    public void GoalEvent()
    {
        ratPuzzle.ShowObejct();
        PuzzleGameManager.instance.ClearPuzzle(fieldRatHole.PuzzleID);
    }
}
