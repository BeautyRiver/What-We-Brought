using UnityEngine;

public class Interaction : MonoBehaviour
{
    [Header("커서 설정")]
    public Texture2D cursorTexture; // 인스펙터 창에서 커서 이미지 할당
    public Vector2 hotSpot = Vector2.zero; // 커서 클릭 지점 설정

    private bool isPlayerInRange = false; // 플레이어가 범위 안에 있는지 여부
    private IInteractable interactable; // 상호작용 인터페이스

    private void Awake()
    {
        // 같은 오브젝트에 있는 IInteractable 구현체(BasicObject 등)를 가져옴
        interactable = GetComponent<IInteractable>();

        if (interactable == null)
        {
            Debug.LogWarning(gameObject.name + "에 IInteractable 컴포넌트가 없습니다!");
        }
    }

    // [3D 변경 포인트] 2D가 빠지고 Collider 타입을 사용합니다.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log(gameObject.name + ": 플레이어가 범위에 들어옴");
        }
    }

    // [3D 변경 포인트] 2D가 빠지고 Collider 타입을 사용합니다.
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log(gameObject.name + ": 플레이어가 범위에서 나감");

            ResetCursor(); // 범위 밖으로 나가면 커서 초기화
        }
    }

    // 3D 오브젝트에 Collider가 붙어있으면 이 함수들은 그대로 작동합니다.
    void OnMouseEnter()
    {
        // 플레이어가 근처에 있을 때만 커서 변경
        if (isPlayerInRange)
        {
            SetCursor();
        }
    }

    void OnMouseExit()
    {
        ResetCursor();
    }

    void OnMouseDown()
    {
        // 플레이어가 근처에 있고 + 마우스로 클릭했을 때 실행
        if (isPlayerInRange)
        {
            Debug.Log("마우스가 " + gameObject.name + "를 클릭함");

            if (interactable != null)
            {
                interactable.Interact(); // 인터페이스 함수 호출
            }
        }
    }

    private void SetCursor()
    {
        if (cursorTexture != null)
        {
            // 3D에서도 동일하게 작동합니다.
            Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
        }
    }

    private void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}