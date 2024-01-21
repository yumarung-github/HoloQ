using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class SoundScript : MonoBehaviour
{
    [SerializeField]
    private AudioSource source;
    // Start is called before the first frame update
    void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    public void PlayMusic(AudioClip audio, bool isLoop)
    {
        source.loop = isLoop;
        source.clip = audio;
        source.Play();
    }
    public void PlayMusic()
    {
        source.Play();
    }
    public void StopMusic()
    {
        source.Stop();
    }
    public bool IsPlayingMusic()
    {
        return source.isPlaying;
    }
}
