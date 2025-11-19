using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class Highlightable : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor; // 원래 색상을 저장할 변수

    void Awake()
    {   
        

        spriteRenderer = GetComponent<SpriteRenderer>();

        // 원래 색상 저장
        originalColor = spriteRenderer.color;
    }

    // Crow 스크립트가 호출 강조 함소
    public void Highlight(Color highlightColor)
    {
        spriteRenderer.color = highlightColor;
    }

    // Crow 스크립트가 호출할 강조 종료 함수
    public void Unhighlight()
    {
        // 원래색 복구
        spriteRenderer.color = originalColor;
    }
}