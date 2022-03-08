using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource musicSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        sfxSource.loop = false;
        musicSource.loop = false;
    }

    public void PlayAudioCue(AudioClip clip, float volume = 1)
    {
        sfxSource.pitch = 1;
        sfxSource.volume = volume;
        sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip, float volume = 1)
    {
        musicSource.clip = clip;
        musicSource.volume = volume;
        musicSource.Play();
    }
}