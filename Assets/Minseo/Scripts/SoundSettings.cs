using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [Header("Audio")]
    public AudioMixer audioMixer;     // MainMixer
    public string volumeParameter = "MasterVolume";

    [Header("UI")]
    public Slider volumeSlider;       // Slider_Sound
    public Toggle muteToggle;         // Toggle_Mute (없으면 비워둬도 됨)

    private void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        volumeSlider.value = savedVolume;
        SetVolume(savedVolume);

        if (muteToggle != null)
        {
            bool savedMute = PlayerPrefs.GetInt("MasterMute", 0) == 1;
            muteToggle.isOn = savedMute;
            SetMute(savedMute);
        }
    }

    public void OnVolumeSliderChanged(float value)
    {
        SetVolume(value);
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public void OnMuteToggleChanged(bool isOn)
    {
        SetMute(isOn);
        PlayerPrefs.SetInt("MasterMute", isOn ? 1 : 0);
    }

    private void SetVolume(float value)
    {
        if (value <= 0.0001f) value = 0.0001f;
        float dB = Mathf.Log10(value) * 20f;   // 올바른 볼륨 슬라이더 방식[web:69][web:74][web:76][web:78]
        audioMixer.SetFloat(volumeParameter, dB);
    }

    private void SetMute(bool mute)
    {
        AudioListener.volume = mute ? 0f : 1f; // 전체 음소거[web:74][web:79]
    }
}