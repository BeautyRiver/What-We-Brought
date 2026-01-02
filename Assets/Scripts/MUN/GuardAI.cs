using UnityEngine;
using UnityEngine.AI;

public class GuardAI : MonoBehaviour
{
    // =========================================================
    // [1. 상태 정의]
    // =========================================================
    public enum State
    {
        Patrol,    // 순찰
        Alert,     // 추격
        Suspicion, // 의심(배회)
        Return     // 복귀
    }

    // =========================================================
    // [2. 인스펙터 설정 변수들]
    // =========================================================
    [Header("기본 설정")]
    public Transform player;          // 쫓아야 할 플레이어 (Drag & Drop)
    public LayerMask obstacleMask;    // 벽 인식 레이어

    [Header("순찰 설정")]
    public Transform[] waypoints;     // 순찰 경로
    private int waypointIndex = 0;

    [Header("감지 설정")]
    public float viewAngle = 90f;
    public float viewDistance = 10f;

    [Header("AI 행동 설정")]
    public float giveUpDistance = 15f;
    public float wanderRadius = 4f;
    public float wanderDuration = 3f;

    [Header("속도 설정")]
    public float moveSpeed = 3.5f;

    // =========================================================
    // [3. 내부 변수]
    // =========================================================
    private NavMeshAgent agent;
    private Animator anim;            // 애니메이션 제어용 변수 추가
    public State currentState = State.Patrol;

    private float waitTimer = 0f;
    private float wanderTimer = 0f;
    private Vector3 guardPostPosition;

    // 캐릭터의 원래 크기(스케일)를 저장할 변수 (0.8 크기 유지용)
    private Vector3 originalScale;

    // =========================================================
    // [4. 초기화] 
    // =========================================================
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>(); // 애니메이터 가져오기

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        guardPostPosition = transform.position;

        // 현재 설정된 크기(0.8, 0.8, 1)를 기억해둠
        originalScale = transform.localScale;

        if (waypoints != null && waypoints.Length > 0 && waypoints[0] != null)
        {
            agent.SetDestination(waypoints[0].position);
        }
    }

    // =========================================================
    // [5. 메인 루프] 
    // =========================================================
    void Update()
    {
        agent.speed = moveSpeed;

        // 1. 방향 전환 및 애니메이션 처리 (문워킹 수정됨)
        HandleAnimationAndRotation();

        // 2. 플레이어 감지 (추격 중이 아닐 때만)
        if (currentState != State.Alert && CheckForPlayer())
        {
            currentState = State.Alert;
        }

        // 3. 상태별 행동 실행
        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                break;
            case State.Alert:
                Chase();
                break;
            case State.Suspicion:
                Suspicion();
                break;
            case State.Return:
                ReturnToPatrol();
                break;
        }
    }

    // =========================================================
    // [6. 보조 기능] 시각 처리 및 감지 로직
    // =========================================================

    // [수정 완료] 원본 그림이 왼쪽을 보는 경우를 위해 로직 반전
    void HandleAnimationAndRotation()
    {
        // 1. 걷기 애니메이션 (속도가 있으면 true, 멈추면 false)
        bool isMoving = agent.velocity.sqrMagnitude > 0.1f;
        if (anim != null)
        {
            anim.SetBool("isWalking", isMoving);
        }

        // 2. 방향 전환 (좌우 반전)
        // Alert 상태일 때는 플레이어를 바라봄
        if (currentState == State.Alert && player != null)
        {
            if (player.position.x > transform.position.x) // 플레이어가 오른쪽에 있음
            {
                // 원본이 왼쪽을 보므로, 오른쪽을 보게 하려면 뒤집어야(-) 함
                transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            }
            else // 플레이어가 왼쪽에 있음
            {
                // 원본이 왼쪽을 보므로, 그냥 그대로(+) 둠
                transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            }
        }
        // 평소에는 이동 방향을 바라봄
        else
        {
            if (agent.velocity.x > 0.1f) // 오른쪽으로 이동 중
            {
                // 오른쪽을 보게 하려면 뒤집어야(-) 함
                transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            }
            else if (agent.velocity.x < -0.1f) // 왼쪽으로 이동 중
            {
                // 왼쪽을 보게 하려면 그대로(+) 둠
                transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            }
        }
    }

    bool CheckForPlayer()
    {
        if (player == null) return false;

        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float dstToPlayer = Vector3.Distance(transform.position, player.position);

        if (dstToPlayer < viewDistance)
        {
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    return true;
                }
            }
        }
        return false;
    }

    // =========================================================
    // [7. 상태별 행동 함수들] 
    // =========================================================

    void Patrol()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer > 2.0f)
            {
                waypointIndex = (waypointIndex + 1) % waypoints.Length;
                if (waypoints[waypointIndex] != null)
                    agent.SetDestination(waypoints[waypointIndex].position);
                waitTimer = 0f;
            }
        }
    }

    void Chase()
    {
        if (player == null) return;
        agent.SetDestination(player.position);

        if (Vector3.Distance(transform.position, player.position) > giveUpDistance)
        {
            EnterSuspicionState();
        }
    }

    void EnterSuspicionState()
    {
        currentState = State.Suspicion;
        wanderTimer = 0f;
        MoveToRandomLocation();
    }

    void Suspicion()
    {
        wanderTimer += Time.deltaTime;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            MoveToRandomLocation();
        }

        if (wanderTimer > wanderDuration)
        {
            currentState = State.Return;
            int closestIndex = GetClosestWaypointIndex();
            if (closestIndex != -1)
            {
                waypointIndex = closestIndex;
                agent.SetDestination(waypoints[closestIndex].position);
            }
            else
            {
                agent.SetDestination(guardPostPosition);
            }
        }

        if (CheckForPlayer()) currentState = State.Alert;
    }

    void ReturnToPatrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentState = State.Patrol;
            waitTimer = 0f;
        }
        if (CheckForPlayer()) currentState = State.Alert;
    }

    // =========================================================
    // [8. 유틸리티] 
    // =========================================================

    void MoveToRandomLocation()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1))
        {
            agent.SetDestination(hit.position);
        }
    }

    int GetClosestWaypointIndex()
    {
        if (waypoints == null || waypoints.Length == 0) return -1;
        int closestIndex = 0;
        float minDistance = Mathf.Infinity;
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] == null) continue;
            float dist = Vector3.Distance(transform.position, waypoints[i].position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closestIndex = i;
            }
        }
        return closestIndex;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Gizmos.color = Color.red;
        Vector3 leftRay = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward;
        Vector3 rightRay = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward;
        Gizmos.DrawRay(transform.position, leftRay * viewDistance);
        Gizmos.DrawRay(transform.position, rightRay * viewDistance);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, giveUpDistance);
    }
}