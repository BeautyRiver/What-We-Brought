using UnityEngine;
using UnityEngine.UI; // 슬라이더 사용

public class SettingPanel : MonoBehaviour
{
    [Header("UI 연결")]
    public Slider bgmSlider;
    public Slider sfxSlider;

    private void OnEnable()
    {
        // 창이 켜질 때, 현재 저장된 볼륨 값을 슬라이더에 반영
        // (SoundManager가 없다면 1.0f 기본값)
        float currentBGM = ES3.Load<float>("BGM_Volume", 1.0f);
        float currentSFX = ES3.Load<float>("SFX_Volume", 1.0f);

        bgmSlider.value = currentBGM;
        sfxSlider.value = currentSFX;

        // 슬라이더 이벤트 연결 
        bgmSlider.onValueChanged.AddListener(OnBGMChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXChanged);
    }

    private void OnDisable()
    {
        // 닫힐 때 이벤트 연결 해제 (메모리 누수 방지)
        bgmSlider.onValueChanged.RemoveListener(OnBGMChanged);
        sfxSlider.onValueChanged.RemoveListener(OnSFXChanged);
    }

    // 슬라이더를 움직일 때 실행될 함수
    public void OnBGMChanged(float value)
    {
        if (SoundManager.instance != null)
            SoundManager.instance.SetBGMVolume(value);
    }

    public void OnSFXChanged(float value)
    {
        if (SoundManager.instance != null)
            SoundManager.instance.SetSFXVolume(value);
    }
}