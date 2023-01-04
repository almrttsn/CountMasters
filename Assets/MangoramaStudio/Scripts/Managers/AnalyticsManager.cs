using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MangoramaStudio.Scripts.Data;
using MatchinghamGames.SherlockModule;
using MatchinghamGames.SherlockModule.Services.GameAnalytics.Runtime;
using MatchinghamGames.SherlockModule.Services.Adjust.Runtime;
using MatchinghamGames.SherlockModule.Services.Firebase.Runtime;
using com.adjust.sdk;

namespace MangoramaStudio.Scripts.Managers
{
    public class AnalyticsManager : CustomBehaviour
    {
        public override void Initialize(GameManager gameManager)
        {
            base.Initialize(gameManager);


            GameManager.EventManager.OnStartGame += GameStarted;
            GameManager.EventManager.OnLevelStarted += LevelStarted;
            GameManager.EventManager.OnLevelFinished += LevelFinished;
            GameManager.EventManager.OnLevelRestarted += LevelRestarted;
        }

        private void OnDestroy()
        {
            GameManager.EventManager.OnStartGame -= GameStarted;
            GameManager.EventManager.OnLevelStarted -= LevelStarted;
            GameManager.EventManager.OnLevelFinished -= LevelFinished;
            GameManager.EventManager.OnLevelRestarted -= LevelRestarted;
        }
        public void SendAdjustIAPEvents(string productIdentifierKey, float price, string transaction, string isoCurrencyCode)
        {
            var adjustAnalyticsService = Sherlock.Instance.GetService<IAdjustAnalyticsService>();
            var token = SherlockUtility.GetToken(productIdentifierKey, adjustAnalyticsService);

            AdjustEvent adjustEvent = new AdjustEvent(token);
            adjustEvent.setRevenue((float)(0.7f * price), isoCurrencyCode);
            adjustEvent.setTransactionId(transaction);
            Adjust.trackEvent(adjustEvent);

            SendIAPTotalEvents(productIdentifierKey, price, transaction, isoCurrencyCode);
        }

        public void SendIAPTotalEvents(string productIdentifierKey, float price, string transaction, string isoCurrencyCode)
        {
            var adjustAnalyticsService = Sherlock.Instance.GetService<IAdjustAnalyticsService>();
            var token = SherlockUtility.GetToken(productIdentifierKey, adjustAnalyticsService);

            AdjustEvent adjustEvent = new AdjustEvent(token);
            adjustEvent.setRevenue((double)(0.7f * price), isoCurrencyCode);
            Adjust.trackEvent(adjustEvent);
        }

        public void TrackAdjustEvent(string eventName)
        {
            Sherlock.Instance.GetService<IGameAnalyticsService>().SendDesignEvent(eventName);
            var adjustAnalyticsService = Sherlock.Instance.GetService<IAdjustAnalyticsService>();
            adjustAnalyticsService.SendCustom(SherlockUtility.GetToken(eventName, adjustAnalyticsService));
        }
        public void TrackFirebaseEvent(string eventName, bool isWithParameter = false)
        {
            if (isWithParameter)
            {
                Sherlock.Instance.GetService<IFirebaseAnalyticsService>().SendCustom(eventName, new Dictionary<string, object>
                {
                {"Level",PlayerData.CurrentLevelId},
                });
                }
            else
            {
                Sherlock.Instance.GetService<IFirebaseAnalyticsService>().SendCustom(eventName);
            }
        }

        private void GameStarted()
        {
            TrackAdjustEvent("Game_Started");
            TrackFirebaseEvent("Game_Started", true);
        }

        private void LevelStarted()
        {
            TrackAdjustEvent("level_" + PlayerData.CurrentLevelId.ToString() + "_Start");
            TrackFirebaseEvent("Level_Started", true);
        }

        public void LevelRestarted()
        {
            TrackAdjustEvent("level_" + PlayerData.CurrentLevelId.ToString() + "_Restarted");
            TrackFirebaseEvent("Level_Restarted", true);
        }

        private void LevelFinished(bool isSuccess)
        {
            if (isSuccess)
            {
                TrackAdjustEvent("level_" + PlayerData.CurrentLevelId.ToString() + "_Completed");
                TrackFirebaseEvent("Level_Completed", true);
            }
            else
            {
                TrackAdjustEvent("level_" + PlayerData.CurrentLevelId.ToString() + "_Failed");
                TrackFirebaseEvent("Level_Failed", true);
            }
        }

    }
}