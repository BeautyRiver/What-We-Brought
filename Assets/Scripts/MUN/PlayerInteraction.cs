using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("설정")]
    [Tooltip("상호작용이 가능한 최대 거리")]
    public float interactionRange = 3.0f;

    [Tooltip("상호작용할 물체가 있는 레이어 (최적화용)")]
    public LayerMask interactableLayer;

    [Header("커서 UI (선택)")]
    public Texture2D interactionCursor; // 손 모양 커서 등
    private Texture2D defaultCursor;    // 기본 커서 저장용

    private void Update()
    {
        HandleInteraction();
    }

    void HandleInteraction()
    {
        // 1. 카메라에서 마우스 커서 위치로 보이지 않는 광선(Ray)을 쏩니다.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // 2. 광선이 'interactableLayer'에 속한 물체에 닿았는지 확인
        if (Physics.Raycast(ray, out hit, 100f, interactableLayer))
        {
            // 3. 플레이어와 그 물체 사이의 거리를 잽니다.
            float distance = Vector3.Distance(transform.position, hit.transform.position);

            // 4. 거리가 사정거리(interactionRange) 안쪽인지 확인
            if (distance <= interactionRange)
            {
                // 5. 그 물체에 IInteractable 스크립트가 있는지 확인
                IInteractable interactable = hit.transform.GetComponent<IInteractable>();

                if (interactable != null)
                {
                    // [커서 변경] 상호작용 가능하므로 커서 바꿈
                    Cursor.SetCursor(interactionCursor, Vector2.zero, CursorMode.Auto);

                    // [클릭 처리] 마우스 왼쪽 버튼 클릭 시
                    if (Input.GetMouseButtonDown(0))
                    {
                        interactable.Interact();
                    }
                    return; // 상호작용 중이면 여기서 함수 종료
                }
            }
        }

        // 아무것도 안 닿았거나 거리가 멀면 커서 원상복구
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    // 디버그용: 씬 뷰에서 플레이어 주변 상호작용 범위를 눈으로 보여줌
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}