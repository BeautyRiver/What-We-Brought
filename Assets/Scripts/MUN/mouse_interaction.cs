using UnityEngine;

public class mouse_interaction:
MonoBehaviour
{
    public Texture2D cursorTexture;

    public Vector2 hotSpot = Vector2.zero;
    void OnMouseEnter()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto); 
        //이미지 커서로 변경
    }
    void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        //이미지 끝 -> 원래 커서로
    }

    void OnMouseDown()
    {
        Debug.Log("오브젝트!");
    }
}

