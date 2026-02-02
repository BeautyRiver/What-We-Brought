using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class PuzzleGameManager : MonoBehaviour
{
    public static PuzzleGameManager instance;

    // 클리어한 퍼즐의 ID를 모아둠
    private HashSet<string> clearedPuzzleIDs = new HashSet<string>();
    [SerializeField] private float fadeDuration = 0.5f;
    private void Awake()
    {
        if (instance == null) { instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }

    // 퍼즐 클리어 처리
    public void ClearPuzzle(string puzzleID)
    {
        if (!clearedPuzzleIDs.Contains(puzzleID))
        {
            clearedPuzzleIDs.Add(puzzleID);
            Debug.Log($"퍼즐 클리어 기록됨: {puzzleID}");

            // (선택) 여기서 바로 SaveSystem.Save() 호출 가능
        }
    }

    // 이미 깬 퍼즐인지 확인
    public bool IsPuzzleCleared(string puzzleID)
    {
        return clearedPuzzleIDs.Contains(puzzleID);
    }


    public IEnumerator SwitchCameraRoutine(bool enterPuzzle, CinemachineCamera puzzleCamera, Action onPuzzleOut = null)
    {
        if (enterPuzzle)
        {
            // 상태 변경 
            GameManager.instance.SetGameState(GameState.Puzzle);

            // 어두워짐
            bool fadeOutDone = false;
            UIManager.instance.FadeInOut(false, fadeDuration, () => {
                fadeOutDone = true;
            });

            // 페이드 아웃이 끝날 때까지 대기
            yield return new WaitUntil(() => fadeOutDone);

            // 암전 상태에서 카메라 전환 및 UI 숨기기
            puzzleCamera.Priority = 20;
            UIManager.instance.HideItemPanel();
            UIManager.instance.HideInfoPanel();

            // 밝아짐
            UIManager.instance.FadeInOut(true, fadeDuration);
        }
        else
        {
            // 어두워짐
            bool fadeOutDone = false;
            UIManager.instance.FadeInOut(false, fadeDuration, () => {
                fadeOutDone = true;
            });

            yield return new WaitUntil(() => fadeOutDone);

            // 암전 상태에서 정리 작업 
            puzzleCamera.Priority = 0; // 카메라 복귀

            // 전달받은 함수 실행
            if (onPuzzleOut != null)
            {
                onPuzzleOut.Invoke();
            }
            UIManager.instance.ShowItemPanel(); // UI 복구
            UIManager.instance.ShowInfoPanel();

            // 밝아짐 + 상태 복구
            UIManager.instance.FadeInOut(true, fadeDuration, () => {
                GameManager.instance.SetGameState(GameState.Playing);
            });
        }
    }
}