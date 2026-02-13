using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using VInspector;
using DarkTonic.MasterAudio;

public class RatPuzzle : MonoBehaviour
{
    [Header("퍼즐 식별 DATA")]
    public PuzzleData puzzleData;
    public string PuzzleID => puzzleData != null ? puzzleData.puzzleID : "NULL";

    [Header("설정")]
    [ReadOnly] public bool isCleared = false;
    public bool canReEnter = false;

    [Header("퍼즐 초기화 대상")]
    [ReadOnly] public RatPuzzleProp[] puzzleProps;
    private RatController_Puzzle puzzleController;
    [SerializeField] private float fadeDuration = 0.5f;

    [Header("카메라 & UI 연결")]
    private CinemachineCamera puzzleCamera; // 켜질 퍼즐 카메라

    [Header("클리어 후 생성될 오브젝트(아이템)")]
    private GameObject showObj;

    private void Awake()
    {
        puzzleProps = GetComponentsInChildren<RatPuzzleProp>();
        puzzleController = GetComponentInChildren<RatController_Puzzle>();
        puzzleCamera = GetComponentInChildren<CinemachineCamera>();

        if (showObj != null )
            showObj.SetActive(false);
    }

    public void SetShowObj(GameObject obj)
    {
        showObj = obj;
    }
    public void ResetPuzzleProps()
    {
        foreach (var prop in puzzleProps)
        {
            if (prop != null) prop.ResetPosition(); // RatPuzzleProp에 있는 함수 호출
        }

        puzzleController.ResetRatState();

        Debug.Log("쥐 퍼즐 오브젝트들이 초기화되었습니다.");
    }


    public void StartSwitchCamera(bool enterPuzzle)
    {
        StartCoroutine(PuzzleGameManager.instance.SwitchCameraRoutine(enterPuzzle, puzzleCamera, ResetPuzzleProps));

        MasterAudio.PlaySound3DAtTransform("DigSound", transform);
    }

    public void ShowObejct()
    {
        showObj.SetActive(true);
    }
}
