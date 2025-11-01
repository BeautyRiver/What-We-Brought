
using UnityEngine;

public class Player_Interaction : MonoBehaviour
{
    public float radius;
    public LayerMask interactableLayer;
    private void Update()
    {
        var interactableObj = Physics2D.OverlapCircleAll(transform.position, radius, interactableLayer);
        if (interactableObj.Length > 0)
        {
            foreach (var item in interactableObj)
            {
                item.GetComponent<Interaction>().isPlayerInRange = true;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }   
}
