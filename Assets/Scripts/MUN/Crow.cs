using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Crow : MonoBehaviour
{
    [Header("능력 설정")]
    public float duration = 30.0f;
    public float cooldown = 180.0f;
    public float detectionRadius = 10.0f; // 감지 범위

    [Header("강조 설정")]
    public string highlightTag = "Clue";

    // [중요 변경] 색깔(Color) 대신 재질(Material)을 받습니다!
    public Material outlineEffectMaterial;

    private bool isAbilityReady = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isAbilityReady)
        {
            StartCoroutine(CrowAbilityCoroutine());
        }
    }

    IEnumerator CrowAbilityCoroutine()
    {
        isAbilityReady = false;
        Debug.Log("까마귀 능력 활성화!");

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        List<Highlightable> activeHighlights = new List<Highlightable>();

        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag(highlightTag))
            {
                // 부모나 자식에 있는 Highlightable 스크립트를 찾음
                Highlightable h = col.GetComponentInChildren<Highlightable>();
                if (h == null) h = col.GetComponentInParent<Highlightable>();

                if (h != null)
                {
                    // [중요] 색깔 대신 아웃라인 재질을 전달함
                    h.Highlight(outlineEffectMaterial);
                    activeHighlights.Add(h);
                }
            }
        }

        yield return new WaitForSeconds(duration);

        // 시간 종료 후 끄기
        foreach (Highlightable h in activeHighlights)
        {
            if (h != null) h.Unhighlight();
        }

        Debug.Log("쿨타임 시작");
        yield return new WaitForSeconds(cooldown);
        isAbilityReady = true;
    }

    // 에디터에서 범위를 눈으로 보기 위한 기즈모
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}