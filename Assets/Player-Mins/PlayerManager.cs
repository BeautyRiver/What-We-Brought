using System;
using UnityEngine;
using Unity.Cinemachine;

public class PlayerManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject Rat;

    
    public CinemachineCamera virtualCamera;

    // 카메라가 따라갈 실제 게임 오브젝트(Transform)를 연결합니다.
    public Transform playerTransform;
    public Transform ratTransform;

    // 각 캐릭터의 '이동'을 담당하는 스크립트 컴포넌트를 연결합니다.
    public Player_Movement playerMovementScript;
    public Rat_Movement ratMovementScript;

    // 현재 누구를 조작 중인지 상태를 저장하는 변수입니다. (true면 플레이어, false면 쥐)
    private bool isPlayerActive = true;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 시작 시 플레이어 활성화, 쥐는 비활성화
        
        UpdateCharacterState();
    }

    // Update is called once per frame
    void Update()
    {
        // E키가 눌렸는지 매 프레임 검사합니다.
        if (Input.GetKeyDown(KeyCode.E))
        {
            // 현재 상태를 반대로 토글합니다. (true -> false, false -> true)
            isPlayerActive = !isPlayerActive;

            // 변경된 상태를 실제 게임에 적용하는 함수를 호출합니다.
            UpdateCharacterState();
        }
    }

    // 실제 캐릭터 조작 권한과 카메라 타겟을 변경하는 핵심 함수입니다.
    private void UpdateCharacterState()
    {
        if (isPlayerActive)
        {
            // [Case 1] 플레이어 조작 상태일 때

            // 1. 시네머신 카메라가 플레이어를 따라가도록 설정
            virtualCamera.Follow = playerTransform;
            virtualCamera.LookAt = playerTransform;

            // 2. 플레이어의 이동 스크립트는 켜고, 쥐의 이동 스크립트는 끕니다.
            playerMovementScript.enabled = true;
            ratMovementScript.enabled = false;
        }
        else
        {
            // [Case 2] 쥐(Rat) 조작 상태일 때

            // 1. 시네머신 카메라가 쥐를 따라가도록 설정
            virtualCamera.Follow = ratTransform;
            virtualCamera.LookAt = ratTransform;

            // 2. 플레이어의 이동 스크립트는 끄고, 쥐의 이동 스크립트는 켭니다.
            playerMovementScript.enabled = false;
            ratMovementScript.enabled = true;
        }

        // 디버깅을 위한 로그 출력 (콘솔창에서 확인 가능)
        Debug.Log($"조작 대상 변경 완료: {(isPlayerActive ? "Player" : "Rat")}");
    }
}