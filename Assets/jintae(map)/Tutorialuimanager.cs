using UnityEngine;
using TMPro;

public class TutorialUIManager : MonoBehaviour
{
    [Header("튜토리얼 UI 패널들")]
    [SerializeField] private GameObject movementTutorialPanel;
    [SerializeField] private GameObject ratTutorialPanel;
    [SerializeField] private GameObject crowTutorialPanel;

    [Header("튜토리얼 텍스트들")]
    [SerializeField] private TextMeshProUGUI movementText;
    [SerializeField] private TextMeshProUGUI ratText;
    [SerializeField] private TextMeshProUGUI crowText;

    [Header("이동 튜토리얼 자동 숨김 설정")]
    [SerializeField] private bool autoHideMovement = true;
    [SerializeField] private float movementShowTime = 5f;

    [Header("플레이어 추적 설정")]
    [SerializeField] private bool followPlayer = true;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Vector3 offset = new Vector3(100f, 50f, 0f);

    [Header("튜토리얼 전환 설정")]
    [SerializeField] private bool hideOtherTutorials = true;

    private bool isMovementShown = false;
    private bool isRatShown = false;
    private bool isCrowShown = false;
    private float movementTimer = 0f;

    private RectTransform movementRect;
    private RectTransform ratRect;
    private RectTransform crowRect;
    private Camera mainCamera;

    void Awake()
    {
        if (movementText != null)
            movementText.text = "[W][A][S][D] 키로 이동하세요";

        if (ratText != null)
            ratText.text = "좁은 공간입니다. [E] 쥐로 전환";

        if (crowText != null)
            crowText.text = "힌트를 보려면 [F] 까마귀 사용";

        movementRect = movementTutorialPanel.GetComponent<RectTransform>();
        ratRect = ratTutorialPanel.GetComponent<RectTransform>();
        crowRect = crowTutorialPanel.GetComponent<RectTransform>();

        mainCamera = Camera.main;

        HideAllTutorials();
    }

    void Start()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }

        ShowMovementTutorial();
    }

    void Update()
    {
        if (isMovementShown && autoHideMovement)
        {
            movementTimer += Time.deltaTime;

            if (movementTimer >= movementShowTime)
            {
                HideMovementTutorial();
            }
        }

        if (followPlayer && playerTransform != null && mainCamera != null)
        {
            if (isMovementShown && movementTutorialPanel.activeSelf)
            {
                UpdateUIPosition(movementRect);
            }

            if (isRatShown && ratTutorialPanel.activeSelf)
            {
                UpdateUIPosition(ratRect);
            }

            if (isCrowShown && crowTutorialPanel.activeSelf)
            {
                UpdateUIPosition(crowRect);
            }
        }
    }

    void UpdateUIPosition(RectTransform uiRect)
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(playerTransform.position);
        uiRect.position = screenPos + offset;
    }

    public void ShowMovementTutorial()
    {
        if (hideOtherTutorials)
        {
            ratTutorialPanel.SetActive(false);
            crowTutorialPanel.SetActive(false);
        }

        if (!isMovementShown)
        {
            movementTutorialPanel.SetActive(true);
            isMovementShown = true;
            movementTimer = 0f;
        }
    }

    public void HideMovementTutorial()
    {
        movementTutorialPanel.SetActive(false);
    }

    public void ShowRatTutorial()
    {
        if (hideOtherTutorials)
        {
            movementTutorialPanel.SetActive(false);
            crowTutorialPanel.SetActive(false);
        }

        if (!isRatShown)
        {
            ratTutorialPanel.SetActive(true);
            isRatShown = true;
        }
    }

    public void HideRatTutorial()
    {
        ratTutorialPanel.SetActive(false);
    }

    public void ShowCrowTutorial()
    {
        if (hideOtherTutorials)
        {
            movementTutorialPanel.SetActive(false);
            ratTutorialPanel.SetActive(false);
        }

        if (!isCrowShown)
        {
            crowTutorialPanel.SetActive(true);
            isCrowShown = true;
        }
    }

    public void HideCrowTutorial()
    {
        crowTutorialPanel.SetActive(false);
    }

    public void HideAllTutorials()
    {
        movementTutorialPanel.SetActive(false);
        ratTutorialPanel.SetActive(false);
        crowTutorialPanel.SetActive(false);
    }

    public bool IsMovementShown()
    {
        return isMovementShown;
    }

    public bool IsRatShown()
    {
        return isRatShown;
    }

    public bool IsCrowShown()
    {
        return isCrowShown;
    }
}