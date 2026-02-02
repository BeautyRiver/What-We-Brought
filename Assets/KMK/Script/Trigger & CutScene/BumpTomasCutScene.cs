using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class BumpTomasCutScene : CutsceneDirector
{
    [Header("출연진")]
    public Transform playerModel;
    public Transform tomasTransform;
    public Animator tomasAnimator;

    [Header("토마스 충돌 연출")]
    public float runSpeed = 6f;
    public float bumpDistance = 1.2f;
    public DialogueData dialogueData;
    public Transform tomasRunTransform;

    [Header("약병 떨어지는 연출")]
    public GameObject bottlePrefab;
    public Transform sewerTransform; // 하수구 구멍
    public Transform spawnPoint; // 생성 위치


    public override void PlayCutscene()
    {
        StartCoroutine(CutsceneSequence());
    }

    IEnumerator CutsceneSequence()
    {
        // 플레이어 조작 잠금
        GameManager.instance.SetGameState(GameState.Dialogue);
        playerModel.gameObject.GetComponent<PlayerController>().StopMove();
        // 토마스 등장 및 이동
        if (tomasAnimator != null)
        {
            tomasAnimator.SetBool("Moving", true);
            tomasAnimator.SetFloat("MoveSpeed", runSpeed);
        }

        while (Vector3.Distance(tomasTransform.position, playerModel.position) > bumpDistance)
        {
            tomasTransform.position = Vector3.MoveTowards(
                tomasTransform.position,
                playerModel.position,
                runSpeed * Time.deltaTime
                );

            if (playerModel.position.x < tomasTransform.position.x)
            {
                tomasTransform.localScale = new Vector3(-1, tomasTransform.localScale.y, tomasTransform.localScale.z);
            }
            else
            {
                tomasTransform.localScale = new Vector3(1, tomasTransform.localScale.y, tomasTransform.localScale.z);
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
        DialogueManager.instance.StartDialogue(dialogueData, EndCutscene);
    }

    private void ThrowBottleToSewer()
    {
        Vector3 startPos = spawnPoint.position;

        GameObject bottle = Instantiate(bottlePrefab, startPos, Quaternion.identity);
        bottle.transform.DOJump(sewerTransform.position, 2.0f, 1, 0.8f)
            .SetEase(Ease.Linear) 
            .OnComplete(() => {
                Debug.Log("약병 하수구 골인!");               
                Destroy(bottle, 0.5f);
            });

        bottle.transform.DORotate(new Vector3(0, 0, 360), 0.8f, RotateMode.FastBeyond360);
    }
    private void EndCutscene()
    {
        StartCoroutine(OnPlaerController());
        StartCoroutine(TomasRun());
        Debug.Log("[CutScene Off] - Tomas Bump");
        
    }

    IEnumerator OnPlaerController()
    {
        yield return new WaitForSeconds(2f);
        GameManager.instance.SetGameState(GameState.Playing);
    }
    IEnumerator TomasRun()
    {
        if (tomasAnimator != null)
        {
            runSpeed += 1f;
            tomasAnimator.SetBool("Moving", true);
            tomasAnimator.SetFloat("MoveSpeed", runSpeed);
        }

        while (Vector3.Distance(tomasTransform.position, tomasRunTransform.position) > bumpDistance)
        {
            tomasTransform.position = Vector3.MoveTowards(
                tomasTransform.position,
                tomasRunTransform.position,
                runSpeed * Time.deltaTime
                );

            if (tomasRunTransform.position.x < tomasTransform.position.x)
            {
                tomasTransform.localScale = new Vector3(-1, tomasTransform.localScale.y, tomasTransform.localScale.z);
            }
            else
            {
                tomasTransform.localScale = new Vector3(1, tomasTransform.localScale.y, tomasTransform.localScale.z);
            }

            yield return null;
        }

        Destroy(tomasTransform.gameObject);
    }
}
