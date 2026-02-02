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
        
    [SerializeField] private CharacterSwapManager characterSwapManager;
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
        characterSwapManager.StopAllCharacter();
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
                characterSwapManager.StopAllCharacter();
                // 이동 잠금, 대화 UI만 허용
                break;

            case GameState.Puzzle:
                characterSwapManager.StopAllCharacter();
                // 1. 일반 UI(인벤토리 등) 숨기기
                //UIManager.instance.HideAllPanels();
                break;
        }
    }   

}

