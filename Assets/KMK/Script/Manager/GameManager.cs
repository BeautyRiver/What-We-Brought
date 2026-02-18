using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
public enum GameState
{
    Playing,    
    Dialogue,   
    Puzzle,
    CutScene,
    Pause
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
    private GameState stateBeforePause;
    [field: SerializeField] public PlayerCharacter CurrentCharacter { get; private set; }
        
    public CharacterSwapManager characterSwapManager;    
    private void Awake()
    {
        if (instance == null) { instance = this; }
        else Destroy(gameObject);

        CurrentState = GameState.Playing;
        stateBeforePause = CurrentState;
        CurrentCharacter = PlayerCharacter.Human;
    }

    private void Start()
    {
        SoundManager.instance.PlayAmbient("Forest_ambient");
        SoundManager.instance.PlayAmbient("CrowAmbient");
        SoundManager.instance.PlayBGM("InGame");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CurrentState != GameState.Pause)
            {
                stateBeforePause = CurrentState;

                UIManager.instance.ShowPausePanel();
                SetGameState(GameState.Pause);
            }
            else
            {
                UIManager.instance.HidePausePanel();
                SetGameState(stateBeforePause);
            }
        }

    }
    public void ChangeCharacter(PlayerCharacter character)
    {
        CurrentCharacter = character;
        characterSwapManager.StopAllCharacter();
    }

    public void SetGameState(GameState newState)
    {
        if (CurrentState == newState) return;

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

