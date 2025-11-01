using UnityEngine;

public abstract class interaction:MonoBehaviour
{
    public Texture2D cursorTexture;

    public Vector2 hotSpot = Vector2.zero;

    [HideInInspector]
    public bool isPlayerInRange = false;

    void OnMouseEnter()
    {
        if (isPlayerInRange)
        {
            Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);

        }
        //isPlayerInRange가 True일때 이미지 커서로 변경
 
    }
    void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        //이미지 끝 -> 원래 커서로
    }


    void OnMouseDown()
    {
        if (isPlayerInRange)
        {
            Debug.Log("마우스가 " + gameObject.name + "에서 나감!");
            OnInteract(); //마우스를 클릭하면 발생되는것으로 오브젝트랑 NPC의 상호작용을 나누기 위해서 존재 -> 자녀스크립트에서 코드를 칠 예정
        }
    }

    public abstract void OnInteract();
}

