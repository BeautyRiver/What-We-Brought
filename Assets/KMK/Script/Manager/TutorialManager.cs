using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    [Header("UI ¿¬°á")]
    public GameObject tutorialPanel;
    public TextMeshProUGUI tutorialText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        tutorialPanel.SetActive(false);
    }

    public void ShowTutorial(string message)
    {
        tutorialText.text = message;
        tutorialPanel.SetActive(true);
    }

    public void HideTutorial()
    {
        tutorialPanel.SetActive(false);
    }
}
