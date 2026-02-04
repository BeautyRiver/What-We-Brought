using System.Collections;
using TMPro;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    [Header("UI 연결")]
    public TextMeshProUGUI tutorialText;

    private Coroutine currentRoutine;
    private CanvasGroup canvasGroup;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        HideInfo();
    }
    // duration이 0이면 무제한, 0보다 크면 시간 제한
    public void ShowInfo(string message, float duration = 0f)
    {
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
            currentRoutine = null;
        }

        // 텍스트 표시
        tutorialText.text = message;

        canvasGroup.alpha = 1f;

        // 시간 제한이 있다면 타이머 시작
        if (duration > 0f)
        {
            currentRoutine = StartCoroutine(AutoHideRoutine(duration));
        }
    }

    public void HideInfo()
    {
        tutorialText.text = "";

        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
            currentRoutine = null;
        }
        canvasGroup.alpha = 0f;
    }

    private IEnumerator AutoHideRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        HideInfo();
    }
}