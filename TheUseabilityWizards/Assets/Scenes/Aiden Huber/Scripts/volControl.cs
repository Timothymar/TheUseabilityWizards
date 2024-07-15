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
        if (PlayerPrefs.HasKey("volume"))
        {
            LoadVolume();
        }
        else
        {
            SetVolume();
        }
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
        PlayerPrefs.SetFloat("volume", volume);

        SetMusicVolume();
    }

    private void LoadMasterVolume()
    {
        masterSlider.value = PlayerPrefs.GetFloat("volume");

        SetMasterVolume();
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        volMixer.SetFloat("MusicVol", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("volume", volume);
    }

    private void LoadMusicVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("volume");

        SetMusicVolume();
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        volMixer.SetFloat("SFXVol", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("volume", volume);
    }

    private void LoadSFXVolume()
    {
        masterSlider.value = PlayerPrefs.GetFloat("volume");

        SetSFXVolume();
    }


}
