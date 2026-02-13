using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System; // ⭐ DOTween 사용 필수

public class Crow : MonoBehaviour
{
    [Header("능력 설정")]
    public float duration = 20.0f;        // 하이라이트 유지 시간
    public float cooldown = 120.0f;       // 쿨타임
    public float detectionRadius = 15.0f; // 탐지 반경

    [Header("탐지 설정")]
    public string highlightTag = "Clue";  // 탐지할 태그
    public LayerMask clueLayer;           // 탐지할 레이어 (최적화용)

    [Header("시각 효과 설정")]
    public Material outlineEffectMaterial; // 하이라이트 재질
    public Material outlineEffectMaterialTransparent; // 하이라이트 재질(투명)

    public GameObject scanEffectPrefab;    // 반투명 구체 Prefab
    public float scanWaveSpeed = 1.5f;     // 파동이 퍼지는 속도 (초)

    private bool isAbilityReady = true;    // 쿨타임 체크용

    // 활성화된 하이라이트 목록 (능력 종료 시 끄기 위해 저장)
    private List<Highlightable> activeHighlights = new List<Highlightable>();

    public void HadleCrowAbility()
    {
        // F키를 눌러 능력 사용
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isAbilityReady)
            {
                StartCoroutine(CrowAbilityRoutine());
            }
            else
            {
                Debug.Log("쿨타임 중입니다!");
            }
        }
    }

    IEnumerator CrowAbilityRoutine()
    {
        isAbilityReady = false;
        Debug.Log("까마귀 탐지 시작 [F]");

        // 1. 기존 하이라이트 정리 (안전장치)
        ClearActiveHighlights();

        // 2. 스캔 이펙트(파동) 생성 및 DOTween 연출
        if (scanEffectPrefab != null)
        {
            GameObject scanVFX = Instantiate(scanEffectPrefab, transform.position, Quaternion.identity);
            Transform t = scanVFX.transform;
            Renderer r = scanVFX.GetComponent<Renderer>();

            // [초기 상태] 크기는 0, 알파값은 절반 정도
            t.localScale = Vector3.zero;
            Color startColor = r.material.color;
            startColor.a = 0.5f;
            r.material.color = startColor;

            // [DOTween 시퀀스] 커지면서 + 사라지기
            Sequence seq = DOTween.Sequence();

            // 1) 크기 확대: 지름(Scale)을 반지름 * 2 만큼 키움
            seq.Join(t.DOScale(Vector3.one * detectionRadius * 2f, scanWaveSpeed)
                .SetEase(Ease.OutQuad)); // 슉~ 퍼지는 느낌

            // 2) 투명도 감소: 서서히 투명해짐 (Fade Out)
            seq.Join(r.material.DOFade(0f, scanWaveSpeed)
                .SetEase(Ease.InQuad));  // 끝에서 자연스럽게 사라짐

            // 3) 종료 후 삭제
            seq.OnComplete(() => Destroy(scanVFX));
        }

        // 3. 주변 단서 탐지 및 거리별 지연 효과
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, clueLayer);

        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag(highlightTag))
            {
                Highlightable h = col.GetComponent<Highlightable>();
                if (h != null)
                {
                    // 거리 계산
                    float distance = Vector3.Distance(transform.position, col.transform.position);

                    // 파동이 이 물체에 도달하는 시간 계산 (거리 비례)
                    float arrivalTime = (distance / detectionRadius) * scanWaveSpeed;

                    // 파동이 닿는 타이밍에 맞춰 하이라이트 켜기
                    StartCoroutine(DelayedHighlight(h, arrivalTime));

                    // 관리 목록에 추가
                    activeHighlights.Add(h);
                }
            }
        }

        // 4. 유지 시간 대기 (파동 시간 + 지속 시간)
        yield return new WaitForSeconds(scanWaveSpeed + duration);

        // 5. 능력 종료 (물리적 하이라이트 끄기)
        ClearActiveHighlights();
        Debug.Log("능력 유지 끝 - 진짜 쿨타임(충전) 시작");

        // 6. 쿨타임 대기
        yield return new WaitForSeconds(cooldown);

        isAbilityReady = true;
        Debug.Log("쿨타임 종료 - 시스템적으로 사용 가능");
    }

    // 시간차 하이라이트 코루틴
    IEnumerator DelayedHighlight(Highlightable target, float delay)
    {
        yield return new WaitForSeconds(delay);

        // 능력이 유효한 상태인지 확인 후 켜기
        if (target != null && !isAbilityReady)
        {
            target.Highlight(outlineEffectMaterial);

        }
    }

    // 하이라이트 해제 헬퍼 함수
    void ClearActiveHighlights()
    {
        foreach (Highlightable h in activeHighlights)
        {
            if (h != null) h.Unhighlight();
        }
        activeHighlights.Clear();
    }

    // 디버그용 기즈모 (범위 표시)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }


}