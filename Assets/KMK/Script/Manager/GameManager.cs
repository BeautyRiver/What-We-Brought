using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
public enum GameState
{
    Playing,    
    Dialogue,   
    Puzzle     
}

public enum PlayerCharacter
{
    Human,
    Rat
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // 상태 변수
    public GameState CurrentState { get; private set; }
    public PlayerCharacter CurrentCharacter { get; private set; }

    [Header("카메라 & UI 연결")]
    [SerializeField] private CinemachineCamera puzzleCamera; // 켜질 퍼즐 카메라

    [SerializeField] private float fadeDuration;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else Destroy(gameObject);

        CurrentState = GameState.Playing;
        CurrentCharacter = PlayerCharacter.Human;
    }

    public void ChangeCharacter(PlayerCharacter character)
    {
        CurrentCharacter = character;
    }

    public void SetGameState(GameState newState)
    {
        CurrentState = newState;

        switch (newState)
        {
            case GameState.Playing:
                // 이동 잠금 해제, UI 보이기
                break;

            case GameState.Dialogue:
                // 이동 잠금, 대화 UI만 허용
                break;

            case GameState.Puzzle:
                // 1. 일반 UI(인벤토리 등) 숨기기
                //UIManager.instance.HideAllPanels();
                break;
        }
    }

    public void StartSwitchCamera(bool enterPuzzle)
    {
        StartCoroutine(SwitchCameraRoutine(enterPuzzle));
    }        
    public IEnumerator SwitchCameraRoutine(bool enterPuzzle)
    {
        // 퍼즐 시작
        if (enterPuzzle)
        {
            GameManager.instance.SetGameState(GameState.Puzzle);

            UIManager.instance.FadeInOut(false, fadeDuration, () => { puzzleCamera.Priority = 20; });
            UIManager.instance.HideItemPanel(); // 아이템 UI 숨기기
            UIManager.instance.HideInfoPanel(); // Info UI 숨기기

            yield return new WaitForSeconds(fadeDuration);

            UIManager.instance.FadeInOut(true, fadeDuration);
        }
        else
        {
            UIManager.instance.FadeInOut(false, fadeDuration, () => { puzzleCamera.Priority = 0; });
            UIManager.instance.ShowItemPanel(); // 아이템 UI 다시 켜기
            UIManager.instance.ShowInfoPanel();
            yield return new WaitForSeconds(fadeDuration);

            UIManager.instance.FadeInOut(true, fadeDuration, () => { GameManager.instance.SetGameState(GameState.Playing); });

        }

    }
}

