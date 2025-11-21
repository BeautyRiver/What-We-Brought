using UnityEngine;
using UnityEngine.UIElements;

public class Player_Movement : MonoBehaviour
{
    Rigidbody rb;         
    Animator anim;
    Transform cameraTransform;
    private Vector3 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();        
        anim = GetComponent<Animator>();
        cameraTransform = GetComponent<Transform>();
    }

    void Update()
    {
        Vector3 inputVec = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        moveDirection = (camForward * inputVec.z) + (camRight * inputVec.x);
        bool isMoving = inputVec.magnitude > 0;
        transform.localScale = new Vector3(isMoving ? Mathf.Sign(-moveDirection.x) : transform.localScale.x, transform.localScale.y, transform.localScale.z);
        anim.SetBool("Moving", isMoving);
    }

    void FixedUpdate()
    {
        Vector3 newVec = moveDirection * Time.fixedDeltaTime * 5f;
        rb.MovePosition(rb.position + newVec);
    }

}