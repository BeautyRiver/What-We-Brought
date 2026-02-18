using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("설정")]
    public float moveSpeed = 5f;

    private Rigidbody rb;
    private Animator anim;
    private Vector3 currentMoveDir;
    private bool isMoving;

    [Tooltip("체크하면: 원래 왼쪽 보는 캐릭터로 인식함")]
    public bool isLeftLookingDefault = false;

    [Header("오디오 설정")]
    public string footstepSoundGroup = "Footsteps";

    // [수정] 시간 간격(stepInterval) 대신 거리 간격(stepDistance) 사용
    // 예: 2.0f = 2미터 이동할 때마다 발소리 재생
    // (기존 속도 5 * 0.4초 = 2.0m 였으므로 비슷하게 맞춤)
    public float stepDistance = 2.0f;

    private float accumulatedDistance; // 이동한 거리를 누적할 변수

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        // 게임 시작 시 저장된 방향을 보고 서있게 함
        if (DataManager.instance != null)
        {
            float savedDir = DataManager.instance.currentData.lastFacingDirection;

            // 0이면(초기화 안됨) 오른쪽(1)으로 처리
            if (savedDir == 0) savedDir = 1f;

            // 강제로 그 방향을 보게 만듦
            LookAtDirection(new Vector3(savedDir, 0, 0));
        }
    }

    public void SetMoveDir(Vector3 dir)
    {
        currentMoveDir = dir;
        isMoving = true;
    }

    public void StopMove()
    {
        currentMoveDir = Vector3.zero;
        isMoving = false;

        // [추가] 멈췄을 때 거리 누적 초기화 (다시 움직일 때 바로 소리 안 나게)
        accumulatedDistance = 0f;
    }

    public void Move()
    {
        if (isMoving)
        {
            Vector3 dir = currentMoveDir.normalized * moveSpeed;
            // Unity 6버전이 아니면 rb.velocity 사용
            rb.linearVelocity = new Vector3(dir.x, rb.linearVelocity.y, dir.z);

            LookAtDirection(currentMoveDir);

            // [수정] 발소리 처리 함수 호출
            HandleFootstepSound();
        }
        else
        {
            // 안 움직일 땐 거리 누적 안 함
            accumulatedDistance = 0f;
        }
        anim.SetBool("isMoving", isMoving);
    }

    private void HandleFootstepSound()
    {
        // [핵심 로직 변경]
        // 시간(Time)이 아니라 실제 이동한 거리(Velocity * Time)를 누적함
        // rb.linearVelocity.magnitude : 현재 실제 이동 속도
        accumulatedDistance += rb.linearVelocity.magnitude * Time.deltaTime;

        // 누적된 거리가 설정한 보폭(stepDistance)보다 커지면 소리 재생
        if (accumulatedDistance >= stepDistance)
        {
            SoundManager.instance.PlaySound3D(footstepSoundGroup, transform);

            // 누적 거리 초기화 (0으로 만드는 것보다 값을 빼주는 게 오차를 줄임)
            accumulatedDistance = 0f;
        }
    }

    public void LookAtDirection(Vector3 dir)
    {
        if (isLeftLookingDefault)
        {
            if (dir.x > 0) anim.transform.localScale = new Vector3(-1, 1, 1);
            else if (dir.x < 0) anim.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            if (dir.x < 0) anim.transform.localScale = new Vector3(-1, 1, 1);
            else if (dir.x > 0) anim.transform.localScale = new Vector3(1, 1, 1);
        }

        if (dir.x != 0 && DataManager.instance != null)
        {
            // 오른쪽(양수)이면 1, 왼쪽(음수)이면 -1로 저장
            float facingDir = dir.x > 0 ? 1f : -1f;
            DataManager.instance.currentData.lastFacingDirection = facingDir;
        }
    }

    public void LookAtDirectionWithTarget(Vector3 targetPos)
    {
        // 타겟 위치와 내 위치의 차이를 구함
        float dirX = targetPos.x - transform.position.x;

        Vector3 dir = new Vector3(dirX, 0, 0);
        LookAtDirection(dir);
    }

    public void SetPlayerMoving(bool active)
    {
        this.isMoving = active;   // 애니메이션 재생 여부
        anim.SetBool("isMoving", isMoving);

        // 컷신이 켜질 때, 혹시 남아있을 물리 속도나 방향을 초기화
        if (active)
        {
            rb.linearVelocity = Vector3.zero; // 미끄러짐 방지
            currentMoveDir = Vector3.zero;
        }
    }
}