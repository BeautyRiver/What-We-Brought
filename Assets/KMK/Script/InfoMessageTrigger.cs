using TMPro;
using UnityEngine;

public class InfoMessageTrigger : MonoBehaviour
{
    [Header("이 구역에서 띄울 메시지")]
    [TextArea]
    public string message;

    [Header("옵션")]
    public bool showOnlyOnce = false; // 한 번만 보여주고 끝낼지

    [SerializeField] private GameObject tutorialPanel;
    private TextMeshProUGUI tutorialText;
    private bool hasShown = false; // 이미 보여줬는지 체크

    private void Start()
    {
        tutorialPanel.SetActive(false);
        tutorialText = tutorialPanel.GetComponent<TextMeshProUGUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("In");
            if (showOnlyOnce && hasShown) return; // 이미 보여줬으면 패스

            tutorialText.text = message; // 메시지 교체
            tutorialPanel.SetActive(true); // 창 켜기
            hasShown = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialPanel.SetActive(false); // 나가면 창 끄기
        }
    }
}
