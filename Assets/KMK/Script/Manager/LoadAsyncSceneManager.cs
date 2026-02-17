using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadAsyncSceneManager : MonoBehaviour
{
    public static LoadAsyncSceneManager instance { get; private set; }

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private CanvasGroup fadeCanvasGroup;

    public bool IsFading { get; private set; }

    private void Awake()
    {
        // 싱글톤 세팅
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        fadeCanvasGroup.alpha = 1f;
        FadeInOut(true, 1.0f);
    }

    /// <summary>
    /// 외부에서 호출: 다음 씬으로 페이드 전환.
    /// 사용법: FadeSceneManager.Instance.FadeToScene("이동할씬이름");
    /// </summary>
    public void FadeToScene(string sceneName)
    {
        if (!IsFading)
        {
            IsFading = true;

            // 1) UIManager를 시켜서 페이드 아웃 (화면 까맣게 덮기)
            // false = 페이드 아웃(1), fadeDuration = 시간
            FadeInOut(false, fadeDuration, () =>
            {
                // 페이드 아웃이 완전히 끝나면 이 안쪽이 실행됨!
                // 2) 화면이 완전히 가려졌으니 안심하고 비동기 씬 로드 시작
                StartCoroutine(LoadSceneAsync(sceneName));
            });
        }
    }

    private IEnumerator LoadSceneAsync(string sceneName = null)
    {
        // 씬 비동기 로드 시작
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        // 씬 로딩이 90% 완료되어도 바로 화면을 넘기지 않도록 막아둠
        op.allowSceneActivation = false;

        // 씬 로딩이 거의 끝날 때까지 대기 (progress 0~0.9)
        while (op.progress < 0.9f)
        {
            yield return null;
        }

        // 로딩이 완료되었으니 씬 활성화 (화면은 여전히 까만색임)
        op.allowSceneActivation = true;


        // 씬이 완전히 켜질 때까지 대기
        while (!op.isDone)
        {
            yield return null;
        }

        yield return null;
        DataManager.instance.SaveGame();
        // 3) 새로운 씬이 완전히 준비되었으니, UIManager를 시켜서 페이드 인 (화면 밝게 켜기)
        // true = 페이드 인(0)
        FadeInOut(true, fadeDuration, () =>
        {
            // 화면이 완전히 밝아지면 페이드 작업 종료 선언
            IsFading = false;
            Debug.Log("FadeIn 완료 추가 로직 실행");
        });
    }


    // ------------------ 페이드 인 아웃 UI --------------------------
    public void FadeInOut(bool isFadeIn, float duration, Action onComplete = null)
    {
        if (fadeCanvasGroup == null) return;

        float targetAlpha = isFadeIn ? 0f : 1f;

        // 페이드 중에는 클릭 막기
        fadeCanvasGroup.blocksRaycasts = true;

        fadeCanvasGroup.DOFade(targetAlpha, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                onComplete?.Invoke();

                // 밝아졌으면(FadeIn) 다시 클릭 가능하게 풀기
                if (isFadeIn)
                {
                    fadeCanvasGroup.blocksRaycasts = false;
                }
            });
    }

}
