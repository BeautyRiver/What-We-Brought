using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Crow : MonoBehaviour
{
    [Header("능력 설정")]
    public float duration = 20.0f; // 기획서 기준 20초
    public float cooldown = 120.0f; // 기획서 난이도별 2~3분
    public float detectionRadius = 15.0f; // 범위 설정

    [Header("탐지 설정")]
    public string highlightTag = "Clue"; // 태그 확인
    public LayerMask clueLayer; // [추가] 최적화를 위해 단서 아이템만 있는 레이어를 선택하세요.

    [Header("시각 효과")]
    public Material outlineEffectMaterial; // [필수] 'Sprite Outline' 쉐이더가 적용된 재질

    private bool isAbilityReady = true;

    public void HadleCrowAbility()
    {
        // 쿨타임 UI 처리 등은 나중에 추가
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isAbilityReady)
            {
                StartCoroutine(CrowAbilityCoroutine());
            }
            else
            {
                Debug.Log("쿨타임 중입니다!"); // 나중에 UI 메시지로 연결
            }
        }
    }
    IEnumerator CrowAbilityCoroutine()
    {
        isAbilityReady = false;
        Debug.Log("까마귀 탐지 시작 [F]");

        // [중요] 3D Collider(BoxCollider)를 가진 오브젝트만 검출합니다.
        // clueLayer에 해당하는 오브젝트만 검사하여 성능 최적화
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, clueLayer);

        List<Highlightable> activeHighlights = new List<Highlightable>();

        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag(highlightTag))
            {
                Highlightable h = col.GetComponent<Highlightable>();
                if (h != null)
                {
                    h.Highlight(outlineEffectMaterial);
                    activeHighlights.Add(h);
                }
            }
        }

        // 지속 시간 대기
        yield return new WaitForSeconds(duration);

        // 끄기 (시간 종료)
        foreach (Highlightable h in activeHighlights)
        {
            if (h != null) h.Unhighlight();
        }
        activeHighlights.Clear();

        Debug.Log("능력 종료 - 쿨타임 시작");

        // 쿨타임 대기
        yield return new WaitForSeconds(cooldown);

        isAbilityReady = true;
        Debug.Log("쿨타임 종료 - 사용 가능");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}