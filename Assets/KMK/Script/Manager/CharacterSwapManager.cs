using UnityEngine;
using Unity.Cinemachine;

// 플레이어 타입 열거형 (GameManager 등에서 공용으로 쓴다면 별도 파일로 빼는 것을 추천)
public enum PlayerCharacter
{
    Human,
    Rat
}

public class CharacterSwapManager : MonoBehaviour
{
    [Header("카메라 설정")]
    [SerializeField] private CinemachineCamera virtualCamera; // CM 3.0 기준

    [Header("캐릭터 컨트롤러")]
    [SerializeField] private GameObject humanObject;
    [SerializeField] private GameObject ratObject;


    // 내부 컴포넌트 캐싱
    private PlayerController _humanController;
    private RatController _ratController;

    // 현재 상태
    private bool _isControlHuman = true;

    void Awake()
    {
        // 컴포넌트 미리 찾아두기 (성능 최적화)
        _humanController = humanObject.GetComponent<PlayerController>();
        _ratController = ratObject.GetComponent<RatController>();
    }

    void Start()
    {
        // 게임 시작 시 초기화
        InitCharacterState();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SwapCharacter();
        }
    }

    private void InitCharacterState()
    {
        _isControlHuman = true;

        // 초기 상태: 사람은 켜고 쥐는 끔
        humanObject.SetActive(true);
        ratObject.SetActive(false);

        virtualCamera.Follow = humanObject.transform;

        // GameManager에 알림 (Null 체크 추가)
        if (GameManager.instance != null)
            GameManager.instance.ChangeCharacter(PlayerCharacter.Human);
    }

    private void SwapCharacter()
    {
        _isControlHuman = !_isControlHuman; // 상태 반전

        //// 변신 이펙트
        //if (transformationEffect != null)
        //{
        //    transformationEffect.transform.position = _isControlHuman ? ratObject.transform.position : humanObject.transform.position;
        //    transformationEffect.Play();
        //}

        if (_isControlHuman)
        {            
            // 카메라 타겟 변경
            virtualCamera.Follow = humanObject.transform;

            ratObject.SetActive(false);

            // 컨트롤러 설정
            _ratController.SetControllerState(false);
            _humanController.SetControllerState(true);

            Debug.Log("Mode: Human");
            if (GameManager.instance != null)
                GameManager.instance.ChangeCharacter(PlayerCharacter.Human);
        }
        else
        {
            // 쥐가 사람의 위치로 이동
            ratObject.transform.position = humanObject.transform.position;

            if (ratObject.activeSelf == false)
                ratObject.SetActive(true);

            // 카메라 타겟 변경
            virtualCamera.Follow = ratObject.transform;

            // 컨트롤러 설정
            _ratController.SetControllerState(true);
            _humanController.SetControllerState(false);

            Debug.Log("Mode: Rat");
            if (GameManager.instance != null)
                GameManager.instance.ChangeCharacter(PlayerCharacter.Rat);
        }
    }
}