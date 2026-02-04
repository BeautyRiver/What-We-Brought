using Unity.Cinemachine;
using UnityEngine;

public class GateTrigger : EventTrigger
{
    private CinemachineCamera gateCamera;

    private void Awake()
    {
        gateCamera = GetComponentInChildren<CinemachineCamera>();    
    }

    protected override void StartEvent()
    {
        gateCamera.Priority = 20;
    }

    protected override void EndEvent()
    {
        gateCamera.Priority = 0;
    }
}
