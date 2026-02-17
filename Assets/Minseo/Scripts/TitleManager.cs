using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public GameObject settingsPanel;
    public Button loadGameButton;
    private void Start()
    {
        CleanUpManagers(); // 매니저들 청소
        SoundManager.instance.PlayBGM("Title");

        if (DataManager.instance != null)
        {
            // 실제 파일이 있을 때만 버튼 활성화
            bool hasData = DataManager.instance.ExistData();
            loadGameButton.interactable = hasData;
        }
    }

    private void CleanUpManagers()
    {
        if (UIManager.instance != null)
        {
            Destroy(UIManager.instance.gameObject);
            UIManager.instance = null;
        }
        if (PuzzleGameManager.instance != null)
        {
            Destroy(PuzzleGameManager.instance.gameObject);
            PuzzleGameManager.instance = null;
        }


        // 4. (만약 있다면) 캐릭터 스왑 매니저, 퀘스트 매니저 등등...
        // if (QuestManager.instance != null) Destroy(QuestManager.instance.gameObject);

        Debug.Log("🧹 타이틀 진입: 인게임 매니저들을 모두 정리했습니다.");
    }
    public void OnClickLoadGameStart()
    {
        if (DataManager.instance == null) return;

        string sceneToLoad = DataManager.instance.currentData.currentSceneName;

        if (string.IsNullOrEmpty(sceneToLoad))
        {
            sceneToLoad = "Tutorial";
        }

        //if (sceneToLoad == "Tutorial" && !DataManager.instance.currentData.isTutorialCleared)
        //{
        //    DataManager.instance.nextSpawnPointID = "Tutorial_Start_Pos";
        //}

        Debug.Log($"게임 시작! 이동할 씬: {sceneToLoad}");        

        LoadAsyncSceneManager.instance.FadeToScene(sceneToLoad);
    }

    public void OnClickNewGameStart()
    {
        DataManager.instance.ResetData();
        OnClickLoadGameStart();
    }
    public void OnClickSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void OnClickCloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void OnClickExit()
    {
#if UNITY_EDITOR
        // 에디터 플레이 모드 종료
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        // 빌드된 게임 종료
        Application.Quit();
    }
}