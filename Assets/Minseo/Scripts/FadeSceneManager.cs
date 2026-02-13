using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeSceneManager : MonoBehaviour
{
    public static FadeSceneManager Instance { get; private set; }

    [Header("Fade Settings")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 1f;

    private bool isFading = false;

    private void Awake()
    {
        // 싱글톤 세팅
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (fadeCanvasGroup != null)
            fadeCanvasGroup.alpha = 0f; // 시작은 밝게
    }

    /// <summary>
    /// 외부에서 호출: 다음 씬으로 페이드 전환.
    /// </summary>
    public void FadeToScene(string sceneName)
    {
        if (!isFading)
            StartCoroutine(FadeAndSwitchScene(sceneName));
    }

    private IEnumerator FadeAndSwitchScene(string sceneName)
    {
        isFading = true;

        // 1) 페이드 아웃 (0 -> 1)
        yield return StartCoroutine(Fade(0f, 1f));

        // 2) 비동기 씬 로드
        yield return StartCoroutine(LoadSceneAsync(sceneName));

        // 3) 페이드 인 (1 -> 0)
        yield return StartCoroutine(Fade(1f, 0f));

        isFading = false;
    }

    private IEnumerator Fade(float from, float to)
    {
        float time = 0f;

        fadeCanvasGroup.alpha = from;
        fadeCanvasGroup.blocksRaycasts = true;   // 페이드 중 입력 막기

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / fadeDuration);
            fadeCanvasGroup.alpha = Mathf.Lerp(from, to, t);
            yield return null;
        }

        fadeCanvasGroup.alpha = to;
        fadeCanvasGroup.blocksRaycasts = (to > 0.99f); // 완전히 어두울 때만 막기
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        op.allowSceneActivation = false;

        // 씬 로딩이 거의 끝날 때까지 대기 (progress 0~0.9)
        while (op.progress < 0.9f)
        {
            // 필요하면 여기서 op.progress로 로딩 퍼센트 사용 가능
            yield return null;
        }

        // 이제 씬 활성화
        op.allowSceneActivation = true;

        while (!op.isDone)
        {
            yield return null;
        }
    }
}
