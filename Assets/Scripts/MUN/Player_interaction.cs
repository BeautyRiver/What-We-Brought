
using UnityEngine;

public class Player_Interaction : MonoBehaviour
{
    // 'Interactable' 대신 새 부모 이름 'interaction'을 사용
    private interaction currentInteractable = null;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 'interaction'을 상속받은 스크립트(NPC, Object)가 있는지 찾음
        interaction interactableScript = other.GetComponent<interaction>();

        if (interactableScript != null)
        {
            Debug.Log(other.name + " 범위 진입");
            interactableScript.isPlayerInRange = true;
            currentInteractable = interactableScript;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // 'interaction'을 상속받았는지 확인
        if (other.GetComponent<interaction>() == currentInteractable)
        {
            if (currentInteractable != null) // 안전 장치
            {
                Debug.Log(currentInteractable.name + " 범위 이탈");
                currentInteractable.isPlayerInRange = false;
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                currentInteractable = null;
            }
        }
    }
}