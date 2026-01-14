using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [Header("ÀÌ ±¸¿ª ¸àÆ®")]
    [TextArea]
    public string myMessage;

    private void OnTriggerEnter(Collider other)
    {        
        if (other.CompareTag("Player"))
        {
            Debug.Log(this.gameObject.name + " IN");

            TutorialManager.Instance.ShowTutorial(myMessage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(this.gameObject.name + " OUT");

            TutorialManager.Instance.HideTutorial();
            
            //gameObject.SetActive(false); 
        }
    }
}