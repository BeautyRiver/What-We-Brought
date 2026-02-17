using UnityEngine;

public class PausePanel : MonoBehaviour
{
    public void ShowPausePanel()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void HidePausePanel()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    public void OnClickExit()
    {
        Time.timeScale = 1f;

        LoadAsyncSceneManager.instance.FadeToScene("Title");
    }
}
