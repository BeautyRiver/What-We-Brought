using UnityEngine;

public class Interaction : MonoBehaviour
{
    [Header("커서 설정")]
    public Texture2D cursorTexture;
    public Vector2 hotSpot = Vector2.zero;

    private bool isPlayerInRange = false;
    private IInteractable interactable;

    private void Awake()
    {
        interactable = GetComponent<IInteractable>();
    }

    // [중요] 여기를 3D용(Collider)으로 다시 변경했습니다!
    private void OnTriggerEnter(Collider other)
    {
        // 디버그: 무엇과 부딪혔는지 확인
        Debug.Log("[3D 충돌 감지] 부딪힌 대상: " + other.name);

        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("플레이어 감지됨 (3D)");
        }
    }

    // [중요] 여기도 3D용으로 변경!
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("플레이어 나감 (3D)");
            ResetCursor();
        }
    }

    // 마우스 이벤트는 2D/3D 공용이므로 그대로 둠
    void OnMouseEnter()
    {
        if (isPlayerInRange) SetCursor();
    }

    void OnMouseExit()
    {
        ResetCursor();
    }

    void OnMouseDown()
    {
        Debug.Log("클릭함"); // 클릭 로그 확인

        if (isPlayerInRange)
        {
            if (interactable != null) interactable.Interact();
        }
        else
        {
            Debug.Log("거리가 너무 멈");
        }
    }

    private void SetCursor()
    {
        if (cursorTexture != null) Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
    }

    private void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}