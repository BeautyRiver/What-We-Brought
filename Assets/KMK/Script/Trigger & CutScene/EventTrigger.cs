using UnityEngine;

public abstract class EventTrigger : MonoBehaviour
{
    [Header("트리거 옵션")]
    public bool isOneShot = false;

    // 인스펙터에서 Size 2로 하고 Element 0: Player, Element 1: Rat
    public string[] targetTags = { "Player", "Rat" };

    protected bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isOneShot && hasTriggered) return;

        if (CheckTags(other))
        {
            hasTriggered = true;
            // 누가 들어왔는지(other)를 같이 넘겨줌!
            StartEvent(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (CheckTags(other))
        {
            // 나갈 때도 누가 나갔는지 알려줌
            EndEvent(other);
        }
    }

    private bool CheckTags(Collider other)
    {
        foreach (string tag in targetTags)
        {
            if (other.CompareTag(tag)) return true;
        }
        return false;
    }

    // [중요] 추상 함수 형태 변경: 매개변수 추가
    protected abstract void StartEvent(Collider other);
    protected virtual void EndEvent(Collider other) { }
}