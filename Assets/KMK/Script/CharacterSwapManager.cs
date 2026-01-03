using UnityEngine;
using Unity.Cinemachine; 

public class CharacterSwapManager : MonoBehaviour
{
    [Header("카메라 설정")]
    [SerializeField] private CinemachineCamera virtualCamera;

    [Header("캐릭터 컨트롤러 (뇌)")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerController ratController;

    private bool isPlayerControl = true; // 현재 사람이 주인공인가

    void Start()
    {
        UpdateCharacterState();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isPlayerControl = !isPlayerControl; // 상태 뒤집기 (Toggle)
            UpdateCharacterState();
        }
    }

    private void UpdateCharacterState()
    {
        if (isPlayerControl)
        {
            // 사람 조작            
            playerController.enabled = true;
            ratController.enabled = false;

            virtualCamera.Follow = playerController.transform;
            ratController.GetComponent<PlayerMovement>().StopMove();
            //virtualCamera.LookAt = playerController.transform; 

            Debug.Log("Mode: Human");
        }
        else
        {
            // 쥐 조작

            playerController.enabled = false;
            ratController.enabled = true;

            virtualCamera.Follow = ratController.transform;
            playerController.GetComponent<PlayerMovement>().StopMove();


            Debug.Log("Mode: Rat");
        }
    }
}