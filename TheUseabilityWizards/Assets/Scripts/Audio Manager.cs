//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class AudioManager : MonoBehaviour
//{
//    [Header("-----Audio Source-----")]
//    [SerializeField] AudioSource bgSource;

//    [Header("-----Audio Clip-----")]
//    public AudioClip background;

//    private void Start()
//    {
//        bgSource.clip = background;
//        bgSource.Play();
//    }
//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("----- Audio Source -----")]
    [SerializeField] private AudioSource bgSource;

    [Header("----- Audio Clips -----")]
    [SerializeField] private List<AudioClip> backgroundTracks;

    private int currentTrackIndex = 0;

    private void Start()
    {
        if (backgroundTracks.Count > 0)
        {
            PlayTrack(currentTrackIndex);
        }
    }

    private void Update()
    {
        if (!bgSource.isPlaying)
        {
            PlayNextTrack();
        }
    }

    private void PlayNextTrack()
    {
        currentTrackIndex = (currentTrackIndex + 1) % backgroundTracks.Count;
        PlayTrack(currentTrackIndex);
    }

    private void PlayTrack(int trackIndex)
    {
        if (trackIndex >= 0 && trackIndex < backgroundTracks.Count)
        {
            bgSource.clip = backgroundTracks[trackIndex];
            bgSource.Play();
        }
    }

    public void PlaySpecificTrack(int trackIndex)
    {
        if (trackIndex >= 0 && trackIndex < backgroundTracks.Count)
        {
            currentTrackIndex = trackIndex;
            PlayTrack(currentTrackIndex);
        }
    }
}