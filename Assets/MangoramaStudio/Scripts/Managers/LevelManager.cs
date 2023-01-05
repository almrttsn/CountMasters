using MangoramaStudio.Scripts.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : BaseManager
{
    [SerializeField] private LevelBehaviour _forcedPlayLevel;

    [SerializeField] private int _startLevelCountAfterLoop;

    [SerializeField] private List<LevelBehaviour> _levelBehaviours;

    private LevelBehaviour _currentLevel;

    #region Initialize

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);

        GameManager.EventManager.OnStartGame += StartGame;

        SROptions.OnLevelInvoked += RetryCurrentLevel;
    }

    private void OnDestroy()
    {
        GameManager.EventManager.OnStartGame -= StartGame;

        SROptions.OnLevelInvoked -= RetryCurrentLevel;
    }

    #endregion

    private void StartGame()
    {
        ClearLevel();

        Resources.UnloadUnusedAssets();

        InputController.IsInputDeactivated = false;

        var currentLevelIndex = PlayerData.CurrentLevelId - 1;

        currentLevelIndex = currentLevelIndex % _levelBehaviours.Count;

        _currentLevel = Instantiate(_levelBehaviours[currentLevelIndex]);

        _currentLevel.Initialize(GameManager);
    }

    private void ClearLevel()
    {
        if (_currentLevel != null)
        {
            Destroy(_currentLevel.gameObject);
        }
    }

    public void ContinueToNextLevel() // For button
    {
        PlayerData.CurrentLevelId += 1;
        StartGame();
    }

    public void RetryCurrentLevel() // For button
    {
        StartGame();
    }

}
