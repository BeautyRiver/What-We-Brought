using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class RatController : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerInteraction interaction;
    private Transform cameraTransform;

    private void Awake()
    {
        cameraTransform = GetComponent<Transform>();
        movement = GetComponent<PlayerMovement>();
        interaction = GetComponent<PlayerInteraction>();
    }

    private void Update()
    {      
        // 인간 상태면 리턴
        if (GameManager.instance.CurrentCharacter == PlayerCharacter.Human)
            return;

        if (GameManager.instance.CurrentState == GameState.Playing)
        {
            SetMoveDir();
            interaction.HandleInteraction();
        }
    }
    private void FixedUpdate()
    {
        movement.Move();
    }

    private void SetMoveDir()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if (x == 0 && z == 0)
        {
            movement.StopMove();
            return;
        }

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 dir = (camForward * z) + (camRight * x);

        movement.SetMoveDir(dir);
    }


    public void StopMove()
    {
        movement.StopMove();
    }
}
