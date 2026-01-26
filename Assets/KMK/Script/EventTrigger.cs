using UnityEngine;

public abstract class EventTrigger : MonoBehaviour
{
    [Header("트리거 옵션")]
    public bool isOneShot = false; 
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isOneShot && hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true; // 실행됨 표시
            StartEvent();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EndEvent();
        }
    }

    protected abstract void StartEvent();
    protected virtual void EndEvent() { }
}
