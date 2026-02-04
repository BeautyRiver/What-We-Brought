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
    public Transform player;          // 쫓아야 할 플레이어
    public LayerMask obstacleMask;    // 벽 인식 레이어

    [Header("순찰 설정")]
    public Transform[] waypoints;     // 순찰 경로
    private int waypointIndex = 0;

    [Header("감지 설정")]
    public float viewAngle = 90f;
    public float viewDistance = 10f;
    public GameObject alertIcon;      // [추가] 발견 시 머리 위에 뜰 느낌표(!) 오브젝트

    [Header("AI 행동 설정")]
    public float giveUpDistance = 15f;
    public float catchDistance = 1.0f; // [추가] 플레이어를 잡는 거리
    public float wanderRadius = 4f;
    public float wanderDuration = 3f;

    [Header("속도 설정")]
    public float moveSpeed = 3.5f;

    // =========================================================
    // [3. 내부 변수]
    // =========================================================
    private NavMeshAgent agent;
    private Animator anim;
    public State currentState = State.Patrol;

    private float waitTimer = 0f;
    private float wanderTimer = 0f;
    private Vector3 guardPostPosition;

    // 캐릭터의 원래 크기 및 회전값 저장용
    private Vector3 originalScale;
    private Quaternion originalRotation; // [추가] 초기 회전값 저장용

    // =========================================================
    // [4. 초기화] 
    // =========================================================
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        agent.updateRotation = false;
        agent.updateUpAxis = true;

        guardPostPosition = transform.position;
        originalScale = transform.localScale;

        // [추가] 게임 시작 시 설정해둔 회전값(기울기 등)을 기억함
        originalRotation = transform.rotation;

        // [추가] 시작할 때 느낌표 끄기
        if (alertIcon != null) alertIcon.SetActive(false);

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

        HandleAnimationAndRotation();

        // [2. 플레이어 감지 로직]
        if (currentState != State.Alert && CheckForPlayer())
        {
            EnterAlertState(); // [변경] 상태 전환 함수로 분리
        }

        // [3. 상태별 행동 실행]
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

    // [추가] X축 각도가 0이 되는 문제 해결을 위해 LateUpdate에서 회전 고정
    void LateUpdate()
    {
        // NavMeshAgent가 멋대로 회전을 바꾸지 못하게 초기 회전값으로 강제 고정
        // (만약 Z축 회전도 막고 싶다면 이 코드가 유효함. 2D 게임이면 보통 Z축 회전만 필요하므로, 
        // 3D 뷰에서 기울인 X축을 유지하려면 이 방식이 필수)
        transform.rotation = originalRotation;
    }

    // =========================================================
    // [6. 보조 기능] 
    // =========================================================

    void HandleAnimationAndRotation()
    {
        bool isMoving = agent.velocity.sqrMagnitude > 0.1f;
        if (anim != null)
        {
            anim.SetBool("isWalking", isMoving);
        }

        // 방향 전환 (Scale.x 반전)
        // Alert 상태일 때는 플레이어를 바라봄
        if (currentState == State.Alert && player != null)
        {
            if (player.position.x > transform.position.x) // 플레이어가 오른쪽
            {
                transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            }
            else // 플레이어가 왼쪽
            {
                transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            }
        }
        else // 평소에는 이동 방향
        {
            if (agent.velocity.x > 0.1f) // 오른쪽 이동
            {
                transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            }
            else if (agent.velocity.x < -0.1f) // 왼쪽 이동
            {
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
            // 2D 게임의 경우 transform.forward 대신 transform.right나 up을 써야 할 수도 있음 (설정에 따라 다름)
            // 일단 기존 코드를 유지하되, Z축 이슈가 있다면 Physics.Raycast 대신 Physics2D 사용 고려 필요
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

    // [추가] 추격 상태 진입 (느낌표 켜기)
    void EnterAlertState()
    {
        currentState = State.Alert;
        if (alertIcon != null) alertIcon.SetActive(true);
    }

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
        float dist = Vector3.Distance(transform.position, player.position);

        // [추가] 플레이어 잡힘 이벤트
        if (dist <= catchDistance)
        {
            Debug.Log("플레이어 잡힘! (Game Over)");
            // 여기서 GameManager.instance.GameOver(); 등을 호출하면 됨
            // 잡힌 후 가드가 멈추게 하려면:
            agent.isStopped = true;
            return;
        }

        // 추격 포기
        if (dist > giveUpDistance)
        {
            EnterSuspicionState();
        }
    }

    void EnterSuspicionState()
    {
        currentState = State.Suspicion;

        // [추가] 추격이 끝났으니 느낌표 끄기
        if (alertIcon != null) alertIcon.SetActive(false);

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
            // 느낌표는 이미 꺼져 있음
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

        if (CheckForPlayer()) EnterAlertState();
    }

    void ReturnToPatrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentState = State.Patrol;
            waitTimer = 0f;
        }
        if (CheckForPlayer()) EnterAlertState();
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

        // 시야각 그리기 (간략화)
        Vector3 leftRay = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward;
        Vector3 rightRay = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, leftRay * viewDistance);
        Gizmos.DrawRay(transform.position, rightRay * viewDistance);

        // 추격 포기 거리
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, giveUpDistance);

        // [추가] 잡히는 거리 표시
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, catchDistance);
    }
}