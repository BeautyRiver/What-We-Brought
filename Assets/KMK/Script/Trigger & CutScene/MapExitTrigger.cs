using UnityEngine;

public class MapExitTrigger : EventTrigger
{
    [SerializeField] private string nextSceneName; // 인스펙터에서 입력

    [SerializeField] private string targetSpawnPointID;

    [Header("메시지 설정")]
    public string ratMessage = "쥐로는 문을 지나갈 수 없습니다.";

    protected override void StartEvent(Collider other)
    {     
        if (other.CompareTag("Rat"))
        {
            UIManager.instance.ShowInfoPanel(ratMessage);
            hasTriggered = false;
        }

        else
        {
            hasTriggered = true;

            if (DataManager.instance != null)
            {
                DataManager.instance.nextSpawnPointID = targetSpawnPointID;
            }

            if (LoadAsyncSceneManager.instance != null)
            {
                LoadAsyncSceneManager.instance.FadeToScene(nextSceneName);
            }
            else
            {
                Debug.LogWarning("FadeSceneManager 인스턴스를 찾을 수 없음. 그냥 씬을 로드합니다.");
                UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
            }
        }
    }
  

    protected override void EndEvent(Collider other)
    {
        if (other.CompareTag("Rat"))
        {
            UIManager.instance.HideInfoPanel();
        }
    }
}
