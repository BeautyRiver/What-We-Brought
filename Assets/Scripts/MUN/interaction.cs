using UnityEngine;

public class Interaction : MonoBehaviour
{
    
    public Texture2D cursorTexture; // 인스팩터 창에서 커서 이미지 받기위해 
    public Vector2 hotSpot = Vector2.zero; // 클릭 좌표 설정

 
    private bool isPlayerInRange = false; // 플레이어가 범위 안에 있니? 처음은 false로 설정

   
    private IInteractable interactable; // 클릭 감지하고 IInteractable의 interact 함수를 호출하면
                                        // npc나 오브젝트 스크립트가 호출받아서 기능을 실현 하기 위한 코드

    private void Awake() // Npc나 BasicObject 스크립트가 잘 붙어있나 확인하기 위한 코드 
    {

        interactable = GetComponent<IInteractable>(); 

        if (interactable == null)
        {
            Debug.LogWarning(gameObject.name + "에 IInteractable 컴포넌트가 없습니다");
        }
    }

    private void OnTriggerEnter2D(Collider2D other) // 다른 콜라이더 들어왔을때 호출
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log(gameObject.name + ": 플레이어가 범위에 들어옴");
        }
    }

    private void OnTriggerExit2D(Collider2D other) // 나갔을때 
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log(gameObject.name + ": 플레이어가 범위에서 나감");

            ResetCursor();
        }
    }


    void OnMouseEnter() // 마우스 커서 올라갔을때 
    {

        if (isPlayerInRange)
        {
            Debug.Log("마우스가 " + gameObject.name + "에 들어옴");
            SetCursor();
        }
    }

    void OnMouseExit() // 마우스 커서 나갔을때
    {
        ResetCursor();
    }

    void OnMouseDown() // 마우스로 클릭했을때 
    {
        if (isPlayerInRange)
        {
            Debug.Log("마우스가 " + gameObject.name + "를 클릭함");

            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }

    private void SetCursor() 
    {
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
    }

    private void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}