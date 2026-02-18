using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VInspector;

[System.Serializable] 
public class GameData
{
    // 1. 튜토리얼 관련
    public bool isTutorialCleared = false;
    public bool hasSeenTomasCutscene = false;

    // 2. 수집한 아이템 ID 목록 (String List)
    public List<string> collectedObjectIDs = new List<string>();
    public List<string> inventory = new List<string>();

    // 3. 클리어한 퍼즐/퀘스트 ID 목록    
    public List<string> clearedPuzzleIDs = new List<string>();
    public List<string> clearedQuestIDs = new List<string>();

    // 4. 플레이어 위치 (구조체는 저장 잘 됨)
    public Vector3 playerPosition;
    public string currentSceneName = "Tutorial";
    public float lastFacingDirection = 1f;
}

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public GameData currentData;
    
    public string nextSpawnPointID;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadGame();
        }
        else Destroy(gameObject);
    }

    // =========================================================
    // 1️⃣ 튜토리얼 & 컷씬 관리 (Bool 값)
    // =========================================================
    public void SetTutorialCleared()
    {
        if (currentData.isTutorialCleared) return; // 이미 깼으면 무시

        currentData.isTutorialCleared = true;
        SaveGame(); // ⭐ 중요 데이터 변경 시 바로 저장!
        Debug.Log("튜토리얼 클리어 저장됨");
    }

    public void SetTomasCutsceneSeen()
    {
        currentData.hasSeenTomasCutscene = true;
        SaveGame();
    }

    // =========================================================
    // 수집 아이템 관리 (List 관리 + 중복 방지)
    // =========================================================
    public void AddCollectedItem(string itemID)
    {
        // ⭐ 핵심: 이미 먹은 건지 체크 (데이터 무결성)
        if (currentData.collectedObjectIDs.Contains(itemID))
        {
            Debug.Log($"이미 획득한 아이템입니다: {itemID}");
            return;
        }

        currentData.collectedObjectIDs.Add(itemID);

        currentData.inventory.Add(itemID);

        SaveGame(); // 획득 즉시 저장
        Debug.Log($"아이템 획득 저장 완료: {itemID}");
    }

    // =========================================================
    // 🧪 아이템 사용
    // =========================================================
    public void UseItem(string itemID)
    {
        if (currentData.inventory.Contains(itemID))
        {
            // 1. [가방]에서만 삭제!
            currentData.inventory.Remove(itemID);

            SaveGame();
            Debug.Log($"아이템 사용함: {itemID}");
        }
    }

    // 맵에 있는 오브젝트가 켜질지 말지 결정하는 함수
    public bool IsWorldObjectCollected(string worldObjectID)
    {
        return currentData.collectedObjectIDs.Contains(worldObjectID);
    }

    // 아이템 보유 여부 확인용 (Getter)
    public bool HasItem(string itemID)
    {
        return currentData.collectedObjectIDs.Contains(itemID);
    }

    // =========================================================
    // 퍼즐 & 퀘스트 관리
    // =========================================================
    public void ClearPuzzle(string puzzleID)
    {
        if (currentData.clearedPuzzleIDs.Contains(puzzleID)) return;

        currentData.clearedPuzzleIDs.Add(puzzleID);
        SaveGame();
        Debug.Log($"퍼즐 클리어 저장: {puzzleID}");
    }

    public bool IsPuzzleCleared(string puzzleID)
    {
        return currentData.clearedPuzzleIDs.Contains(puzzleID);
    }

    // 저장 & 로드 
    public void SaveGame()
    {
        // 저장할 때 현재 씬 이름과 위치도 갱신        
        string activeSceneName = SceneManager.GetActiveScene().name;
        if (activeSceneName != "Title")
        {
            currentData.currentSceneName = activeSceneName;

            // 플레이어 위치 저장 (
            if (GameManager.instance != null && GameManager.instance.characterSwapManager != null)
            {
                // 현재 조작 중인 캐릭터 위치 저장
                if (GameManager.instance.characterSwapManager.humanObject != null)
                {
                    currentData.playerPosition = GameManager.instance.characterSwapManager.humanObject.transform.position;
                }
            }
        }

        ES3.Save("MyGameData", currentData);
        Debug.Log("게임이 저장되었습니다.");
    }

    public bool ExistData()
    {
        return ES3.KeyExists("MyGameData");
    }
    public void LoadGame()
    {
        if (ES3.KeyExists("MyGameData"))
            currentData = ES3.Load<GameData>("MyGameData");
        else
            currentData = new GameData();
    }

    [Button]
    public void ResetData()
    {
        currentData = new GameData(); // 메모리 상의 데이터는 깨끗하게 초기화
        ES3.DeleteKey("MyGameData");  // 실제 저장 파일 삭제
        Debug.Log("데이터 리셋 완료 (파일 삭제됨)");
    }
}