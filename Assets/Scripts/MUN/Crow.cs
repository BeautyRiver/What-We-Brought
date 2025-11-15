using UnityEngine;
using System.Collections; // 코루틴

public class Crow : MonoBehaviour
{
    [Header("능력 설정")]
    [Tooltip("까마귀 능력이 지속되는 시간 (초)")]
    public float duration = 30.0f; // 지속시간

    [Tooltip("능력 사용 후 대기 시간 (초)")]
    public float cooldown = 180.0f; // 쿨타임

    [Header("강조 설정")]
    [Tooltip("강조할 오브젝트의 태그")]
    public string highlightTag = "Clue"; // 강조할 아이템 태그

    [Tooltip("강조할 때 사용할 색상")]
    public Color highlightColor = Color.yellow;

    private bool isAbilityReady = true; // 현재 능력 사용이 가능한가?

    void Update()
    {
     
        
        if (Input.GetKeyDown(KeyCode.F) && isAbilityReady)
        {
            // 타이머, 쿨타임 시작
            StartCoroutine(CrowAbilityCoroutine());
        }
        else if (Input.GetKeyDown(KeyCode.F) && !isAbilityReady)
        {
            Debug.Log("까마귀 능력이 아직 준비되지 않았습니다. (쿨타임)");
        }
    }

    
    IEnumerator CrowAbilityCoroutine()  // 코루틴사용을 위해 필요
    {
  
        isAbilityReady = false; 
        Debug.Log("까마귀 능력 활성화 " + duration + "초 동안 단서를 찾습니다.");

        // Clue 태그를 가진 오브젝트 찾기
        GameObject[] clues = GameObject.FindGameObjectsWithTag(highlightTag);

        // 강조해야할 아이템 강조
        foreach (GameObject clueObject in clues)
        {
            Highlightable h = clueObject.GetComponent<Highlightable>();
            if (h != null)
            {
                h.Highlight(highlightColor); // 하이라이트 스크립트 실행
            }
        }

       
        // 코드가 지속시간동안 정지
        yield return new WaitForSeconds(duration);

        Debug.Log("까마귀 능력 지속 시간 종료. 강조를 해제합니다.");

        // 강조 정지
        foreach (GameObject clueObject in clues)
        {
            if (clueObject != null) 
            {
                Highlightable h = clueObject.GetComponent<Highlightable>();
                if (h != null)
                {
                    h.Unhighlight(); // 원래대로 복구
                }
            }
        }

        
        Debug.Log("쿨타임 시작. " + cooldown + "초 후에 사용 가능합니다.");
        yield return new WaitForSeconds(cooldown); // 쿨타임동안 코드 정지

  
        isAbilityReady = true;
        Debug.Log("까마귀 능력이 준비되었습니다.");
    }
}