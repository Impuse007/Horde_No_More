using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;
    
    public AudioClip[] playerSFX, enemySFX, musicSongs, environmentSFX;
    public AudioSource playerSource, enemySource, musicSource, environmentSource;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
        playerSource.volume = PlayerPrefs.GetFloat("Volume", 1f);
        enemySource.volume = PlayerPrefs.GetFloat("Volume", 1f);
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        environmentSource.volume = PlayerPrefs.GetFloat("Volume", 1f);
    }
    
    public void PlayPlayerSFX(int sfxToPlay)
    {
        playerSource.clip = playerSFX[sfxToPlay];
        playerSource.Play();
    }
    
    public void PlayEnemySFX(int sfxToPlay)
    {
        enemySource.clip = enemySFX[sfxToPlay];
        enemySource.Play();
    }
    
    public void PlayMusic(int musicToPlay)
    {
        musicSource.clip = musicSongs[musicToPlay];
        musicSource.Play();
        Debug.Log("Playing music: " + musicSongs[musicToPlay]);
    }
    
    public void PlayEnvironmentSFX(int sfxToPlay)
    {
        environmentSource.clip = environmentSFX[sfxToPlay];
        environmentSource.Play();
    }
}
