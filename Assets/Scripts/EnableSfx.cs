﻿using UnityEngine;

public class EnableSfx : MonoBehaviour
{
    void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.enabled = Settings.IsSfxEnabled;
    }
}
