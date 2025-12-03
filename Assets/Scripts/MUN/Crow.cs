using UnityEngine;
using System.Collections;
using System.Collections.Generic; // 리스트 사용을 위해 추가

public class Crow : MonoBehaviour
{
    [Header("능력 설정")]
    [Tooltip("까마귀 능력이 지속되는 시간 (초)")]
    public float duration = 30.0f;

    [Tooltip("능력 사용 후 대기 시간 (초)")]
    public float cooldown = 180.0f;

    [Tooltip("단서를 탐지할 반경")]
    public float detectionRadius = 10.0f; // 탐지 범위 추가

    [Header("강조 설정")]
    [Tooltip("강조할 오브젝트의 태그")]
    public string highlightTag = "Clue";

    [Tooltip("강조할 때 사용할 색상")]
    public Color highlightColor = Color.yellow;

    private bool isAbilityReady = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isAbilityReady)
        {
            StartCoroutine(CrowAbilityCoroutine());
        }
        else if (Input.GetKeyDown(KeyCode.F) && !isAbilityReady)
        {
            Debug.Log("까마귀 능력이 아직 준비되지 않았습니다. (쿨타임)");
        }
    }

    IEnumerator CrowAbilityCoroutine()
    {
        isAbilityReady = false;
        Debug.Log("까마귀 능력 활성화: 반경 " + detectionRadius + "m 내의 단서를 찾습니다.");

        // OverlapSphere 3D 
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);

        List<Highlightable> activeHighlights = new List<Highlightable>();

      
        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag(highlightTag))
            {
                Highlightable h = col.GetComponent<Highlightable>();
                if (h != null)
                {
                    h.Highlight(highlightColor);
                    activeHighlights.Add(h); 
                }
            }
        }

        // 지속 시간 대기
        yield return new WaitForSeconds(duration);

        Debug.Log("까마귀 능력 지속 시간 종료. 강조를 해제합니다.");

        // 3. 아까 켰던 애들만 다시 끄기
        foreach (Highlightable h in activeHighlights)
        {
            if (h != null)
            {
                h.Unhighlight();
            }
        }

        // 쿨타임 대기
        Debug.Log("쿨타임 시작. " + cooldown + "초 후에 사용 가능합니다.");
        yield return new WaitForSeconds(cooldown);

        isAbilityReady = true;
        Debug.Log("까마귀 능력이 준비되었습니다.");
    }

    // [편의 기능] 에디터 상에서 탐지 범위를 눈으로 보여주는 함수
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}