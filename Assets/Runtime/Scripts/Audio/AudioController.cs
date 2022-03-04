using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;
    [SerializeField] AudioSource sfxSource;

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
    }

    public void PlayAudioCue(AudioClip clip, float volume = 1)
    {
        sfxSource.pitch = 1;
        sfxSource.volume = volume;
        sfxSource.PlayOneShot(clip);
    }
}