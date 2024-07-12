using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI; 

public class volControl : MonoBehaviour
{
    [SerializeField] AudioMixer volMixer;
    [SerializeField] Slider volSlider;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("masterVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMasterVolume();
        }
    }

    public void SetMasterVolume()
    {
        float volume = volSlider.value;
        volMixer.SetFloat("MasterVol", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("masterVolume", volume);
    }

    private void LoadVolume()
    {
        volSlider.value = PlayerPrefs.GetFloat("musicVolume");

        SetMasterVolume();
    }
}
