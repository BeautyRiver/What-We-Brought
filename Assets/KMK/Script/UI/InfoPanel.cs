using TMPro;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    [Header("UI ¿¬°á")]    
    public TextMeshProUGUI tutorialText;

    private void Awake()
    {
        gameObject.SetActive(false);
    }
    public void ShowInfo(string message)
    {
        tutorialText.text = message;
    }

    public void HideInfo()
    {
        tutorialText.text = "";
        gameObject.SetActive(false);
    }
}
