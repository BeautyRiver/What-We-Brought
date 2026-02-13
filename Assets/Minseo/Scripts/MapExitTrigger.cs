using UnityEngine;

public class MapExitTrigger : MonoBehaviour
{
    [SerializeField] private string nextSceneName; // 인스펙터에서 입력

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other) // 3D
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;

        hasTriggered = true;

        if (FadeSceneManager.Instance != null)
        {
            FadeSceneManager.Instance.FadeToScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("FadeSceneManager 인스턴스를 찾을 수 없음. 그냥 씬을 로드합니다.");
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
        }
    }

    // 2D 사용 시 이걸로 교체
    /*
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;

        hasTriggered = true;

        if (FadeSceneManager.Instance != null)
        {
            FadeSceneManager.Instance.FadeToScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("FadeSceneManager 인스턴스를 찾을 수 없음. 그냥 씬을 로드합니다.");
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
        }
    }
    */
}
