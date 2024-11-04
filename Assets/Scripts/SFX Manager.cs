using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip mainMenuMusic;
    public AudioClip gamePlayMusic;
    
    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = mainMenuMusic;
        audioSource.Play();
    }
    
    public void PlayGamePlayMusic()
    {
        audioSource.clip = gamePlayMusic;
        audioSource.Play();
    }
    
    public void PlayMainMenuMusic()
    {
        audioSource.clip = mainMenuMusic;
        audioSource.Play();
    }
}
