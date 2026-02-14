using DarkTonic.MasterAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
 
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

    public void PlayBGM(string playlistName)
    {
        // Master Audio의 Playlist Controller를 찾아서 재생
        MasterAudio.TriggerPlaylistClip(playlistName);
    }

}
