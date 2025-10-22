using UnityEngine;
using UnityEngine.UIElements;

public class Player_Movement : MonoBehaviour
{
    Vector2 inputVec;
    Rigidbody2D Rigidbody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // 이동 입력 받기

        inputVec = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
    void FixedUpdate()
    {
        // 물리 기반 이동 처리
        Vector2 newVec = inputVec.normalized * Time.fixedDeltaTime * 5f;
        Rigidbody.MovePosition(Rigidbody.position + newVec);
    }

}