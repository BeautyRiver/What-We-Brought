using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("BGM")]
    [SerializeField] private GameObject bgmPrefab;
    [SerializeField] private AudioClip[] bgmClips;
    private AudioSource bgmSource;

    [Header("SFX")]
    [SerializeField] private GameObject sfxPrefab;
    [SerializeField] private AudioClip[] sfxClips;
    private Queue<AudioSource> sfxPool = new Queue<AudioSource>();
    private Dictionary<string, AudioClip> sfxClipDict = new Dictionary<string, AudioClip>();

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

        foreach (var clip in sfxClips)
        {
            if (clip != null)
            {
                string key = clip.name;
                sfxClipDict.Add(key, clip);
            }
        }
    }

    public void PlayBGM(int index)
    {
        if (index < 0 || index >= bgmClips.Length) return;

        if (bgmSource == null)
        {
            GameObject go = Instantiate(bgmPrefab, transform);
            bgmSource = go.GetComponent<AudioSource>();
        }

        if (bgmSource.isPlaying && bgmSource.clip == bgmClips[index]) return;

        bgmSource.clip = bgmClips[index];
        bgmSource.Play();
    }

    public void PlaySFX(string clipName)
    {
        if (!sfxClipDict.TryGetValue(clipName, out AudioClip clip)) return;

        AudioSource source = GetPooledSFX(clipName);
        source.clip = clip;
        source.Play();
    }

    private AudioSource GetPooledSFX(string clipName)
    {
        foreach (var s in sfxPool)
        {
            if (!s.isPlaying) return s;
        }

        GameObject go = Instantiate(sfxPrefab, transform);
        go.name = "SFX_Source: " + clipName;
        AudioSource newSource = go.GetComponent<AudioSource>();
        sfxPool.Enqueue(newSource);
        return newSource;
    }

    public AudioClip GetSFXClip(string clipName)
    {
        if (sfxClipDict.TryGetValue(clipName, out AudioClip clip))
        {
            return clip;
        }
        Debug.LogWarning($"SoundManager: {clipName}을 찾을 수 없습니다!");
        return null;
    }
}
