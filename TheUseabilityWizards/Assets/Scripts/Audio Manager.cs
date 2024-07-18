using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("-----Audio Source-----")]
    [SerializeField] AudioSource bgSource;

    [Header("-----Audio Clip-----")]
    public AudioClip background;

    private void Start()
    {
        bgSource.clip = background;
        bgSource.Play();
    }
}
