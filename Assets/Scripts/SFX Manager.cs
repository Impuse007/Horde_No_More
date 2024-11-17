using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;
    
    public AudioClip[] playerSFX, enemySFX, music;
    public AudioSource playerSource, enemySource, musicSource;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
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
        musicSource.clip = music[musicToPlay];
        musicSource.Play();
    }
}
