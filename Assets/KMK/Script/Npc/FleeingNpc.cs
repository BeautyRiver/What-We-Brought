using UnityEngine;
using UnityEngine.AI;

// Npc를 상속받음 -> Interact 기능 자동 포함됨!
public class FleeingNpc : Npc
{
    [Header("🏃 도망 설정")]
    [SerializeField] private Transform player;
    [SerializeField] private float detectionRadius = 4f; // 감지 범위
    [SerializeField] private float fleeDistance = 3f;    // 도망 거리
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float runSpeed = 5.0f;

    // 내부 변수
    private NavMeshAgent agent;
    private Animator animator;
    private Vector3 originalPosition;
    private Vector3 originalScale;
    private bool isTalking = false; // 대화 중인지 체크

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = true;

        originalPosition = transform.position;
        originalScale = transform.localScale;

        if (player == null)
        {
            GameObject p = GameObject.Find("Player");
            if (p != null) player = p.transform;
        }
    }

    private void Update()
    {
        // 대화 중이면 움직이지 않음
        if (isTalking) return;
        if (player == null) return;

        // 1. 거리 계산
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 2. AI 로직 (도망 vs 복귀)
        if (distanceToPlayer < detectionRadius)
        {
            FleeFromPlayer();
        }
        else
        {
            ReturnToOriginalPos();
        }

        // 3. 애니메이션 & 방향
        HandleAnimationAndFacing();
    }

    // =========================================================
    // [부모(Npc)의 기능을 덮어쓰기(Override)]
    // =========================================================

    // 대화가 시작되면 멈춤
    protected override void OnDialogueStart()
    {
        isTalking = true;
        agent.isStopped = true; // 이동 정지
        LookAtPlayer();         // 플레이어 바라보기
    }

    // 대화가 끝나면 다시 움직임
    protected override void OnDialogueEnd()
    {
        isTalking = false;
        agent.isStopped = false; // 이동 재개
    }

    // =========================================================
    // [이동 로직들]
    // =========================================================

    private void FleeFromPlayer()
    {
        agent.speed = runSpeed;
        Vector3 dirToPlayer = transform.position - player.position;
        Vector3 fleePos = transform.position + dirToPlayer.normalized * fleeDistance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(fleePos, out hit, 2.0f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    private void ReturnToOriginalPos()
    {
        agent.speed = moveSpeed;
        agent.SetDestination(originalPosition);
    }

    private void HandleAnimationAndFacing()
    {
        bool isMoving = agent.velocity.sqrMagnitude > 0.1f;
        if (animator != null) animator.SetBool("isMoving", isMoving);

        if (isMoving)
        {
            if (agent.velocity.x < -0.1f)
                animator.transform.localScale = new Vector3(1, 1, 1);
            else if (agent.velocity.x > 0.1f)
                animator.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            // 복귀해서 제자리에 멈추면 원래 방향 보기
            if (Vector3.Distance(transform.position, originalPosition) < 0.5f)
            {
                animator.transform.localScale = originalScale;
            }
        }
    }

    private void LookAtPlayer()
    {
        if (animator != null) animator.SetBool("isMoving", false);

        if (player.position.x > transform.position.x)
            animator.transform.localScale = new Vector3(-1, 1, 1);
        else
            animator.transform.localScale = new Vector3(1, 1, 1);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}