using UnityEngine;

public class Crow : MonoBehaviour
{
    public float cooldown = 30.0f;
    private float lastInteraction;

    private void Start()
    {
        lastInteraction = -cooldown;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(Time.time > cooldown + lastInteraction)
            {
                Debug.Log("±î¸¶±Í »ç¿ë");
            }
            else
            {
                Debug.Log("ÄðÅ¸ÀÓ");
            }
        }
    }
}

