using UnityEngine;
using UnityEngine.UIElements;

public class Player_Movement : MonoBehaviour
{
    Vector2 inputVec;
    Rigidbody2D rb;         
    Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // 같은 오브젝트(또는 자식)에 있는 Animator 컴포넌트를 가져옵니다.
        // 만약 애니메이터가 자식 오브젝트에 있다면 GetComponentInChildren<Animator>()를 써야 합니다.
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 1. 이동 입력 받기
        inputVec = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector3 localVelocity = transform.InverseTransformDirection(rb.linearVelocity);
        float moveX = rb.linearVelocity.x;
        Vector3 currentScale = transform.localScale;

        // 2. 애니메이션 파라미터 업데이트
        // 입력 벡터의 길이(magnitude)가 0보다 크면 이동 중으로 판단합니다.
        if (moveX < -0.1f)
        {
            currentScale.x = Mathf.Abs(currentScale.x);
            transform.localScale = currentScale;
        }
        else if (moveX > 0.1f)
        {
            currentScale.x = -Mathf.Abs(currentScale.x);
            transform.localScale = currentScale;
        }
        bool isMoving = inputVec.magnitude > 0;
        anim.SetBool("Moving_bool", isMoving);
        
    }

    void FixedUpdate()
    {
        // 3. 물리 기반 이동 처리
        Vector2 newVec = inputVec.normalized * Time.fixedDeltaTime * 5f;
        rb.MovePosition(rb.position + newVec);
    }

}