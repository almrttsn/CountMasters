using MatchinghamGames.DataUsageConsent;
using MatchinghamGames.DealerModule;
using MatchinghamGames.SherlockModule;
using MatchinghamGames.VegasModule;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MangoramaStudio.Scripts.Managers
{
    public class AppInitializer : MonoBehaviour
    {
        private void Start()
        {
            Vegas.Instance.Initialize();

            Sherlock.Instance.Initialize();
            Sherlock.Instance.WhenInitialized(SherlockInitialized);

            Dealer.Instance.Initialize();

            DataUsageConsentManager.Instance.Initialize();

            Vegas.LoadRewardedVideo();
            Vegas.LoadInterstitial();
            Vegas.LoadBanner();

            StartCoroutine(LoadGameSceneCo());
        }
        private void SherlockInitialized()
        {
            Debug.Log("Sherlock Init");
        }
        private IEnumerator LoadGameSceneCo()
        {
            yield return new WaitForSeconds(2f);
            SceneManager.LoadSceneAsync(ProjectConstants.SceneNames.GetSceneName(ProjectConstants.Scenes.Game));
        }
    }
}