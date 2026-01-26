using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public GameObject settingsPanel;

    public void OnClickStart()
    {
        // 씬 이름 또는 인덱스로 로드
        SceneManager.LoadScene("Minseo_2nd");  // GameScene 이름과 정확히 일치해야 함
        // 또는: SceneManager.LoadScene(1);   // Scene List에서의 인덱스
    }

    public void OnClickSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void OnClickCloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void OnClickExit()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}