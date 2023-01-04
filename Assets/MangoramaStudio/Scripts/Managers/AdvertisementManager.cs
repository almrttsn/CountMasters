using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MatchinghamGames.VegasModule;

namespace MangoramaStudio.Scripts.Managers
{
    public class AdvertisementManager : CustomBehaviour
    {
        public override void Initialize(GameManager gameManager)
        {
            base.Initialize(gameManager);
        }

        public void LoadAndEnableBannerAd()
        {
            Vegas.LoadBanner();
        }

        public void HideBannerAd()
        {
            Vegas.HideBanner();
        }

        public void DestroyBannerAd()
        {
            Vegas.DestroyBanner();
        }

        public void ShowRewardedVideo(Action GiveRewardCallback)
        {
            Vegas.ShowRewardedVideo(GiveRewardCallback, () => { Debug.Log("No Reward..."); });
        }

        public void ShowInterstitial()
        {
            Vegas.ShowInterstitial();
        }
    }
}