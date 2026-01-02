using UnityEngine;

public class Highlightable : MonoBehaviour
{
    private SpriteRenderer myRenderer; // Renderer -> SpriteRenderer로 변경
    private Material originalMaterial;
    private bool isHighlighted = false;

    void Awake()
    {
        // 2D 게임이므로 SpriteRenderer를 찾습니다.
        myRenderer = GetComponent<SpriteRenderer>();
        if (myRenderer == null) myRenderer = GetComponentInChildren<SpriteRenderer>();

        if (myRenderer != null)
        {
            originalMaterial = myRenderer.material;
        }
        else
        {
            Debug.LogError(gameObject.name + ": SpriteRenderer를 찾을 수 없습니다!");
        }
    }

    public void Highlight(Material outlineMat)
    {
        if (myRenderer == null || isHighlighted) return;

        isHighlighted = true;

        // 1. 현재 스프라이트 텍스처 저장 (스프라이트 아틀라스 대응)
        Texture currentTexture = myRenderer.sprite.texture;

        // 2. 재질 교체
        myRenderer.material = outlineMat;

        // 3. [중요] 쉐이더에 텍스처 다시 주입
        // URP 스프라이트 쉐이더의 경우 _MainTex를 주로 사용하지만, 
        // 사용하는 쉐이더 속성 이름이 다르면 맞춰줘야 합니다.
        if (myRenderer.material.HasProperty("_MainTex"))
        {
            myRenderer.material.SetTexture("_MainTex", currentTexture);
        }
    }

    public void Unhighlight()
    {
        if (myRenderer == null || !isHighlighted) return;

        isHighlighted = false;
        // 원래 재질로 복구
        myRenderer.material = originalMaterial;
    }
}