using System;
using UnityEngine;
using DarkTonic.MasterAudio;

public class PlayerMovement : MonoBehaviour
{
    [Header("설정")]
    public float moveSpeed = 5f;

   
    // Master Audio 그룹 이름과 정확히 일치해야 합니다!
    private const string HUMAN_SOUND = "HumanSound";
    private const string RAT_SOUND = "RatStep";
    private const string TRANSFORM_SOUND = "Rat";

    private Rigidbody rb;
    private Animator anim;
    private Vector3 currentMoveDir;
    private bool isMoving;

    private PlayerCharacter lastCharacter;

    [Tooltip("체크하면: 원래 왼쪽 보는 캐릭터로 인식함")]
    public bool isLeftLookingDefault = false;

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

    void Update()
    {
        if (GameManager.instance.CurrentCharacter == PlayerCharacter.Rat &&
            lastCharacter != PlayerCharacter.Rat)
        {
           
            MasterAudio.PlaySound(TRANSFORM_SOUND);
        }

        lastCharacter = GameManager.instance.CurrentCharacter;
    }

    private string GetCurrentSoundGroup()
    {
        if (GameManager.instance.CurrentCharacter == PlayerCharacter.Rat)
        {
            return RAT_SOUND; 
        }
        return HUMAN_SOUND;  
    }

    public void Move(Vector3 dir)
    {
        currentMoveDir = dir;
        isMoving = true;

        string soundToPlay = GetCurrentSoundGroup();
        MasterAudio.PlaySound3DAtTransform(soundToPlay, transform);
    }

    public void StopMove()
    {
        currentMoveDir = Vector3.zero;
        isMoving = false;

        // [수정됨] 상수 사용
        MasterAudio.PauseSoundGroup(HUMAN_SOUND);
        MasterAudio.PauseSoundGroup(RAT_SOUND);
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
            if (dir.x > 0) anim.transform.localScale = new Vector3(-1, 1, 1);
            else if (dir.x < 0) anim.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            if (dir.x < 0) anim.transform.localScale = new Vector3(-1, 1, 1);
            else if (dir.x > 0) anim.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void SetPlayerMovingForCutScene(bool isMoving)
    {
        this.isMoving = isMoving;
    }
}