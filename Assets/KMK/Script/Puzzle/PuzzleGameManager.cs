using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class PuzzleGameManager : MonoBehaviour
{
    public static PuzzleGameManager instance;
    [SerializeField] private float fadeDuration = 0.5f;

    private void Awake()
    {
        if (instance == null) { instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }

    // 퍼즐 클리어 처리
    public void ClearPuzzle(string puzzleID)
    {
        // DataManager가 없으면 에러 방지
        if (DataManager.instance == null) return;

        // 이미 깬 건지 데이터 매니저에게 물어봄
        if (!DataManager.instance.currentData.clearedPuzzleIDs.Contains(puzzleID))
        {
            // 1. 데이터 매니저 명단에 추가
            DataManager.instance.currentData.clearedPuzzleIDs.Add(puzzleID);

            // 2. 즉시 저장 (선택 사항)
            DataManager.instance.SaveGame();

            Debug.Log($"[PuzzleManager] 퍼즐 클리어 저장됨: {puzzleID}");
        }
    }

    // 이미 깬 퍼즐인지 확인
    public bool IsPuzzleCleared(string puzzleID)
    {
        if (DataManager.instance == null) return false;

        // 데이터 매니저 명단 확인
        return DataManager.instance.currentData.clearedPuzzleIDs.Contains(puzzleID);
    }

    // =========================================================
    // 연출 부분 
    public IEnumerator SwitchCameraRoutine(bool enterPuzzle, CinemachineCamera puzzleCamera, Action onPuzzleOut = null)
    {
        if (enterPuzzle)
        {
            GameManager.instance.SetGameState(GameState.Puzzle);

            bool fadeOutDone = false;
            LoadAsyncSceneManager.instance.FadeInOut(false, fadeDuration, () => {
                fadeOutDone = true;
            });

            yield return new WaitUntil(() => fadeOutDone);

            puzzleCamera.Priority = 20;
            UIManager.instance.HideItemPanel();
            UIManager.instance.HideInfoPanel();

            LoadAsyncSceneManager.instance.FadeInOut(true, fadeDuration);

            // 캐릭터 상태 쥐로
            UIManager.instance.ChangeCharStateSprite("Rat");
        }
        else
        {
            bool fadeOutDone = false;
            LoadAsyncSceneManager.instance.FadeInOut(false, fadeDuration, () => {
                fadeOutDone = true;
            });

            yield return new WaitUntil(() => fadeOutDone);

            puzzleCamera.Priority = 0;

            if (onPuzzleOut != null)
            {
                onPuzzleOut.Invoke();
            }
            UIManager.instance.ShowItemPanel();

            LoadAsyncSceneManager.instance.FadeInOut(true, fadeDuration, () => {
                GameManager.instance.SetGameState(GameState.Playing);
            });


            // 캐릭터 상태 사람으로
            UIManager.instance.ChangeCharStateSprite("Human");

        }
    }
}