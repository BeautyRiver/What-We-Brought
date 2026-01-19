using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerInteraction interaction;

    private Transform cameraTransform;
    private Vector3 moveDir;

    private void Awake()
    {        
        cameraTransform = GetComponent<Transform>();
        movement = GetComponent<PlayerMovement>();
        interaction = GetComponent<PlayerInteraction>();
    }

    private void Update()
    {
        SetMoveDir();
        interaction.HandleInteraction();
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

        movement.Move(dir);
    }

    public void StopMove()
    {
        movement.StopMove();
    }
}
