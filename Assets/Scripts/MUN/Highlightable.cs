using UnityEngine;

public class Highlightable : MonoBehaviour
{
    private SpriteRenderer myRenderer; // 2D 그림을 담당하는 렌더러
    private Material defaultMaterial;  // 원래 입고 있던 옷 (기본)

    void Awake()
    {
        // 내 몸통이나 자식들 중에서 SpriteRenderer를 찾습니다.
        myRenderer = GetComponentInChildren<SpriteRenderer>();

        if (myRenderer != null)
        {
            // 게임 시작할 때 입고 있던 원래 재질을 저장해둡니다.
            defaultMaterial = myRenderer.material;
        }
        else
        {
            Debug.LogError(gameObject.name + ": SpriteRenderer가 없습니다! 이미지가 제대로 들어갔는지 확인하세요.");
        }
    }

    // [F키 눌렀을 때] 아웃라인 재질로 '교체'
    public void Highlight(Material outlineMat)
    {
        if (myRenderer == null) return;

        // 2D 이미지는 덧입히기보다 교체하는 방식이 훨씬 깔끔합니다.
        myRenderer.material = outlineMat;
    }

    // [시간 종료] 원래 재질로 '복구'
    public void Unhighlight()
    {
        if (myRenderer == null) return;

        // 저장해뒀던 원래 재질로 돌아갑니다.
        myRenderer.material = defaultMaterial;
    }
}