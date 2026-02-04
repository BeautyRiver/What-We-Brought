using UnityEngine;

public class TutorialTrigger : EventTrigger
{
    [Header("이 구역 멘트")]
    [TextArea] public string content;
    [SerializeField] private float showTime = 3f;
    private Collider myCollider;

    private void Awake()
    {
        myCollider = GetComponent<Collider>();
    }

    protected override void StartEvent()
    {
        if (isOneShot)
        {
            // 1회성이면 시간(showTime)을 같이 넘겨줌
            UIManager.instance.ShowInfoPanel(content, showTime);

            // 트리거 자체 기능(충돌체 끄기)은 여기서 처리
            if (myCollider != null) myCollider.enabled = false;
            this.enabled = false;
        }
        else
        {
            // 일반 모드면 시간 없이(0f) 보냄 -> 계속 떠있음
            UIManager.instance.ShowInfoPanel(content);
        }
    }

    protected override void EndEvent()
    {
        if (isOneShot) return;

        UIManager.instance.HideInfoPanel();
    }

    // IEnumerator OneShotRoutine() <- 삭제됨!
}