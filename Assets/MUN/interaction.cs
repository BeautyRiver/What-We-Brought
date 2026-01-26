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

    // [3D 충돌 감지]
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("플레이어 감지됨 (3D)");
            // 들어오자마자 마우스가 위에 있을 수 있으니 상태 갱신
            OnMouseEnter();
        }
    }

    // [3D 충돌 감지]
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("플레이어 나감 (3D)");
            ResetCursor();

            if (UIManager.instance != null)
            {
                UIManager.instance.HideTalkPanel();
            }
        }
    }

    // [수정 핵심 ⭐] 마우스 올렸을 때
    void OnMouseEnter()
    {
        // 1. 자식 오브젝트까지 뒤져서 그림(Sprite)을 찾아라!
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();

        // 2. 그림을 못 찾았다면? (빈 껍데기) -> 투명한 놈으로 취급해서 무시!
        if (sr == null) return;

        // 3. 그림은 있는데 눈을 감고 있다? (투명) -> 무시!
        if (sr.enabled == false) return;

        // 4. 다 통과했으면 커서 띄우기
        if (isPlayerInRange) SetCursor();
    }

    void OnMouseExit()
    {
        ResetCursor();
    }

    // [수정 핵심 ⭐] 마우스 클릭했을 때
    void OnMouseDown()
    {
        // 1. 자식까지 뒤져서 확인
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();

        // 2. 그림이 없거나(null), 꺼져있으면(false) -> 클릭 금지!
        if (sr == null || sr.enabled == false) return;

        Debug.Log("클릭함");

        // 3. 상호작용 실행
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