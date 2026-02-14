using UnityEngine;

public class MapExitTrigger : EventTrigger
{
    [SerializeField] private string nextSceneName; // 인스펙터에서 입력

    private bool hasTriggered = false;

    protected override void StartEvent()
    {
        if (hasTriggered) return;

        hasTriggered = true;

        if (LoadAsyncSceneManager.Instance != null)
        {
            LoadAsyncSceneManager.Instance.FadeToScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("FadeSceneManager 인스턴스를 찾을 수 없음. 그냥 씬을 로드합니다.");
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
        }
    }    
}
