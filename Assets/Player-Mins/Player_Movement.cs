using UnityEngine;
using UnityEngine.UIElements;

public class Player_Movement : MonoBehaviour
{
    Rigidbody rb;         
    Animator anim;
    Transform cameraTransform;
    private Vector3 moveDir;

    void Start()
    {
        rb = GetComponent<Rigidbody>();        
        anim = GetComponentInChildren<Animator>();
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

        moveDir = (camForward * inputVec.z) + (camRight * inputVec.x);
        bool isMoving = inputVec.magnitude > 0;

        float scaleX = moveDir.x == 0 ? transform.localScale.x : -Mathf.Sign(moveDir.x);
        transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);

        anim.SetBool("Moving", isMoving);
    }

    void FixedUpdate()
    {
        Vector3 newVec = moveDir.normalized * Time.fixedDeltaTime * 5f;
        rb.MovePosition(rb.position + newVec);
    }

}