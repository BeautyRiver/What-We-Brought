using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class Npc : MonoBehaviour, IInteractable
{
    [Header("집 설정")]
    [SerializeField] private Transform houseTransform;
    [SerializeField] private float walkableRadius = 10f;

    [Header("대기 설정")]
    [SerializeField] private float minWaitTime;
    [SerializeField] private float maxWaitTime;

    private NavMeshAgent agent;
    private Animator animator;
    private bool isMoving;
    private float timer;
    private float currentWaitTime;

    protected private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }
    protected private void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = true;
        currentWaitTime = Random.Range(minWaitTime, maxWaitTime);
    }

    protected private void Update()
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

        if (agent.velocity.x < -0.1f)
            animator.transform.localScale = new Vector3(1, 1, 1); // 왼쪽 봄
        else if (agent.velocity.x > 0.1f)
            animator.transform.localScale = new Vector3(-1, 1, 1); // 오른쪽 봄

        isMoving = agent.velocity.magnitude > 0.1f;
        animator.SetBool("Moving", isMoving);
    }

    private void MoveToRandomPos()
    {
        // 집이 없으면 그냥 내 자리 기준
        Vector3 origin = houseTransform != null ? houseTransform.position : transform.position;

        Vector3 randomPoint = origin + Random.insideUnitSphere * walkableRadius;
        NavMeshHit hit;

        
        while(!NavMesh.SamplePosition(randomPoint, out hit, 2.0f, NavMesh.AllAreas))
        {
            randomPoint = origin + Random.insideUnitSphere * walkableRadius;
        }

        agent.SetDestination(hit.position);
    }

    public void Interact()
    {
        Debug.Log("NPC와 상호작용 발생!");
    }

    private void OnDrawGizmosSelected()
    {
        if (houseTransform != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(houseTransform.position, walkableRadius);
        }
    }
}
