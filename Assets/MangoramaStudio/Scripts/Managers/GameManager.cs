using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : BaseManager
{
    public bool IsAnalyticsEnabled;
    
    public UIManager UIManager;
    public EventManager EventManager;
    public LevelManager LevelManager;
    public AnalyticsManager AnalyticsManager;

    public InputController Inputs;

    public static GameManager Instance;

    public void Awake()
    {
        Instance = this;
        
        //Application.targetFrameRate = 60;
        EventManager.Initialize(this);
        UIManager.Initialize(this);
        LevelManager.Initialize(this);
        AnalyticsManager.Initialize(this);

        Inputs.Initialize(this);
    }

    private void Start()
    {
        EventManager.StartGame();
    }
}