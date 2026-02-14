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

    private PlayerCharacter lastCharacter;


    [Tooltip("체크하면: 원래 왼쪽 보는 캐릭터로 인식함")]
    public bool isLeftLookingDefault = false;

    [Header("오디오 설정")]
    public string footstepSoundGroup = "Footsteps";
    public float stepInterval = 0.4f;
    private float nextStepTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        if (GameManager.instance != null)
            lastCharacter = GameManager.instance.CurrentCharacter;
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
    }
 
    public void Move()
    {
        if (isMoving)
        {
            Vector3 moveStep = moveSpeed * Time.fixedDeltaTime * currentMoveDir.normalized;
            rb.MovePosition(rb.position + moveStep);
            LookAtDirection(currentMoveDir);

            HandleFootstepSound();
        }
        anim.SetBool("isMoving", isMoving);
    }
    private void HandleFootstepSound()
    {
        if (Time.time >= nextStepTime)
        {
            SoundManager.instance.PlaySound3D(footstepSoundGroup, transform);

            nextStepTime = Time.time + stepInterval;
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