using UnityEngine;

// 3D에서는 SpriteRenderer 대신 Renderer(MeshRenderer 등)를 사용합니다.
[RequireComponent(typeof(Renderer))]
public class Highlightable : MonoBehaviour
{
    private Renderer myRenderer;
    private Color originalColor;

    void Awake()
    {
        // 3D 렌더러 컴포넌트 가져오기
        myRenderer = GetComponent<Renderer>();

        // 원래 색상 저장 (Material의 색상)
        originalColor = myRenderer.material.color;
    }

    public void Highlight(Color highlightColor)
    {
        // 3D 재질 색상 변경
        myRenderer.material.color = highlightColor;
    }

    public void Unhighlight()
    {
        // 원래 색 복구
        myRenderer.material.color = originalColor;
    }
}