
using UnityEngine;

public class Player_Interaction : MonoBehaviour
{
    public float radius; //Inspector 창에서 범위 설정
    public LayerMask interactableLayer;
    private void Update() //상호작용 가능한 오브젝트, NPC를 계속 찾을 수 있도록
    {
        var interactableObj = Physics2D.OverlapCircleAll(transform.position, radius, interactableLayer);  // 상호작용 범위를 만들 원
        if (interactableObj.Length > 0)
        {
            foreach (var item in interactableObj)
            {
                item.GetComponent<Interaction>().isPlayerInRange = true; // 상호작용 가능한 오브젝트, NPC가 들어왔는가?
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; //빨간원
        Gizmos.DrawWireSphere(transform.position, radius);
    }   
}
