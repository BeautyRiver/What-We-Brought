using UnityEngine;

public class Highlightable : MonoBehaviour
{
    [Header("설정")]
    [Tooltip("체크하면 게임 시작 시 투명하게 숨겨집니다.")]
    public bool isHiddenByDefault = true; // 기본값을 true로 설정
    public bool IsVisible => myRenderer != null && myRenderer.enabled;

    private SpriteRenderer myRenderer;
    private Material originalMaterial;
    private bool isHighlighted = false;

    void Awake()
    {
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

    void Start()
    {
        // [수정 포인트] 시작할 때 설정에 따라 모습을 숨김
        if (isHiddenByDefault && myRenderer != null)
        {
            myRenderer.enabled = false; // 렌더러를 꺼서 안 보이게 만듦 (충돌체는 살아있음)
        }
    }

    // Crow.cs가 이 함수를 부릅니다
    public void Highlight(Material outlineMat)
    {
        if (myRenderer == null || isHighlighted) return;

        isHighlighted = true;

        // [수정 포인트] 능력이 발동되면 일단 보이게 켬!
        myRenderer.enabled = true;

        // 1. 텍스처 보존
        Texture currentTexture = myRenderer.sprite.texture;

        // 2. 아웃라인 재질로 교체
        myRenderer.material = outlineMat;

        // 3. 텍스처 재연결
        if (myRenderer.material.HasProperty("_MainTex"))
        {
            myRenderer.material.SetTexture("_MainTex", currentTexture);
        }     
    }

    // Crow.cs가 능력이 끝나면 이 함수를 부릅니다
    public void Unhighlight()
    {
        if (myRenderer == null || !isHighlighted) return;

        isHighlighted = false;

        // [수정 포인트] 원래 숨겨진 녀석이었다면 다시 숨김
        if (isHiddenByDefault)
        {
            myRenderer.enabled = false; // 다시 안 보이게 끄기
            // 숨겨졌으니 재질 복구는 굳이 안 해도 되지만, 깔끔하게 원래대로
            myRenderer.material = originalMaterial;
        }
        else
        {
            // 원래 보이던 녀석이면 재질만 원래대로 복구
            myRenderer.material = originalMaterial;
        }
    }
}