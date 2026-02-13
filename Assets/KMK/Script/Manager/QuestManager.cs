using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    private HashSet<string> clearedQuestIDs = new HashSet<string>();

    private void Awake()
    {
        if (instance == null) { instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }

    public void RegisterQuestClear(string questID)
    {
        if (!clearedQuestIDs.Contains(questID))
        {
            clearedQuestIDs.Add(questID);
            Debug.Log($"[Quest] 클리어 기록: {questID}");
            // SaveQuestData(); // 나중에 저장 추가
        }
    }

    public bool IsQuestCleared(string questID)
    {
        return clearedQuestIDs.Contains(questID);
    }
}