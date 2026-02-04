using System.Collections;
using TMPro;
using UnityEngine;

public class TooltipPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tooltipText;
    [SerializeField] private float heightOffset = 2.0f;

    private Transform targetTransform;
    private Coroutine hideCoroutine;

    // isActive 변수 대신 gameObject.activeSelf를 믿거나
    // targetTransform이 있는지 확인하는 게 더 깔끔해.

    private void Awake()
    {
        // 처음에 꺼두기
        gameObject.SetActive(false);
    }

    // ⭐ Update 대신 LateUpdate 사용 (카메라 떨림 방지)
    private void LateUpdate()
    {
        // 타겟이 없으면 계산할 필요 없음
        if (targetTransform == null) return;

        UpdatePosition();
    }

    private void UpdatePosition()
    {
        // 1. 월드 좌표 (머리 위)
        Vector3 worldPos = targetTransform.position + (Vector3.up * heightOffset);

        // 2. 화면 좌표 변환
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        // 3. 적용
        transform.position = screenPos;

        // 4. 카메라 뒤쪽인지 체크 (카메라 뒤에 있는데 화면에 뜨면 안 되니까)
        if (screenPos.z < 0)
            tooltipText.enabled = false;
        else
            tooltipText.enabled = true;
    }

    public void Show(string message, Transform target, float duration = 0f)
    {
        tooltipText.text = message;
        targetTransform = target;

        gameObject.SetActive(true); // 켜지면 LateUpdate가 돌기 시작함

        // 켜지자마자 위치 한 번 잡아주기 (깜빡임 방지)
        UpdatePosition();

        if (hideCoroutine != null) StopCoroutine(hideCoroutine);
        if (duration > 0f) hideCoroutine = StartCoroutine(AutoHideRoutine(duration));
    }

    public void Hide()
    {
        targetTransform = null; // 타겟 끊기
        gameObject.SetActive(false); // 꺼지면 LateUpdate도 멈춤 (성능 절약)
    }

    private IEnumerator AutoHideRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        Hide();
    }
}