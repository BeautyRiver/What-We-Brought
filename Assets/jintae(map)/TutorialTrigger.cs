using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [Header("어떤 튜토리얼을 보여줄지 선택")]
    [SerializeField] private TutorialType tutorialType;

    [Header("한 번만 보여줄지 선택")]
    [SerializeField] private bool showOnce = true;

    [Header("몇 초 후에 사라질지 (0일 시 지속)")]
    [SerializeField] private float autoHideTime = 3f;

    private TutorialUIManager tutorialManager;
    private bool hasShown = false;

    public enum TutorialType
    {
        Movement,
        Rat,
        Crow
    }

    void Start()
    {
        tutorialManager = FindObjectOfType<TutorialUIManager>();

        if (tutorialManager == null)
        {
            Debug.LogError("TutorialUIManager를 찾을 수 없습니다");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (showOnce && hasShown)
            {
                return;
            }

            ShowTutorial();
            hasShown = true;

            if (autoHideTime > 0f)
            {
                Invoke("HideTutorial", autoHideTime);
            }
        }
    }

    void ShowTutorial()
    {
        if (tutorialManager == null)
            return;

        if (tutorialType == TutorialType.Movement)
        {
            tutorialManager.ShowMovementTutorial();
        }
        else if (tutorialType == TutorialType.Rat)
        {
            tutorialManager.ShowRatTutorial();
        }
        else if (tutorialType == TutorialType.Crow)
        {
            tutorialManager.ShowCrowTutorial();
        }
    }

    void HideTutorial()
    {
        if (tutorialManager == null)
            return;

        if (tutorialType == TutorialType.Movement)
        {
            tutorialManager.HideMovementTutorial();
        }
        else if (tutorialType == TutorialType.Rat)
        {
            tutorialManager.HideRatTutorial();
        }
        else if (tutorialType == TutorialType.Crow)
        {
            tutorialManager.HideCrowTutorial();
        }
    }
}