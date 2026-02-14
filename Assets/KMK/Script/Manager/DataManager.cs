using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public class GameData
{
    // 1. 튜토리얼 관련
    public bool isTutorialCleared = false;
    public bool hasSeenTomasCutscene = false;

    // 2. 수집한 아이템 ID 목록 (String List)
    public List<string> collectedObjectIDs = new List<string>();

    // 3. 클리어한 퍼즐/퀘스트 ID 목록    
    public List<string> clearedPuzzleIDs = new List<string>();
    public List<string> clearedQuestIDs = new List<string>();

    // 4. 플레이어 위치 (구조체는 저장 잘 됨)
    public Vector3 playerPosition;
    public string currentSceneName;
}

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    // ⭐ 현재 게임의 데이터를 담고 있는 변수
    public GameData currentData;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadGame(); // 게임 켜자마자 데이터 불러오기
            
        }
        else Destroy(gameObject);
    }

    // 💾 저장: 클래스 통째로 저장
    public void SaveGame()
    {
        // 현재 상태 갱신 (예: 위치 정보 등)
        // (아이템 리스트나 퀘스트 리스트는 획득할 때마다 currentData에 넣었을 테니 그대로 둠)

        ES3.Save("MyGameData", currentData); // ⭐ 딱 한 줄로 끝!
        Debug.Log("게임 데이터 통째로 저장 완료!");
    }

    // 📂 로드: 클래스 통째로 불러오기
    public void LoadGame()
    {
        // 저장된 파일이 있으면 불러오고, 없으면 새로 만듦
        if (ES3.KeyExists("MyGameData"))
        {
            currentData = ES3.Load<GameData>("MyGameData");
        }
        else
        {
            currentData = new GameData(); // 새 게임 데이터 생성
        }
    }
}
