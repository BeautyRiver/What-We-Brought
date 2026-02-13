using UnityEngine;
using DarkTonic.MasterAudio;

public class RatPuzzleProp : MonoBehaviour
{
    private Vector3 initialPosition; 
    private Quaternion initialRotation; // 태어난 회전값
    private Rigidbody rb; 

    private void Awake()
    {
        // 시작하자마자 내 위치와 회전값 기억
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
    }

    public void ResetPosition()
    {
        // 위치와 회전을 원래대로
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero; 
            rb.angularVelocity = Vector3.zero;
        }
    }
}
