using UnityEngine;
using UnityEngine.EventSystems; // ⭐ UI 감지용

public class InteractiveObject : MonoBehaviour
{
    private Vector2 hotSpot = Vector3.zero;
    private bool isPlayerInRange = false;
    private IInteractable interactable;
    private SpriteRenderer targetSprite; // ⭐ 매번 찾지 않고 기억해두기

    private void Awake()
    {
        interactable = GetComponent<IInteractable>();
        targetSprite = GetComponentInChildren<SpriteRenderer>();
    }
  
    // 마우스 이벤트
    void OnMouseEnter()
    {
        // 1. UI(인벤토리 등)가 가리고 있으면 커서 바꾸지 마! (⭐ 핵심)
        if (EventSystem.current.IsPointerOverGameObject()) return;

        // 2. 범위 밖이면 무시
        if (!isPlayerInRange) return;

        // 3. 그림이 없거나 꺼져있으면 무시
        if (targetSprite == null || targetSprite.enabled == false) return;

        SetCursor();
    }

    void OnMouseExit()
    {
        ResetCursor();
    }

    void OnMouseDown()
    {
        // 1. UI 클릭 중이면 게임 상호작용 금지 (⭐ 핵심)
        if (EventSystem.current.IsPointerOverGameObject()) return;

        // 2. 그림 검사
        if (targetSprite == null || targetSprite.enabled == false) return;

        // 3. 상호작용
        if (isPlayerInRange)
        {
            if (interactable != null) interactable.Interact();
        }
        else
        {
            // (선택) "너무 멉니다" 메시지 띄우기
            Debug.Log("거리가 너무 멉니다.");
        }
    }

    private void SetCursor()
    {
        Texture2D cursorTexture = UIManager.instance.defaultInteractCursor;
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
    }

    private void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    // 오브젝트가 비활성화(Disable) 될 때 커서 초기화 (안전장치)
    private void OnDisable()
    {
        ResetCursor();
    }
}