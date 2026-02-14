using UnityEngine;
using UnityEngine.AI;

public class GuardAI : MonoBehaviour
{
    // =========================================================
    // [1. 상태 정의]
    // =========================================================
    public enum State { Patrol, Alert, Suspicion, Return }

    // =========================================================
    // [2. 설정 변수]
    // =========================================================
    [Header("타겟 및 장애물")]
    public Transform player;
    public LayerMask obstacleMask;
    public LayerMask ratLayer;

    [Header("순찰 경로")]
    public Transform[] waypoints;

    [Header("감지 설정")]
    public float viewAngle = 120f;
    public float playerViewDist = 10f;
    public float ratViewDist = 5f;
    public float visionUpdateRate = 0.2f; // 감지 주기 (0.2초마다 체크)

    [Header("아이콘 설정")]
    public GameObject alertIcon;    // 느낌표 (!)
    public GameObject questionIcon; // 물음표 (?)

    [Header("행동 설정")]
    public float moveSpeed = 3.5f;
    public float runSpeed = 5.0f;
    public float catchDistance = 1.0f;
    public float giveUpDistance = 15f;
    public float wanderRadius = 4f;
    public float wanderDuration = 4f; // 의심 상태 유지 시간

    // =========================================================
    // [3. 내부 변수]
    // =========================================================
    private NavMeshAgent agent;
    private Animator anim;

    // 상태 관리
    public State currentState;
    private Transform currentTarget;

    // 타이머 및 위치
    private float stateTimer = 0f;      // 상태별 시간 체크용
    private float visionTimer = 0f;     // 시야 감지 쿨타임용
    private int waypointIndex = 0;
    private Vector3 startPos;
    private Vector3 originalScale;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        // 2D/3D 하이브리드 필수 설정 (회전 떨림 방지)
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        startPos = transform.position;
        originalScale = transform.localScale;

        if (alertIcon != null) alertIcon.SetActive(false);
        if (questionIcon != null) questionIcon.SetActive(false);

        // 초기 상태 진입
        ChangeState(State.Patrol);
    }

    void Update()
    {
        // 1. 시야 감지 (최적화를 위해 일정 주기로만 실행)
        visionTimer += Time.deltaTime;
        if (currentState != State.Alert && visionTimer >= visionUpdateRate)
        {
            LookForIntruders();
            visionTimer = 0f;
        }

        // 2. 상태별 로직 실행
        switch (currentState)
        {
            case State.Patrol: UpdatePatrol(); break;
            case State.Alert: UpdateAlert(); break;
            case State.Suspicion: UpdateSuspicion(); break;
            case State.Return: UpdateReturn(); break;
        }

        // 3. 애니메이션 & 방향 전환
        UpdateAnimationAndFacing();
    }

    // =========================================================
    // [핵심] 상태 전환 관리자 (가장 중요한 함수!)
    // =========================================================
    void ChangeState(State newState)
    {
        currentState = newState;
        stateTimer = 0f;

        if (alertIcon != null) alertIcon.SetActive(false);
        if (questionIcon != null) questionIcon.SetActive(false);

        switch (currentState)
        {
            case State.Patrol:
                agent.speed = moveSpeed;
                agent.isStopped = false;
                MoveToNextWaypoint();
                break;

            case State.Alert:
                agent.speed = runSpeed;
                agent.isStopped = false;
                if (alertIcon != null) alertIcon.SetActive(true);
                break;

            case State.Suspicion:
                agent.speed = moveSpeed;
                if (questionIcon != null) questionIcon.SetActive(true);
                MoveToRandomPos();
                break;

            case State.Return:
                agent.speed = moveSpeed;
                Vector3 returnPos = (waypoints != null && waypoints.Length > 0)
                                    ? waypoints[waypointIndex].position
                                    : startPos;
                agent.SetDestination(returnPos);
                break;
        }
    }

    // =========================================================
    // [상태별 업데이트 로직] (매 프레임 실행됨)
    // =========================================================

    void UpdatePatrol()
    {
        // 도착했는지 확인
        if (!agent.pathPending && agent.remainingDistance < 0.2f)
        {
            stateTimer += Time.deltaTime;
            if (stateTimer > 2.0f) // 2초 대기 후 이동
            {
                waypointIndex = (waypointIndex + 1) % waypoints.Length;
                MoveToNextWaypoint();
                stateTimer = 0f;
            }
        }
    }

    void UpdateAlert()
    {
        // 타겟 소실 체크
        if (currentTarget == null || !currentTarget.gameObject.activeInHierarchy)
        {
            ChangeState(State.Suspicion);
            return;
        }

        // 타겟 위치로 계속 이동 (추격은 매 프레임 갱신 필요)
        agent.SetDestination(currentTarget.position);

        float dist = Vector3.Distance(transform.position, currentTarget.position);

        // 잡았다!
        if (dist <= catchDistance)
        {
            agent.isStopped = true;
            Debug.Log(currentTarget.name + " 잡힘!");

            if (currentTarget == player)
            {
                // 게임오버 로직 추가하기

                ChangeState(State.Return); // 또는 Patrol
                return;
            }
            else
            {
                // 쥐 잡음 -> 캐릭터 교체 -> 의심 모드
                GameManager.instance.SwapToHuman();
                ChangeState(State.Suspicion);
            }
            return;
        }

        // 놓쳤다...
        if (dist > giveUpDistance)
        {
            ChangeState(State.Suspicion);
        }
    }

    void UpdateSuspicion()
    {
        stateTimer += Time.deltaTime;

        // 전체 의심 시간이 끝나면 복귀
        if (stateTimer > wanderDuration)
        {
            ChangeState(State.Return);
            return;
        }

        // 랜덤 위치 도착했으면 잠시 멈췄다가 다시 이동
        if (!agent.pathPending && agent.remainingDistance < 0.2f)
        {
            // 도착 후 바로 움직이지 않고 0.5초 정도 텀을 줌 (자연스러움)
            // (여기선 간단히 바로 다음 위치로 가지만, 필요하면 별도 타이머 추가 가능)
            MoveToRandomPos();
        }
    }

    void UpdateReturn()
    {
        // ChangeState에서 이미 목적지를 찍었으므로, 여기선 도착만 감시하면 됨
        if (!agent.pathPending && agent.remainingDistance < 0.2f)
        {
            ChangeState(State.Patrol);
        }
    }

    // =========================================================
    // [감지 및 유틸리티]
    // =========================================================

    void LookForIntruders()
    {
        // 1. 플레이어 감지
        if (CanSeeTarget(player, playerViewDist))
        {
            currentTarget = player;
            ChangeState(State.Alert);
            return;
        }

        // 2. 쥐 감지
        Collider[] rats = Physics.OverlapSphere(transform.position, ratViewDist, ratLayer);
        foreach (var rat in rats)
        {
            if (CanSeeTarget(rat.transform, ratViewDist))
            {
                currentTarget = rat.transform;
                ChangeState(State.Alert);
                return;
            }
        }
    }

    bool CanSeeTarget(Transform target, float distLimit)
    {
        if (target == null) return false;

        float dist = Vector3.Distance(transform.position, target.position);
        if (dist > distLimit) return false;

        Vector3 dirToTarget = (target.position - transform.position).normalized;

        // 시선 방향 (좌우 반전 고려)
        Vector3 facing = (transform.localScale.x > 0) ? Vector3.right : Vector3.left;

        if (Vector3.Angle(facing, dirToTarget) < viewAngle / 2f)
        {
            // 눈 높이 보정
            Vector3 eyePos = transform.position + Vector3.up * 0.5f;
            Vector3 targetPos = target.position + Vector3.up * 0.5f;

            if (!Physics.Raycast(eyePos, (targetPos - eyePos).normalized, dist, obstacleMask))
            {
                return true;
            }
        }
        return false;
    }

    void MoveToNextWaypoint()
    {
        if (waypoints != null && waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[waypointIndex].position);
        }
    }

    void MoveToRandomPos()
    {
        Vector3 randomDir = Random.insideUnitSphere * wanderRadius;
        randomDir += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDir, out hit, wanderRadius, 1))
        {
            agent.SetDestination(hit.position);
        }
    }

    void UpdateAnimationAndFacing()
    {
        bool isMoving = agent.velocity.sqrMagnitude > 0.1f;
        if (anim != null) anim.SetBool("isMoving", isMoving);

        // 이동 중이거나 추격 중일 때만 방향 전환 (제자리 떨림 방지)
        if (!isMoving && currentState != State.Alert) return;

        Vector3 lookTarget;
        if (currentState == State.Alert && currentTarget != null)
            lookTarget = currentTarget.position;
        else if (isMoving)
            lookTarget = transform.position + agent.velocity;
        else
            return;

        // X축 차이가 너무 작으면 회전 안 함 (떨림 방지)
        if (Mathf.Abs(lookTarget.x - transform.position.x) < 0.1f) return;

        if (lookTarget.x > transform.position.x)
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    }

    // 디버그 그리기 (선택)
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, playerViewDist);
        Gizmos.color = new Color(1, 0.5f, 0);
        Gizmos.DrawWireSphere(transform.position, ratViewDist);
    }
}