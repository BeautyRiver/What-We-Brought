using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Crow : MonoBehaviour
{
    [Header("능력 설정")]
    public float duration = 30.0f;
    public float cooldown = 180.0f;
    public float detectionRadius = 10.0f;

    [Header("강조 설정")]
    public string highlightTag = "Clue";

    // [변경] 색상 대신 아웃라인 머티리얼을 직접 받습니다.
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
                Highlightable h = col.GetComponent<Highlightable>(); // GetComponentInParent가 필요할 수도 있음
                if (h != null)
                {
                    // [변경] 재질을 전달합니다.
                    h.Highlight(outlineEffectMaterial);
                    activeHighlights.Add(h);
                }
            }
        }

        yield return new WaitForSeconds(duration);

        // 끄기
        foreach (Highlightable h in activeHighlights)
        {
            if (h != null) h.Unhighlight();
        }

        Debug.Log("쿨타임 시작");
        yield return new WaitForSeconds(cooldown);
        isAbilityReady = true;
        Debug.Log("준비 완료");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}