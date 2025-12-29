using UnityEngine;
using UnityEngine.AI;

public class GuardAI : MonoBehaviour
{
    public enum State { Patrol, Alert, Suspicion, Return }

    [Header("기본 설정")]
    public Transform player;
    public LayerMask obstacleMask;

    [Header("스프라이트 설정")]
    public SpriteRenderer spriteRenderer;

    [Header("순찰 설정")]
    public Transform[] waypoints;
    private int waypointIndex = 0;

    [Header("감지 설정")]
    public float viewAngle = 90f;
    public float viewDistance = 10f; // 이 거리 안이면 발견

    [Header("AI 행동 설정 (변경됨)")]
    // ★ [변경] 경비병과 플레이어 사이의 거리가 이 값보다 커지면 추격 포기
    public float giveUpDistance = 15f; 
    
    public float wanderRadius = 4f;      
    public float wanderDuration = 3f;    

    private NavMeshAgent agent;
    public State currentState = State.Patrol; 
    private float waitTimer = 0f;
    private float wanderTimer = 0f;       
    private Vector3 guardPostPosition;    

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        guardPostPosition = transform.position; // 복귀 위치는 여전히 저장 필요

        if (waypoints.Length > 0)
            agent.SetDestination(waypoints[0].position);

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        LookAtTarget();

        // 1. 플레이어 감지 시도 (추격 중이 아닐 때만)
        if (currentState != State.Alert && CheckForPlayer())
        {
            currentState = State.Alert;
        }

        // 2. 상태별 행동
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

    void LookAtTarget()
    {
        if (currentState == State.Alert)
        {
            if (player.position.x > transform.position.x)
                spriteRenderer.flipX = true; 
            else
                spriteRenderer.flipX = false;
        }
        else
        {
            if (agent.velocity.x > 0.1f)
                spriteRenderer.flipX = true;
            else if (agent.velocity.x < -0.1f)
                spriteRenderer.flipX = false;
        }
    }

    bool CheckForPlayer()
    {
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

    // --- 상태별 로직 ---

    void Patrol()
    {
        agent.speed = 2.5f;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer > 2.0f)
            {
                waypointIndex = (waypointIndex + 1) % waypoints.Length;
                agent.SetDestination(waypoints[waypointIndex].position);
                waitTimer = 0f;
            }
        }
    }

    void Chase() 
    {
        agent.speed = 5.0f; 
        agent.SetDestination(player.position);

        // ★ [핵심 변경] 플레이어와 나의 거리 체크
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        // 플레이어가 도망쳐서 거리가 벌어지면 -> 추격 포기 (Suspicion 전환)
        if (distanceToPlayer > giveUpDistance)
        {
            EnterSuspicionState();
        }
    }

    void EnterSuspicionState()
    {
        currentState = State.Suspicion;
        wanderTimer = 0f;
        agent.speed = 3.5f; 
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
            waypointIndex = closestIndex; 
            agent.SetDestination(waypoints[closestIndex].position);
        }

        if (CheckForPlayer())
        {
            currentState = State.Alert;
        }
    }

    void ReturnToPatrol()
    {
        agent.speed = 3.5f;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentState = State.Patrol;
            waitTimer = 0f; 
        }

        if (CheckForPlayer())
        {
            currentState = State.Alert;
        }
    }

    // --- 유틸리티 ---

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
        int closestIndex = 0;
        float minDistance = Mathf.Infinity;

        for (int i = 0; i < waypoints.Length; i++)
        {
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
        // 1. 감지 범위 (노란색)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        // 2. 시야각 (빨간색)
        Gizmos.color = Color.red;
        Vector3 leftRay = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward;
        Vector3 rightRay = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward;
        Gizmos.DrawRay(transform.position, leftRay * viewDistance);
        Gizmos.DrawRay(transform.position, rightRay * viewDistance);

        // ★ 3. 추격 포기 거리 (파란색)
        // 이번에는 기준점이 '플레이어'가 아니라 '나(경비병)'를 기준으로 이 원 밖으로 플레이어가 나가면 포기한다는 의미로 그립니다.
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, giveUpDistance);
    }
}