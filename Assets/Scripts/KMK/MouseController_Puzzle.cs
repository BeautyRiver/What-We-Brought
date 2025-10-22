using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class MouseController_Puzzle : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 direction;
    public bool _isMoving;
    [SerializeField] private float moveDistance = 1f;
    [SerializeField] private float moveDuration = 0.1f;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask boxLayer;

    // 플레이어가 마지막으로 바라보 방향
    private Vector2 recentlyDirection = Vector2.right; 
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // _isMoving이 false일 때만 입력을 받음
        if (_isMoving) return;

        direction = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.W))
        {
            direction = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            direction = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            direction = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            direction = Vector2.right;
        }
        if (direction != Vector2.zero)
        {
            recentlyDirection = direction; 
            Move();
        }

        Vector2 startPos = (Vector2)transform.position;
        Debug.DrawRay(startPos, recentlyDirection * moveDistance, Color.red);
    }

  
    private void Move()
    {        
        Vector2 startPos = (Vector2)transform.position; // 현재 위치
        Vector2 targetPos = startPos + direction * moveDistance; // 목표 위치
        RaycastHit2D wallHit = Physics2D.Raycast(startPos, direction, moveDistance, wallLayer);

        if (wallHit.collider != null)
        {
            print("벽임. 이동X.");
            return;
        }

        

        RaycastHit2D boxHit = Physics2D.Raycast(startPos, direction, moveDistance, boxLayer);
        if (boxHit.collider != null)
        {
            // 박스를 밀 수 있는지 확인 (박스 뒤 체크)
            Transform box = boxHit.transform;
            Vector2 boxTargetPos = (Vector2)box.position + direction * moveDistance;

            LayerMask obstacaleLayer = wallLayer | boxLayer;
            RaycastHit2D boxBehindHit = Physics2D.Raycast(box.position, direction, moveDistance, obstacaleLayer);            
            if (boxBehindHit.collider != null)
            {
                // 뒤에 벽이나 상자 있으니까 못밈
                print("뒤에 물체가 존재함");
                return;
            }
            StartCoroutine(MoveSmooth(box, boxTargetPos, moveDuration)); // 박스 이동
        }        
        StartCoroutine(MoveSmooth(transform, targetPos, moveDuration)); // 쥐 이동
    }
    IEnumerator MoveSmooth(Transform obj, Vector2 endPosition, float duration)
    {
        _isMoving = true;
        float elapsedTime = 0f;
        Vector2 startPosition = obj.position;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            obj.position = Vector2.Lerp(startPosition, endPosition, elapsedTime / duration);
            yield return null;
        }
        obj.position = endPosition;
        _isMoving = false;
    }
 

}
