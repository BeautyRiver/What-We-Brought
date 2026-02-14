using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
public enum GameState
{
    Playing,    
    Dialogue,   
    Puzzle,
    CutScene
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
    [field:SerializeField] public GameState CurrentState { get; private set; }
    [field: SerializeField] public PlayerCharacter CurrentCharacter { get; private set; }
        
    [SerializeField] private CharacterSwapManager characterSwapManager;
    private void Awake()
    {
        if (instance == null) { instance = this; }
        else Destroy(gameObject);

        CurrentState = GameState.Playing;
        CurrentCharacter = PlayerCharacter.Human;
    }

    private void Start()
    {
        SoundManager.instance.PlayAmbient("Forest_ambient");
        SoundManager.instance.PlayAmbient("CrowAmbient");
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
                break;
        }
    }   

    public void SwapToHuman()
    {
        characterSwapManager.SwapToHuman();
    }
}

