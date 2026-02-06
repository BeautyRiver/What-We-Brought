using UnityEngine;

public class SaveableObject : MonoBehaviour
{
    [Header("고유 식별자 (필수)")]
    [Tooltip("맵이름_오브젝트명_번호 형식")]
    public string objectID;

    private void Start()
    {
        if (DataManager.instance == null) return;

        // 2. "사라진 오브젝트 명단"에 내 이름이 있는지 확인
        if (DataManager.instance.currentData.collectedObjectIDs.Contains(objectID))
        {
            gameObject.SetActive(false);
        }
    }

    // 외부에서 호출: "나 이제 할 일 다 했으니 기록하고 사라질게"
    public void MarkAsGone()
    {
        if (DataManager.instance == null) return;

        // 명단에 없으면 추가
        if (!DataManager.instance.currentData.collectedObjectIDs.Contains(objectID))
        {
            DataManager.instance.currentData.collectedObjectIDs.Add(objectID);

            // 데이터 변경됐으니 즉시 저장 (필요에 따라 주석 처리 가능)
            DataManager.instance.SaveGame();

            Debug.Log($"[SaveableObject] 상태 저장됨: {objectID}");
        }

        // 오브젝트 끄기
        gameObject.SetActive(false);
    }
}