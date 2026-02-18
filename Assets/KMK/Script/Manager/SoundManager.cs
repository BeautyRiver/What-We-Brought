using DarkTonic.MasterAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Bus 이름")]
    public string sfxBusName = "SFX";
    public string force2DBusName = "Force2D";

    [Header("현재 재생 중인 BGM")]
    public string currentBGMName = "";

    [Header("기본 값")]
    public float defaultValue = 0.75f;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }      
    }

    private void Start()
    {
        InitVolume();
    }
    private void InitVolume()
    {
        // 1. ES3에서 저장된 값 불러오기
        float savedBGM = ES3.Load<float>("BGM_Volume", defaultValue);
        float savedSFX = ES3.Load<float>("SFX_Volume", defaultValue);

        // 2. Master Audio에 바로 적용
        SetBGMVolume(savedBGM);
        SetSFXVolume(savedSFX);
    }

    // UI 슬라이더에서 호출할 함수 (BGM)
    public void SetBGMVolume(float volume)
    {
        // Master Audio 버스 볼륨 조절
        MasterAudio.PlaylistMasterVolume = volume;
        // 값 변경될 때마다 저장 
        ES3.Save("BGM_Volume", volume);
    }

    // UI 슬라이더에서 호출할 함수 (SFX)
    public void SetSFXVolume(float volume)
    {
        MasterAudio.SetBusVolumeByName(sfxBusName, volume);
        MasterAudio.SetBusVolumeByName(force2DBusName, volume);

        ES3.Save("SFX_Volume", volume);
    }
    [Button]
    public void ResetSoundSettings()
    {
        // 사운드 매니저 값을 기본값(1.0)으로 돌려놓기        
        SetBGMVolume(defaultValue);
        SetSFXVolume(defaultValue);

        Debug.Log("사운드 설정이 초기화됐어!");
    }
    public void PlaySound(string soundName)
    {
        MasterAudio.PlaySound(soundName);        
    }

    public void PlaySound3D(string soundName, Transform transform)
    {
        MasterAudio.PlaySound3DAtTransform(soundName, transform);
    }

    public void PauseSoundGroup(string soundName)
    {
        MasterAudio.PauseSoundGroup(soundName);
    }

    public void UnpauseSoundGroup(string soundName)
    {
        MasterAudio.UnpauseSoundGroup(soundName);
    }

    public void PlayAmbient(string soundGroupName)
    {
        // 앰비언트는 보통 2D로(어디서나 들리게) 재생함
        MasterAudio.PlaySoundAndForget(soundGroupName);
    }

    public void StopAmbient(string soundGroupName)
    {
        MasterAudio.StopAllOfSound(soundGroupName);
    }

    public void PlayBGM(string bgmName)
    {
        // 1. 이미 그 노래가 나오고 있다면? -> 아무것도 안 함 (끊김 방지)
        if (currentBGMName == bgmName) return;

        // 2. 다른 노래라면? -> 교체 시작
        currentBGMName = bgmName;

        // Master Audio의 Playlist Controller를 찾아서 재생
        MasterAudio.ChangePlaylistByName(bgmName, true);
    }
    

}
