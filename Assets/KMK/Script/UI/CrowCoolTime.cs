using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections; // ⭐ DOTween 필수

public class CrowCoolTime : MonoBehaviour
{
    [Header("UI 연결")]
    public Image fillImage; // 0~1로 차오를 게이지 (밝은색 원본 아이콘 추천!)
    public Image skillIcon; // 번쩍이고 튕길 전체 아이콘

    private Sequence cooldownSeq; // DOTween 시퀀스 저장용
    private Color originalColor;

    private void Start()
    {
        if (skillIcon != null) originalColor = skillIcon.color;

        // 처음엔 스킬이 준비된 상태니까 게이지를 1(100%)로 꽉 채워둠
        if (fillImage != null) fillImage.fillAmount = 1f;
    }

    // ⭐ 지속 시간(activeTime)과 쿨타임(coolTime) 두 개를 받음
    public void StartCooldown(float activeTime, float coolTime)
    {
        // 1. 기존에 돌고 있던 트윈 애니메이션이 있다면 강제 종료 (버그 방지)
        if (cooldownSeq != null && cooldownSeq.IsActive()) cooldownSeq.Kill();

        if (skillIcon != null)
        {
            skillIcon.transform.DOKill();
            skillIcon.color = originalColor;
            skillIcon.transform.localScale = Vector3.one;
        }

        // 2. DOTween 시퀀스(연속 동작) 생성
        cooldownSeq = DOTween.Sequence();

        // 3. 시작하자마자 100%로 설정
        fillImage.fillAmount = 1f;

        // [동작 1] 지속 시간(activeTime) 동안 1 -> 0으로 쭈욱 닳아 없어짐 (스킬 유지 시간)
        cooldownSeq.Append(fillImage.DOFillAmount(0f, activeTime).SetEase(Ease.Linear));

        // [동작 2] 이어서 쿨타임(coolTime) 동안 0 -> 1로 서서히 차오름 (스킬 재장전 시간)
        cooldownSeq.Append(fillImage.DOFillAmount(1f, coolTime).SetEase(Ease.Linear));

        // [동작 3] 1까지 다 차오르면 대망의 하이라이트 연출 실행!
        cooldownSeq.OnComplete(() => PlayReadyEffect());
    }
    
    // ⭐ 스킬 준비 완료 DOTween 연출
    private void PlayReadyEffect()
    {
        if (skillIcon == null) return;

        // 통통 튀는 효과
        skillIcon.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0f), 0.5f, 5, 1f);
        fillImage.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0f), 0.5f, 5, 1f);

        // 하얗게 번쩍! 하는 효과
        skillIcon.DOColor(Color.white, 0.15f)
            .SetLoops(2, LoopType.Yoyo)
            .OnComplete(() => skillIcon.color = originalColor); // 끝나면 원래 색으로
    }
}