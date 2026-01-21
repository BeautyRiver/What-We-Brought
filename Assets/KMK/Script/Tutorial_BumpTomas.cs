using System;
using System.Collections;
using UnityEngine;

public class Tutorial_BumpTomas : CutsceneDirector
{
    [Header("출연진")]
    public PlayerController playerController;
    public Transform playerModel;
    public Transform tomasTransform;
    public Animator tomasAnimator;

    [Header("연출")]
    public float runSpeed = 6f;
    public float bumpDistance = 1.2f;
    public DialogueData dialogueData;

    public override void PlayCutscene()
    {
        StartCoroutine(CutsceneSequence());
    }

    IEnumerator CutsceneSequence()
    {
        // 플레이어 조작 잠금
        playerController.enabled = false;
        Debug.Log("[CutScene On] - Tomas Bump");
        playerController.StopMove();

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

        yield return new WaitForSeconds(0.5f);

        // 대화 
        DialogueManager.instance.StartDialogue(dialogueData, EndCutscene);
    }

    private void EndCutscene()
    {
        playerController.enabled = true;
        Debug.Log("[CutScene Off] - Tomas Bump");
    }
}
