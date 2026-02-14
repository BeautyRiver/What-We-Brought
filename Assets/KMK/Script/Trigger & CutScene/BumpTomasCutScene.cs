using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class BumpTomasCutScene : CutsceneDirector
{
    [Header("출연진")]
    public Transform playerTransform;
    public Transform tomasTransform;
    public Animator tomasAnimator;

    [Header("토마스 충돌 연출")]
    public Transform walkSewerTransform; // 플레이어가 걸어갈 하수구 근처 위치
    public float playerWalkSpeed = 3f;
    public float tomasWalkSpeed = 6f;
    public float bumpDistance = 1.2f;
    public DialogueData dialogueData;
    public Transform tomasRunTransform;

    [Header("약병 떨어지는 연출")]
    public GameObject bottlePrefab;
    public Transform sewerTransform; // 하수구 구멍
    public Transform spawnPoint; // 생성 위치

    [Header("컷씬 이후")]
    public BoxCollider sewerFuctioncol;

    private void Start()
    {
        sewerFuctioncol.enabled = false;
    }
    public override void PlayCutscene()
    {
        StartCoroutine(CutsceneSequence());
    }

    IEnumerator CutsceneSequence()
    {
        yield return null;
        yield return null;
        if (GameManager.instance.CurrentCharacter == PlayerCharacter.Rat)
            GameManager.instance.SwapCharacter();

        // 플레이어 조작 잠금
        GameManager.instance.SetGameState(GameState.CutScene);

        // 하수구 근처까지 천천히 걸어감..
        var pMovement = playerTransform.GetComponent<PlayerMovement>();
        pMovement.SetPlayerMovingForCutScene(true);

        Vector3 targetPos = new Vector3(
            walkSewerTransform.position.x,
            playerTransform.position.y, // 내 Y값 유지
            walkSewerTransform.position.z
        );

        Transform playerModel = playerTransform.GetChild(0);
        // 목표 지점까지 이동 루프
        while (Vector3.Distance(playerTransform.position, targetPos) > 0.1f)
        {
            // 이동
            playerTransform.position = Vector3.MoveTowards(
                playerTransform.position,
                targetPos,
                playerWalkSpeed * Time.deltaTime
            );

            // 방향 전환 (하수구가 왼쪽에 있으면 왼쪽 봄)
            if (walkSewerTransform.position.x < playerTransform.position.x)
                playerModel.localScale = new Vector3(1, playerTransform.localScale.y, playerTransform.localScale.z);
            else
                playerModel.localScale = new Vector3(-1, playerTransform.localScale.y, playerTransform.localScale.z);

            yield return null;
        }

        // 도착 후 멈춤
        pMovement.SetPlayerMovingForCutScene(false);

        // 도착 후 잠시 대기 (자연스러운 연출을 위해)
        yield return new WaitForSeconds(0.25f);

        playerModel.localScale = new Vector3(-1, playerTransform.localScale.y, playerTransform.localScale.z);

        // 토마스 등장 및 이동
        if (tomasAnimator != null)
        {
            tomasAnimator.SetBool("Moving", true);
            tomasAnimator.SetFloat("MoveSpeed", tomasWalkSpeed);
        }

        targetPos = new Vector3(
            playerTransform.position.x,
            tomasTransform.position.y, // 내 Y값 유지
            playerTransform.position.z
        );


        Transform tomasModel = tomasAnimator.transform;

        while (Vector3.Distance(tomasTransform.position, targetPos) > bumpDistance)
        {
            tomasTransform.position = Vector3.MoveTowards(
                tomasTransform.position,
                targetPos,
                tomasWalkSpeed * Time.deltaTime
                );

            if (targetPos.x < tomasTransform.position.x)
            {
                tomasModel.localScale = new Vector3(-1, tomasModel.localScale.y, tomasModel.localScale.z);
            }
            else
            {
                tomasModel.localScale = new Vector3(1, tomasModel.localScale.y, tomasModel.localScale.z);
            }

            yield return null;
        }

        // 도착 후 정지 (충돌)
        if (tomasAnimator != null)
            tomasAnimator.SetBool("Moving", false);

        // 약병 떨어지는 애니메이션 재생
        ThrowBottleToSewer();

        yield return new WaitForSeconds(1f);        

        // 대화 
        DialogueManager.instance.StartDialogue(dialogueData, EndCutscene, false);
    }

    private void ThrowBottleToSewer()
    {
        Vector3 startPos = spawnPoint.position;

        GameObject bottle = Instantiate(bottlePrefab, startPos, Quaternion.identity);
        bottle.transform.DOJump(sewerTransform.position, 2.5f, 1, 1f)
            .SetEase(Ease.Linear) 
            .OnComplete(() => {
                Debug.Log("약병 하수구 골인!");               
                Destroy(bottle, 0.5f);
            });

        bottle.transform.DORotate(new Vector3(0, 0, 450), 1.3f, RotateMode.FastBeyond360);
    }
    private void EndCutscene()
    {
        StartCoroutine(TomasRun());
        
    }

    IEnumerator OnPlaerController()
    {
        yield return new WaitForSeconds(2f);
    }
    IEnumerator TomasRun()
    {
        if (tomasAnimator != null)
        {
            tomasWalkSpeed += 1f;
            tomasAnimator.SetBool("Moving", true);
            tomasAnimator.SetFloat("MoveSpeed", tomasWalkSpeed);
        }

        Vector3 targetPos = new Vector3(
            tomasRunTransform.position.x,
            tomasTransform.position.y, // 내 Y값 유지
            tomasRunTransform.position.z
        );

        Transform tomasModel = tomasAnimator.transform;
        while (Vector3.Distance(tomasTransform.position, targetPos) > bumpDistance)
        {
            tomasTransform.position = Vector3.MoveTowards(
                tomasTransform.position,
                targetPos,
                tomasWalkSpeed * Time.deltaTime
                );

            if (targetPos.x < tomasTransform.position.x)
            {
                tomasModel.localScale = new Vector3(-1, tomasModel.localScale.y, tomasModel.localScale.z);
            }
            else
            {
                tomasModel.localScale = new Vector3(1, tomasModel.localScale.y, tomasModel.localScale.z);
            }

            yield return null;
        }
        
        GameManager.instance.SetGameState(GameState.Playing);
        tomasTransform.gameObject.SetActive(false);
        sewerFuctioncol.enabled = true;
    }
}
