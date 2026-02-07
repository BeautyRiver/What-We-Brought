using UnityEngine;
using DG.Tweening; // ⭐ DOTween 필수

public class Highlightable : MonoBehaviour
{
    [Header("기본 설정")]
    [Tooltip("체크하면 게임 시작 시 투명하게 숨겨집니다.")]
    public bool isHiddenByDefault = true;
    public bool IsVisible => myRenderer != null && myRenderer.enabled;

    [Header("조명 연출 설정")]
    [Tooltip("자식 오브젝트에 있는 Light를 연결하세요. (없으면 알아서 찾음)")]
    public Light highlightLight;

    [Tooltip("빛이 켜지고 꺼지는 시간 (초)")]
    public float fadeDuration = 0.5f; // 💡 켜지는 시간 변수

    // 내부 변수
    private SpriteRenderer myRenderer;
    private bool isHighlighted = false;
    private float originalIntensity; // 💡 원래 설정해둔 밝기 기억용

    void Awake()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        if (myRenderer == null) myRenderer = GetComponentInChildren<SpriteRenderer>();

        // 1. 라이트 찾기
        if (highlightLight == null) highlightLight = GetComponentInChildren<Light>();

        // 2. ⭐ 원래 밝기 기억하기!
        if (highlightLight != null)
        {
            originalIntensity = highlightLight.intensity; // 인스펙터에서 설정한 값 저장
        }
    }

    void Start()
    {
        // 3. 기본적으로 숨김 처리
        if (isHiddenByDefault && myRenderer != null)
        {
            myRenderer.enabled = false;
        }

        // 4. 라이트 초기화 (꺼두기)
        if (highlightLight != null)
        {
            highlightLight.intensity = 0f; // 밝기 0으로 시작 (꺼짐)
            highlightLight.enabled = true; // 컴포넌트는 켜둠
        }
    }

    // Crow.cs에서 호출
    public void Highlight(Material outlineMat = null)
    {
        if (isHighlighted) return;
        isHighlighted = true;

        // 물체 모습 드러내기
        if (myRenderer != null)
        {
            myRenderer.enabled = true;
            // (선택) 투명도 페이드인 추가 가능: myRenderer.DOFade(1f, fadeDuration);
        }

        // ⭐ 빛이 서서히 밝아짐 (0 -> 원래 밝기)
        if (highlightLight != null)
        {
            highlightLight.DOKill();
            // fadeDuration 변수 사용!
            highlightLight.DOIntensity(originalIntensity, fadeDuration).SetEase(Ease.OutQuad);
        }
    }

    // Crow.cs에서 호출
    public void Unhighlight()
    {
        if (!isHighlighted) return;
        isHighlighted = false;

        // ⭐ 빛이 서서히 사라짐 (현재 밝기 -> 0)
        if (highlightLight != null)
        {
            highlightLight.DOKill();
            // fadeDuration 변수 사용!
            highlightLight.DOIntensity(0f, fadeDuration).SetEase(Ease.InQuad)
                .OnComplete(() =>
                {
                    // 빛이 다 꺼진 뒤에 물체 숨기기
                    if (isHiddenByDefault && myRenderer != null)
                    {
                        myRenderer.enabled = false;
                    }
                });
        }
        else
        {
            if (isHiddenByDefault && myRenderer != null) myRenderer.enabled = false;
        }
    }
}