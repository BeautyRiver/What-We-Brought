using UnityEngine;

public class Interaction : MonoBehaviour
{
    public Texture2D cursorTexture;
    public Vector2 hotSpot = Vector2.zero;
    public bool isPlayerInRange = false;

    void OnMouseEnter()
    {
        if (isPlayerInRange)
        {
            Debug.Log("마우스가 " + gameObject.name + "에 들어옴!");
            Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
        }
 
    }
    void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }


    void OnMouseDown()
    {
        if (isPlayerInRange)
        {
            Debug.Log("마우스가 " + gameObject.name + "를 클릭함!");
            OnInteract(); //마우스를 클릭하면 발생되는것으로 오브젝트랑 NPC의 상호작용을 나누기 위해서 존재 -> 자녀스크립트에서 코드를 칠 예정
        }
    }

    public void OnInteract()
    {
        Debug.Log("상호작용 발생!");
    }
}

