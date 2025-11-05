using UnityEngine;

public class GoalCheck : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Goal"))
        {
            Debug.Log("Goal Reached!");
            this.gameObject.SetActive(false);
        }
    }
   
}
