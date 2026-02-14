using UnityEngine;
using Unity.Cinemachine;
using DG.Tweening;
using System; // DOTween 네임스페이스 필수

public class CharacterSwapManager : MonoBehaviour
{
    [Header("카메라 설정")]
    [SerializeField] private CinemachineCamera human_virtualCamera;
    [SerializeField] private CinemachineCamera rat_virtualCamera;

    [Header("캐릭터 오브젝트")]
    [SerializeField] private GameObject humanObject;
    [SerializeField] private GameObject ratObject;

    [Header("연출 설정")]
    [SerializeField] private float ratReturnSpeed = 8f; // 쥐가 돌아오는 속도
    [SerializeField] private string ratSwapSoundGroup = "Rat";
    [SerializeField] private string humanSwapSoundGroup = "HumanReturn"; // 사람 변신 사운드 (필요시 추가)

    // 내부 컴포넌트 캐싱
    private PlayerController _humanController;
    private PlayerMovement _humanMovement;
    private RatController _ratController;
    private PlayerMovement _ratMovement; // 쥐 애니메이션 제어용

    // 상태 관리
    private bool _isControlHuman = true;
    private bool _isSwapping = false; // 변신 중인지 체크 (쿨타임 역할)

    void Awake()
    {
        _humanController = humanObject.GetComponent<PlayerController>();
        _humanMovement = humanObject.GetComponent<PlayerMovement>();
        _ratController = ratObject.GetComponent<RatController>();
        _ratMovement = ratObject.GetComponent<PlayerMovement>();
    }

    void Start()
    {
        InitCharacterState();
    }

    void Update()
    {
        if (GameManager.instance == null || GameManager.instance.CurrentState != GameState.Playing)
            return;

        // 변신 중(쿨타임)이면 입력 무시
        if (_isSwapping) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_isControlHuman) SwapToRat();
            else SwapToHuman();
        }
    }

    private void InitCharacterState()
    {
        _isControlHuman = true;
        _isSwapping = false;

        humanObject.SetActive(true);
        ratObject.SetActive(false);

        // 카메라는 사람 따라가기
        human_virtualCamera.Follow = humanObject.transform;

        SetCameraPriority(true);

        if (GameManager.instance != null)
            GameManager.instance.ChangeCharacter(PlayerCharacter.Human);
    }

    // 사람 -> 쥐 (기존과 거의 동일 + 즉시 변신)
    private void SwapToRat()
    {
        if (_humanController != null) _humanMovement.SetPlayerMoving(false);

        _isControlHuman = false;
        _humanController.enabled = false;

        // 1. 쥐 위치를 사람 위치로 이동
        // (Y축은 쥐의 원래 높이 유지, X/Z는 사람 위치)
        var spawnPos = new Vector3(humanObject.transform.position.x, ratObject.transform.position.y, humanObject.transform.position.z);
        ratObject.transform.position = spawnPos;

        // 2. 오브젝트 활성/비활성
        ratObject.SetActive(true);

        _ratController.enabled = true;

        

        // 3. 카메라 및 게임 상태 변경
        SetCameraPriority(false); // 쥐 카메라 우선

        if (GameManager.instance != null)
        {
            GameManager.instance.ChangeCharacter(PlayerCharacter.Rat);
            _humanController.StopMove();
        }

        Debug.Log("Mode: Rat");
        SoundManager.instance.PlaySound(ratSwapSoundGroup);
    }

    // 쥐 -> 사람 (DOTween 연출 추가)
    public void SwapToHuman()
    {
        // 4. 쥐 애니메이션 켜기 (달리는 모션)
        if (_ratMovement != null) _ratMovement.SetPlayerMoving(true);

        _isSwapping = true; // ⛔ 입력 차단 시작

        // 쥐 컨트롤러 끄기 (⭐ 핵심: 입력을 차단해서 StopMove 호출 방지)
        _ratController.enabled = false;
        
        // 2. 카메라를 먼저 사람 쪽으로 전환! (요청사항 2번)
        SetCameraPriority(true);

        // 3. 거리 계산 및 이동 시간 산출
        float distance = Vector3.Distance(ratObject.transform.position, humanObject.transform.position);
        float duration = distance / ratReturnSpeed;

        // 쥐가 사람을 바라보게 함
        FlipRatToHuman();        

        // 5. DOTween으로 이동 시작 (요청사항 1번)
        ratObject.transform.DOMove(humanObject.transform.position, duration)
            .SetEase(Ease.InOutQuad) // 부드러운 가속/감속
            .OnComplete(() =>
            {
                // 도착 후 실행될 로직 (변신 완료)
                FinishSwapToHuman();
            });
    }

    private void FlipRatToHuman()
    {   
        _ratMovement.LookAtDirectionWithTarget(humanObject.transform.position);
    }

    private void FinishSwapToHuman()
    {
        _isControlHuman = true;
        _isSwapping = false; // ✅ 입력 차단 해제

        // 쥐 정리
        ratObject.SetActive(false);
        _ratController.StopMove();

        _humanController.enabled = true;
        _ratController.enabled = true;

        // 게임 매니저 갱신
        if (GameManager.instance != null)
        {
            GameManager.instance.ChangeCharacter(PlayerCharacter.Human);
        }

        Debug.Log("Mode: Human (Return Complete)");
        // SoundManager.instance.PlaySound(humanSwapSoundGroup); // 사운드 재생
    }

    // 카메라 우선순위 설정 헬퍼 함수
    private void SetCameraPriority(bool isHuman)
    {
        if (isHuman)
        {
            human_virtualCamera.Priority = 10; // 숫자가 높을수록 우선순위 높음
            rat_virtualCamera.Priority = 5;
        }
        else
        {
            human_virtualCamera.Priority = 5;
            rat_virtualCamera.Priority = 10;
        }
    }

    public void StopAllCharacter()
    {
        _humanController.StopMove();
        _ratController.StopMove();
    }
 
}