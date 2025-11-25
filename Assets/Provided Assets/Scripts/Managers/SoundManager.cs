using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] soundEffects;
    public AudioSource source;

    public static SoundManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    public void Play(Enums.SoundEffects soundEffect)
    {
        source.PlayOneShot(soundEffects[(int)soundEffect]);
    }
    public void Play(AudioSource audio)
    {
        audio.Play();
    }
    
    public void Stop(AudioSource audio)
    {
        audio.Stop();
    }
}
