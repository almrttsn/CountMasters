using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using GameAnalyticsSDK;
using UnityEngine;
using MangoramaStudio.Scripts.Data;

public class AnalyticsManager : BaseManager, IGameAnalyticsATTListener
{
    #region Initialize

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);

        if (!GameManager.Instance.IsAnalyticsEnabled) return;
        FB_Initialize();
        GameAnalytics_Initialize();

        GameManager.EventManager.OnLevelStarted += LevelStarted;
        GameManager.EventManager.OnLevelFinished += LevelFinished;
    }

    #region FB_Initialize
    
    private void FB_Initialize()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            //Handle FB.Init
            FB.Init(() => { FB.ActivateApp(); });
        }
    }
    #endregion

    #region GameAnalytics_Initialize
    private void GameAnalytics_Initialize()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            GameAnalytics.RequestTrackingAuthorization(this);
        }
        else
        {
            GameAnalytics.Initialize();
        }
    }

    public void GameAnalyticsATTListenerNotDetermined()
    {
        GameAnalytics.Initialize();
    }

    public void GameAnalyticsATTListenerRestricted()
    {
        GameAnalytics.Initialize();
    }

    public void GameAnalyticsATTListenerDenied()
    {
        GameAnalytics.Initialize();
    }

    public void GameAnalyticsATTListenerAuthorized()
    {
        GameAnalytics.Initialize();
    }
    #endregion

    private void OnDestroy()
    {
#if SS_ANALYTICS
        GameManager.EventManager.OnLevelStarted -= LevelStarted;
        GameManager.EventManager.OnLevelFinished -= LevelFinished;
#endif
    }

    #endregion

    private void LevelStarted()
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start,
            "level_" + PlayerData.CurrentLevelId.ToString("D3"));
    }

    private void LevelFinished(bool isSuccess)
    {
        if (isSuccess)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete,
                "level_" + PlayerData.CurrentLevelId.ToString("D3"));
        }
        else
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail,
                "level_" + PlayerData.CurrentLevelId.ToString("D3"));
        }
    }
}