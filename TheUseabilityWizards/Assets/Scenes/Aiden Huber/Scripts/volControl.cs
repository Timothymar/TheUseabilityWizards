using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class volControl : MonoBehaviour
{
    [SerializeField] AudioMixer volMixer;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    // Start is called before the first frame update
    void Start()
    {
        LoadVolume();
    }

    public void SetVolume()
    {
        SetMasterVolume();
        SetMusicVolume();
        SetSFXVolume();
    }

    protected void LoadVolume()
    {
        LoadMasterVolume();
        LoadMusicVolume();
        LoadSFXVolume();
    }

    public void SetMasterVolume()
    {
        float volume = masterSlider.value;
        volMixer.SetFloat("MasterVol", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    private void LoadMasterVolume()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
            SetMasterVolume();
        }
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        volMixer.SetFloat("MusicVol", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    private void LoadMusicVolume()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
            SetMusicVolume();
        }
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        volMixer.SetFloat("SFXVol", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    private void LoadSFXVolume()
    {
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
            SetSFXVolume();
        }
    }
}