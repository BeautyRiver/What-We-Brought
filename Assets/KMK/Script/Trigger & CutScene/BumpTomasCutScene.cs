using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.AI;
using DarkTonic.MasterAudio;


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

    [Header("오디오 설정")]
    public string footstepSoundGroup = "TomasWalk";
    public float stepInterval = 0.4f;
    private float nextStepTime;

    private void Start()
    {
        if (DataManager.instance.currentData.hasSeenTomasCutscene)
        {
            sewerFuctioncol.enabled = true;
            gameObject.SetActive(false);
            return;
        }

        sewerFuctioncol.enabled = false;
    }

    public override void PlayCutscene()
    {
        StartCoroutine(CutsceneSequence());
    }

    IEnumerator CutsceneSequence()
    {
        if (DataManager.instance.currentData.hasSeenTomasCutscene)
            yield break;

        yield return null;
        yield return null;
        if (GameManager.instance.CurrentCharacter == PlayerCharacter.Rat)
        {
            GameManager.instance.SwapToHuman();

            // 1-1. 상태가 Human으로 바뀔 때까지 대기
            yield return new WaitUntil(() => GameManager.instance.CurrentCharacter == PlayerCharacter.Human);
            yield break;
        }

        // 플레이어 조작 잠금
        GameManager.instance.SetGameState(GameState.CutScene);
        // 2️⃣ [수정] 특정 좌표가 아니라, 현재 위치에서 하수구 방향으로 '조금만' 전진
        var pMovement = playerTransform.GetComponent<PlayerMovement>();
        pMovement.SetPlayerMoving(true);

        // 하수구가 플레이어보다 오른쪽에 있는지 왼쪽에 있는지 판별
        // (1 = 오른쪽, -1 = 왼쪽)
        float direction = (walkSewerTransform.position.x > playerTransform.position.x) ? 1f : -1f;
        float walkDistance = 2.0f; // 앞으로 걸어갈 거리 (조절 가능)

        // 목표 지점 = 내 현재 위치 + (방향 * 거리)
        Vector3 targetPos = new Vector3(
            playerTransform.position.x + (direction * walkDistance),
            playerTransform.position.y, // 내 Y값 유지
            playerTransform.position.z
        );

        Transform playerModel = playerTransform.GetChild(0);

        // 목표 지점까지 이동 루프
        // (Distance 체크를 0.1f로 하면 너무 정확히 맞추려다 덜덜거릴 수 있어서 0.05f 정도로 수정하거나 그대로 유지)
        while (Vector3.Distance(playerTransform.position, targetPos) > 0.1f)
        {
            // 이동
            playerTransform.position = Vector3.MoveTowards(
                playerTransform.position,
                targetPos,
                playerWalkSpeed * Time.deltaTime
            );

            // 방향 전환 (진행 방향 보기)
            // 오른쪽으로 가면 1, 왼쪽으로 가면 -1
            if (direction > 0)
                playerModel.localScale = new Vector3(-1, playerTransform.localScale.y, playerTransform.localScale.z); // 모델 좌우 반전에 따라 -1이 오른쪽일 수도 있음 확인 필요
            else
                playerModel.localScale = new Vector3(1, playerTransform.localScale.y, playerTransform.localScale.z);

            yield return null;
        }

        // 도착 후 멈춤
        pMovement.SetPlayerMoving(false);

        // 3️⃣ 도착 후 잠시 대기 (자연스러운 연출)
        yield return new WaitForSeconds(0.5f);

        // (필요 시) 하수구 쪽(충돌 대상)을 바라보게 강제 조정
        // 오른쪽(1)에 하수구가 있다면 오른쪽을 보게 함
        if (direction > 0)
            playerModel.localScale = new Vector3(-1, playerTransform.localScale.y, playerTransform.localScale.z);
        else
            playerModel.localScale = new Vector3(1, playerTransform.localScale.y, playerTransform.localScale.z);

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

            // 사운드 관련
            HandleFootstepSound();

            if (targetPos.x < tomasTransform.position.x)
                tomasModel.localScale = new Vector3(-1, tomasModel.localScale.y, tomasModel.localScale.z);
            else
                tomasModel.localScale = new Vector3(1, tomasModel.localScale.y, tomasModel.localScale.z);

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

            HandleFootstepSound();

            if (targetPos.x < tomasTransform.position.x)
                tomasModel.localScale = new Vector3(-1, tomasModel.localScale.y, tomasModel.localScale.z);
            else
                tomasModel.localScale = new Vector3(1, tomasModel.localScale.y, tomasModel.localScale.z);

            yield return null;
        }
        
        GameManager.instance.SetGameState(GameState.Playing);
        tomasTransform.gameObject.SetActive(false);
        sewerFuctioncol.enabled = true;

        DataManager.instance.SetTomasCutsceneSeen(); // 토마스 컷 씬 봤다고 기록
    }

    private void HandleFootstepSound()
    {
        if (Time.time >= nextStepTime)
        {
            SoundManager.instance.PlaySound3D(footstepSoundGroup, tomasTransform);

            nextStepTime = Time.time + stepInterval;
        }
    }

}
