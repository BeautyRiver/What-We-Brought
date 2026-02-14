using UnityEngine;
using System; // Guid 사용을 위해 필요

public class SaveableObject : MonoBehaviour
{
    [Header("고유 식별자 (필수)")]
    public string objectID;

    private void Start()
    {
        if (DataManager.instance == null) return;
        if (DataManager.instance.currentData.collectedObjectIDs.Contains(objectID))
        {
            gameObject.SetActive(false);
        }
    }

    public void MarkAsGone()
    {
        if (DataManager.instance == null) return;
        if (!DataManager.instance.currentData.collectedObjectIDs.Contains(objectID))
        {
            DataManager.instance.currentData.collectedObjectIDs.Add(objectID);
            DataManager.instance.SaveGame();
        }
        gameObject.SetActive(false);
    }

    // 👇 [꿀팁] 인스펙터에서 우클릭하면 ID 자동 생성!
    [ContextMenu("Generate Unique ID")]
    private void GenerateID()
    {
        // 씬 이름 + 오브젝트 이름 + 랜덤코드 조합
        objectID = System.Guid.NewGuid().ToString();

        // 에디터에서 변경사항 저장되게 표시
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}