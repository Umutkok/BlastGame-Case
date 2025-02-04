using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour
{
    public Text fpsText; // UI Text nesnesi atanmalı

    void Start()
    {
     QualitySettings.vSyncCount = 0;
     Application.targetFrameRate = 10000;
    }

    void Update()
    {
        float frameTimeMs = Time.unscaledDeltaTime * 1000f; // Milisaniye cinsinden hesapla
        float fps = 1f / Time.unscaledDeltaTime; // FPS hesapla
        fpsText.text = $"Frame {frameTimeMs:F2} ms\nFPS: {fps:F0}";
    }
}