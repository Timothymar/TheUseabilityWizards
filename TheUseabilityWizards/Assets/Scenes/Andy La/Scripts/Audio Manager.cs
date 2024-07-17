using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip inGameMusic;
    private float musicVolume = 1.0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        audioSource.loop = true;
        PlayMainMenuMusic();
    }

    public void PlayMainMenuMusic()
    {
        if (audioSource.clip != mainMenuMusic)
        {
            audioSource.clip = mainMenuMusic;
            audioSource.volume = musicVolume;
            audioSource.Play();
        }
    }

    public void PlayInGameMusic()
    {
        if (audioSource.clip != inGameMusic)
        {
            audioSource.clip = inGameMusic;
            audioSource.volume = musicVolume;
            audioSource.Play();
        }
    }

    public void SetVolume(float volume)
    {
        musicVolume = volume;
        audioSource.volume = musicVolume;
    }
}
