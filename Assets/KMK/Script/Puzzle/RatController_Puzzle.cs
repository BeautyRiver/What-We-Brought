using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class RatController_Puzzle : MonoBehaviour
{
    private Animator animator;

    private Vector2 direction;
    public bool isMoving;
    [SerializeField] private float moveDistance = 1f;
    [SerializeField] private float moveDuration = 0.1f;
    [SerializeField] private LayerMask wallLayer; // 벽 레이어
    [SerializeField] private LayerMask pushableLayer;  // 밀 수 있는 오브젝트 레이어

    [Header("오디오 설정")]
    public string footstepSoundGroup = "RatStep";
    public string pushSoundGroup = "PushSound";

    // 플레이어가 마지막으로 바라보 방향
    private Vector2 recentlyDirection = Vector2.right;
    private float prevH;
    private float prevV;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (!(GameManager.instance.CurrentState == GameState.Puzzle))
            return;

        ProcessInput();

        // 디버그용 Ray 그리기
        Vector2 startPos = (Vector2)transform.position;
        Debug.DrawRay(startPos, recentlyDirection * moveDistance, Color.red);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Out"))
        {
            gameObject.GetComponentInParent<RatPuzzle>().StartSwitchCamera(false);
        }
    }

    // 입력 처리
    private void ProcessInput()
    {
        // isMoving이 false일 때만 입력을 받음
        if (isMoving) return;

        direction = Vector2.zero;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 입력값이 0이 아니고 이전값이 0일때만
        if (h != 0 && prevH == 0)
        {
            direction = h > 0 ? Vector2.right : Vector2.left;
        }
        else if (v != 0 && prevV == 0)
        {
            direction = v > 0 ? Vector2.up : Vector2.down;
        }

        prevH = h;
        prevV = v;

        if (direction != Vector2.zero)
        {
            recentlyDirection = direction;
            MoveAndPush();
        }

        float angle = Mathf.Atan2(recentlyDirection.y, recentlyDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        animator.SetBool("isMoving", isMoving);
    }
    private void MoveAndPush()
    {
        Vector2 startPos = (Vector2)transform.position; // 현재 위치
        Vector2 targetPos = startPos + direction * moveDistance; // 목표 위치

        // 벽 체크 Ray
        RaycastHit2D wallHit = Physics2D.Raycast(startPos, direction, moveDistance, wallLayer);

        // 벽이 있으면 이동하지 않음
        if (wallHit.collider != null)
        {
            print("벽임. 이동X.");
            return;
        }
        // pushableObject 오브젝트 체크 Ray
        RaycastHit2D boxHit = Physics2D.Raycast(startPos, direction, moveDistance, pushableLayer);
        if (boxHit.collider != null)
        {
            // 박스를 밀 수 있는지 확인 (박스 뒤 체크)
            Transform box = boxHit.transform;
            Vector2 boxTargetPos = (Vector2)box.position + direction * moveDistance;

            // 벽과 pushableObject 둘다 체크
            LayerMask obstacaleLayer = wallLayer | pushableLayer;

            // pushableObject 뒤 체크 Ray             
            RaycastHit2D boxBehindHit = Physics2D.Raycast(box.position, direction, moveDistance, obstacaleLayer);
            if (boxBehindHit.collider != null)
            {
                // 뒤에 벽이나 상자 있으니까 못밈
                print("뒤에 물체가 존재함");
                return;
            }
            else StartCoroutine(MoveSmooth(box, boxTargetPos, moveDuration)); // pushableObject 이동
        }
        StartCoroutine(MoveSmooth(transform, targetPos, moveDuration)); // 쥐 이동
    }

    // 부드럽게 이동하는 코루틴
    IEnumerator MoveSmooth(Transform obj, Vector2 endPosition, float duration)
    {
        isMoving = true;

        if (obj == transform)
        {
            SoundManager.instance.PlaySound3D(footstepSoundGroup, transform);
        }
        // 움직이는 오브젝트가 '박스'라면 미는 소리 재생 (선택사항)
        else
        {
            SoundManager.instance.PlaySound3D(pushSoundGroup, obj.transform);
        }

        float elapsedTime = 0f;
        Vector2 startPosition = obj.position;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            obj.position = Vector2.Lerp(startPosition, endPosition, elapsedTime / duration);
            yield return null;
        }
        obj.position = endPosition;
        isMoving = false;
    }

    public void ResetRatState()
    {
        // 이동 중이던 상태 취소
        isMoving = false;
        StopAllCoroutines();

        // 바라보는 방향을 기본값으로 초기화
        recentlyDirection = Vector2.right;

        // 입력 값 초기화
        prevH = 0;
        prevV = 0;
        direction = Vector2.zero;

        // 회전 각도 0으로 강제 적용
        transform.rotation = Quaternion.Euler(0, 0, 0);

        // 애니메이션 초기화 (가만히 서있는 상태로)
        if (animator != null) animator.SetBool("isMoving", false);
    }
}
