using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    void Update()
    {
        FlipPlayerByScale();
    }

    void FlipPlayerByScale()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // 이동 입력이 있을 때만 실행
        if (horizontalInput != 0)
        {
            // 현재 크기 값을 가져옵니다.
            Vector3 currentScale = transform.localScale;

            // horizontalInput이 양수(오른쪽)면 1, 음수(왼쪽)면 -1
            // Mathf.Abs를 쓰는 이유: 혹시 모를 크기 오류 방지 (항상 양수에서 시작)
            currentScale.x = Mathf.Abs(currentScale.x) * (horizontalInput > 0 ? -1 : 1);

            // 변경된 크기 적용
            transform.localScale = currentScale;
        }
    }
}