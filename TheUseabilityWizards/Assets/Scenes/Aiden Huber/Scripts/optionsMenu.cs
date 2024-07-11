using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class optionsMenu : MonoBehaviour
{
    [SerializeField] AudioMixer audMixer;
    [SerializeField] Slider volSlider;

    float volume;

    public void SetMasterVolume()
    {
        volume = volSlider.value;
        audMixer.SetFloat("MasterVol", Mathf.Log10(volume) * 20);

    }
}
