using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [Header("설정")]
    public float moveSpeed = 5f; // 속도

    private Rigidbody rb;
    private Animator anim;
    private Vector3 currentMoveDir; 
    private bool isMoving;

    [Tooltip("체크하면: 원래 왼쪽 보는 캐릭터로 인식함 / 체크 해제: 원래 오른쪽 보는 캐릭터")]
    public bool isLeftLookingDefault = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();        
        anim = GetComponentInChildren<Animator>();
    }


    public void Move(Vector3 dir)
    {
        currentMoveDir = dir;
        isMoving = true;
    }

    public void StopMove()
    {
        currentMoveDir = Vector3.zero;
        isMoving = false;
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            Vector3 moveStep = currentMoveDir.normalized * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + moveStep);
            LookAtDirection(currentMoveDir);
        }
        anim.SetBool("Moving", isMoving);
    }

    private void LookAtDirection(Vector3 dir)
    {
        if (isLeftLookingDefault)
        {
            if (dir.x > 0)
                anim.transform.localScale = new Vector3(-1, 1, 1);
            else if (dir.x < 0)
                anim.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            if (dir.x < 0)
                anim.transform.localScale = new Vector3(-1, 1, 1);
            else if (dir.x > 0)
                anim.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void SetPlayerMovingForCutScene(bool isMoving)
    {
        this.isMoving = isMoving;
    }
}