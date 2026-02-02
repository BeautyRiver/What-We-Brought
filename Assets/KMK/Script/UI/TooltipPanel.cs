using System.Collections;
using TMPro;
using UnityEngine;

public class TooltipPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tooltipText;
    [SerializeField] private Vector2 offset = new Vector2(15, -15); // 마우스 우하단

    private RectTransform rectTransform;
    private Coroutine hideCoroutine; // 코루틴 제어용 변수

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        Hide(); // 시작할 때 숨김
    }

    private void Update()
    {
        // 켜져 있을 때만 마우스 위치 따라가기
        if (gameObject.activeSelf)
        {
            transform.position = Input.mousePosition + (Vector3)offset;
        }
    }

    public void Show(string message, float duration = 0f)
    {
        tooltipText.text = message;
        gameObject.SetActive(true);

        transform.position = Input.mousePosition + (Vector3)offset;

        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        if (duration > 0f)
        {
            hideCoroutine = StartCoroutine(AutoHideRoutine(duration));
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator AutoHideRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        Hide();
    }
}