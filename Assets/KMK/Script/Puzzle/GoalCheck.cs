using UnityEngine;

public abstract class GoalCheck : MonoBehaviour
{
    [SerializeField] protected FieldRatHole fieldRatHole;
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Goal"))
        {            
            this.gameObject.SetActive(false);
            gameObject.GetComponentInParent<RatPuzzle>().StartSwitchCamera(false);
            GoalEvent();
        }
    }

    protected abstract void GoalEvent();    
}
