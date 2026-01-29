using UnityEngine;

public class GoalCheck : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Goal"))
        {            
            this.gameObject.SetActive(false);
            GameManager.instance.StartSwitchCamera(false);
        }
    }
   
}
