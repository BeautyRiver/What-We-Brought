using UnityEngine;

public class IconFixer : MonoBehaviour
{
    private Vector3 originalScale;
    private Transform rootParent;

    void Awake()
    {
        // 게임 시작 시 원래 크기 기억
        originalScale = transform.localScale;
        rootParent = transform.parent.parent;
    }

    void LateUpdate()
    {
        // 내 부모가 존재하는지 확인
        if (rootParent != null)
        {
            // 부모의 X 스케일이 음수라면
            if (rootParent.localScale.x < 0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            }
            else
            {
                transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            }
        }
    }
}