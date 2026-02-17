using Unity.Cinemachine;
using UnityEngine;

public class GateTrigger : EventTrigger
{
    [SerializeField] private CinemachineCamera gateCamera;

    protected override void StartEvent(Collider other)
    {
        // 1. 공통 기능: 카메라는 누가 오든 비춰준다 (원한다면 Player일 때만 비추게 if문 넣어도 됨)
        if (gateCamera != null) gateCamera.Priority = 20;       
    }

    protected override void EndEvent(Collider other)
    {
        if (gateCamera != null) gateCamera.Priority = 0;     
    }
}
