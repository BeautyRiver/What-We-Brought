using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("연결")]
    public PlayerController player; // 플레이어 스크립트 연결

    // 상태 변수 (다른 곳에서 확인용)
    public PlayerCharacter currentCharacter = PlayerCharacter.Human;
    public bool IsGameState { get; private set; } = true;

    private void Awake()
    {
        if (instance == null) { instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }

    private void Start()
    {
        SoundManager.instance.PlayBGM(0);
    }

    public void ChangeCharacter(PlayerCharacter character)
    {
        currentCharacter = character;
    }

    // 플레이어 움직임 상태 설정 함수
    public void SetPlayerMoveState(bool isActive)
    {
        if (isActive == false)
        {
            player.enabled = false; // 플레이어 조작 잠금
            player.StopMove();
        }
        else
        {
            player.enabled = true;
        }
    }
}

