using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerInteraction interaction;
    private Crow crow;
    private Transform cameraTransform;


    private void Awake()
    {        
        cameraTransform = GetComponent<Transform>();
        movement = GetComponent<PlayerMovement>();
        interaction = GetComponent<PlayerInteraction>();
        crow = GetComponent<Crow>();
    }
    
    IEnumerator Start()
    {
        //  다른 매니저(SwapManager 등)와 물리 엔진이 초기화될 때까지 1프레임 대기
        yield return null;

        if (DataManager.instance == null)
        {
            Debug.LogError("DataManager가 없습니다! 프리팹 위치로 초기화됩니다.");
            yield break; 
        }

        Debug.Log("Player초기 세팅 (지연 실행됨)");

        string targetID = DataManager.instance.nextSpawnPointID;
        bool isMovedByPortal = false;

        // 2️⃣ [포탈 이동 시도]
        if (!string.IsNullOrEmpty(targetID))
        {
            Transform spawnPoint = null;

            if (SceneSpawnManager.instance != null)
                spawnPoint = SceneSpawnManager.instance.GetSpawnPoint(targetID);

            if (spawnPoint != null)
            {
                // 🔥 물리 간섭 방지를 위해 잠깐 끄기
                Rigidbody rb = GetComponent<Rigidbody>();
                if (rb != null) rb.isKinematic = true;

                // 위치 이동
                transform.position = new Vector3(spawnPoint.position.x, transform.position.y, spawnPoint.position.z);

                // 🔥 물리 엔진 강제 동기화 (이게 없으면 이동이 무시될 수 있음)
                Physics.SyncTransforms();

                Debug.Log($"스폰포인트 찾기 성공 pos: {transform.position}");

                // 물리 복구
                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.linearVelocity = Vector3.zero; // 미끄러짐 방지
                }

                // ID 사용 후 초기화 (선택 사항 - 무한루프 방지)
                DataManager.instance.nextSpawnPointID = "";
                isMovedByPortal = true;
            }
            else
            {
                Debug.LogError($"[오류] '{targetID}' 스폰 포인트를 못 찾았습니다!");
            }
        }

        // 3️⃣ [이어하기 시도] (포탈 안 탔을 때)
        if (!isMovedByPortal)
        {
            string savedScene = DataManager.instance.currentData.currentSceneName;
            string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            if (savedScene == currentScene)
            {
                Vector3 savedPos = DataManager.instance.currentData.playerPosition;
                if (savedPos != Vector3.zero)
                {
                    Rigidbody rb = GetComponent<Rigidbody>();
                    if (rb != null) rb.isKinematic = true;

                    transform.position = savedPos;
                    Physics.SyncTransforms(); // 동기화

                    if (rb != null)
                    {
                        rb.isKinematic = false;
                        rb.linearVelocity = Vector3.zero;
                    }

                    Debug.Log("DataManager에 저장된 위치로 지정");
                }
            }
            else
            {
                Debug.Log("기존위치 유지 (씬 다름)");
            }
        }
    }

    private void Update()
    {
        // 쥐 상태면 리턴
        if (GameManager.instance.CurrentCharacter == PlayerCharacter.Rat)
            return;

        if (LoadAsyncSceneManager.instance != null && LoadAsyncSceneManager.instance.IsFading)
        {
            movement.StopMove(); 
            return;
        }

        if (GameManager.instance.CurrentState == GameState.Playing)
        {
            SetMoveDir();
            interaction.HandleInteraction();
            crow.HadleCrowAbility();

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
