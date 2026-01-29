using UnityEngine;
using UnityEngine.EventSystems;

public class RatInteraction : MonoBehaviour
{
    [Header("설정")]
    [Tooltip("상호작용이 가능한 최대 거리")]
    public float interactionRange = 3.0f;

    [Tooltip("상호작용할 물체가 있는 레이어 (최적화용)")]
    public LayerMask interactableLayer;

    public void HandleInteraction()
    {
        // 1. UI 위에서는 끄기
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            // UI 위니까 고스트도 꺼야 함
            if (EquipmentManager.Instance.ghostUI != null)
                EquipmentManager.Instance.ghostUI.Hide();
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool isHitInteractable = false; // 상호작용 물체를 찾았는지 여부

        // 2. 레이캐스트 확인
        if (Physics.Raycast(ray, out hit, 100f, interactableLayer))
        {
            float distance = Vector3.Distance(transform.position, hit.transform.position);

            if (distance <= interactionRange)
            {
                Highlightable hiddenClue = hit.transform.GetComponent<Highlightable>();

                // 만약 단서인데(null이 아님) && 현재 안 보이는 상태(!IsVisible)라면?
                if (hiddenClue != null && !hiddenClue.IsVisible)
                {
                    // 아무것도 안 본 척하고 커서 초기화 후 리턴
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                    return;
                }

                IInteractable interactable = hit.transform.GetComponent<IInteractable>();

                // 상호작용 가능한 물체를 발견했다면?
                if (interactable != null)
                {
                    isHitInteractable = true;

                    // A. 아이템을 들고 있다면? -> 고스트 UI 띄우기
                    if (EquipmentManager.Instance.equippedItem != null)
                    {
                        EquipmentManager.Instance.ghostUI.Show();
                        // 커서는 기본 커서(null)나 투명 커서로 해서 고스트만 보이게 해도 좋음
                        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                    }
                    // B. 빈손이라면? -> 기존처럼 손바닥 커서
                    else
                    {
                        var interactionCursor = UIManager.instance.defaultInteractCursor;
                        Cursor.SetCursor(interactionCursor, Vector2.zero, CursorMode.Auto);
                    }

                    // 클릭 처리
                    if (Input.GetMouseButtonDown(0))
                    {
                        interactable.Interact();
                    }
                }
            }
        }

        // 3. 아무것도 안 닿았거나 상호작용 대상이 아니라면?
        if (!isHitInteractable)
        {
            // 커서 원상복구
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

            // ? 고스트 숨기기 (핵심!)
            // 아이템을 들고 있어도, 허공에서는 안 보여줌
            if (EquipmentManager.Instance.ghostUI != null)
            {
                EquipmentManager.Instance.ghostUI.Hide();
            }

            // 빈 허공 클릭 시 장착 해제 (이전 로직 유지)
            if (Input.GetMouseButtonDown(0) && EquipmentManager.Instance.equippedItem != null)
            {
                EquipmentManager.Instance.Unequip();
            }
        }
    }

    // 디버그용: 씬 뷰에서 플레이어 주변 상호작용 범위를 눈으로 보여줌
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}