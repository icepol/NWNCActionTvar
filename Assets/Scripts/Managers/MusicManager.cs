using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    [SerializeField] private float musicOffest = 0;
    
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        
        EventManager.AddListener(Events.LEVEL_START, OnLevelStart);
        EventManager.AddListener(Events.MUSIC_SETTINGS_CHANGED, OnMusicSettingsChanged);
    }

    private void Start()
    {
        if (!Settings.IsMusicEnabled)
            audioSource.Stop();
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(Events.LEVEL_START, OnLevelStart);
        EventManager.RemoveListener(Events.MUSIC_SETTINGS_CHANGED, OnMusicSettingsChanged);
    }

    private void OnLevelStart()
    {
        audioSource.time = musicOffest;
    }

    private void OnMusicSettingsChanged()
    {
        if (Settings.IsMusicEnabled)
            audioSource.Play();
        else
            audioSource.Stop();
    }
}