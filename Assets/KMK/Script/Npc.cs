using UnityEngine;
using UnityEngine.AI;

public class Npc : MonoBehaviour, IInteractable
{
    [Header("대화 설정")]
    [TextArea(3, 5)] public string message = "안녕하세요!";

    [Header("집 설정")]
    [SerializeField] private Transform houseTransform;
    [SerializeField] private float walkableRadius = 5f;

    // ⭐ 추가: 집 중심에서 얼마나 떨어진 곳을 순찰할지 (X, Z)
    [Tooltip("집 위치 기준으로 (x, z) 만큼 떨어진 곳을 중심으로 순찰합니다.")]
    [SerializeField] private Vector3 patrolCenterOffset;

    [Header("대기 설정")]
    [SerializeField] private float minWaitTime = 2f;
    [SerializeField] private float maxWaitTime = 5f;

    private NavMeshAgent agent;
    private Animator animator;
    private bool isMoving;
    private float timer;
    private float currentWaitTime;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = true;
        currentWaitTime = Random.Range(minWaitTime, maxWaitTime);
    }

    private void Update()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            timer += Time.deltaTime;

            if (timer >= currentWaitTime)
            {
                MoveToRandomPos();
                timer = 0f;
                currentWaitTime = Random.Range(minWaitTime, maxWaitTime);
            }
        }

        // 방향 전환 (스프라이트)
        if (agent.velocity.x < -0.1f)
            animator.transform.localScale = new Vector3(1, 1, 1);
        else if (agent.velocity.x > 0.1f)
            animator.transform.localScale = new Vector3(-1, 1, 1);

        isMoving = agent.velocity.sqrMagnitude > 0.01f;
        animator.SetBool("Moving", isMoving);
    }

    private void MoveToRandomPos()
    {
        // 1. 기준점 잡기 (집 위치 + 오프셋 적용)
        Vector3 originPos = houseTransform != null ? houseTransform.position : transform.position;

        // ⭐ 오프셋 적용 (Vector2의 y를 3D의 z로 변환)
        Vector3 offset3D = new Vector3(patrolCenterOffset.x, 0, patrolCenterOffset.y);

        // 최종 중심점 (이 점을 기준으로 원을 그림)
        Vector3 patrolCenter = originPos + offset3D;

        Vector3 finalDestination = Vector3.zero;
        NavMeshHit hit;

        for (int i = 0; i < 30; i++)
        {
            // 2. 최종 중심점(patrolCenter) 기준으로 랜덤 위치 뽑기
            Vector2 randomCircle = Random.insideUnitCircle * walkableRadius;
            Vector3 randomOffset = new Vector3(randomCircle.x, 0, randomCircle.y);

            // 최종 목적지 후보
            Vector3 randomPoint = patrolCenter + randomOffset;

            // 3. NavMesh 위인지 검사
            if (NavMesh.SamplePosition(randomPoint, out hit, 2.0f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
                return;
            }
        }
    }

    // ⭐ 기즈모도 수정: 오프셋이 적용된 실제 순찰 구역을 보여줌
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector3 originPos = houseTransform != null ? houseTransform.position : transform.position;
        Vector3 offset3D = new Vector3(patrolCenterOffset.x, 0, patrolCenterOffset.y);
        Vector3 patrolCenter = originPos + offset3D;

        // 실제 순찰 범위 그리기
        Gizmos.DrawWireSphere(patrolCenter, walkableRadius);

        // 집에서 순찰 중심까지 이어지는 선 그리기 (연결 관계 확인용)
        if (houseTransform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(houseTransform.position, patrolCenter);
        }
    }

    public void Interact()
    {
        UIManager.instance.UpdateTalkText("마을 주민", message);
        UIManager.instance.ShowTalkPanel();
    }
}