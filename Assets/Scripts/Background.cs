using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private BackgroundLayer[] _backgroundLayers;

    private void Awake()
    {
        _backgroundLayers = GetComponentsInChildren<BackgroundLayer>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
