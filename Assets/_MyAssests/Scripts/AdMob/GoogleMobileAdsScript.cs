using System;
using System.Collections;
using System.Collections.Generic;
 using GoogleMobileAds.Api;
using UnityEngine;

public class GoogleMobileAdsScript : MonoBehaviour
{
    [SerializeField] private string _adInterstitialAdId = "ca-app-pub-6452881415006935/9331073457";
    [SerializeField] private string _adBannerAdId = "ca-app-pub-6452881415006935/6812721321";
    [SerializeField] private string _adRewardtId = "ca-app-pub-6452881415006935/5180894662";
    [SerializeField] GameController _gameController;
    
    InterstitialAd _interstitialAd;
    BannerView _bannerView;
    RewardedAd _rewardedAd;
    
    
    private void Start()
    {
      DestroyBannerAd();
      DestroyInterstitialAd();
      DestroyRewardedAd();
      
      LoadInterstitialAd();
      LoadBannerAd();
      CreateBannerView();
    }
    
    /// <summary>
    /// Shows the interstitial ad.
    /// </summary>
    public void ShowInterstitialAd()
    {
      if (_interstitialAd != null && _interstitialAd.CanShowAd())
      {
        Debug.Log("Showing interstitial ad.");
        _interstitialAd.Show();
      }
      else
      {
        Debug.LogError("Interstitial ad is not ready yet.");
      }
    }
    /// <summary>
    /// Loads the interstitial ad.
    /// </summary>
    public void LoadInterstitialAd()
    {
      // // Clean up the old ad before loading a new one.
      // if (_interstitialAd != null)
      // {
      //   _interstitialAd.Destroy();
      //   _interstitialAd = null;
      // }
    
      Debug.Log("Loading the interstitial ad.");
    
      // create our request used to load the ad.
      var adRequest = new AdRequest();
    
      // send the request to load the ad.
      InterstitialAd.Load(_adInterstitialAdId, adRequest,
        (InterstitialAd ad, LoadAdError error) =>
        {
          // if error is not null, the load request failed.
          if (error != null || ad == null)
          {
            Debug.LogError("interstitial ad failed to load an ad " +
                           "with error : " + error);
            return;
          }
    
          Debug.Log("Interstitial ad loaded with response : "
                    + ad.GetResponseInfo());
    
          _interstitialAd = ad;
        });
    }
    
    public void CreateBannerView()
    {
      Debug.Log("Creating banner view");
    
      // If we already have a banner, destroy the old one.
      // if (_bannerView != null)
      // {
      //   DestroyBannerAd();
      // }
    
      // Create a 320x50 banner at top of the screen
      _bannerView = new BannerView(_adBannerAdId, AdSize.SmartBanner, AdPosition.Bottom);
    }
    
    /// <summary>
    /// Creates the banner view and loads a banner ad.
    /// </summary>
    public void LoadBannerAd()
    {
      // create an instance of a banner view first.
      if(_bannerView == null)
      {
        CreateBannerView();
      }
    
      // create our request used to load the ad.
      var adRequest = new AdRequest();
    
      // send the request to load the ad.
      Debug.Log("Loading banner ad.");
      _bannerView.LoadAd(adRequest);
    }
    public void ShowRewardedAd()
    {
      const string rewardMsg =
        "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

      if (_rewardedAd != null && _rewardedAd.CanShowAd())
      {
        _rewardedAd.Show((Reward reward) =>
        {
          _gameController.RestartGameAfterReward();
          // TODO: Reward the user.
          Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
        });
      }
    }
    
    /// <summary>
    /// Loads the rewarded ad.
    /// </summary>
    public void LoadRewardedAd()
    {
      // // Clean up the old ad before loading a new one.
      // if (_rewardedAd != null)
      // {
      //   _rewardedAd.Destroy();
      //   _rewardedAd = null;
      // }
      //
      // Debug.Log("Loading the rewarded ad.");

      // create our request used to load the ad.
      var adRequest = new AdRequest();

      // send the request to load the ad.
      RewardedAd.Load(_adRewardtId, adRequest,
        (RewardedAd ad, LoadAdError error) =>
        {
          // if error is not null, the load request failed.
          if (error != null || ad == null)
          {
            Debug.LogError("Rewarded ad failed to load an ad " +
                           "with error : " + error);
            return;
          }

          Debug.Log("Rewarded ad loaded with response : "
                    + ad.GetResponseInfo());

          _rewardedAd = ad;
        });
    }
    
    private void RegisterEventHandlers(RewardedAd ad)
    {
      // Raised when the ad is estimated to have earned money.
      ad.OnAdPaid += (AdValue adValue) =>
      {
        Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
          adValue.Value,
          adValue.CurrencyCode));
      };
      // Raised when an impression is recorded for an ad.
      ad.OnAdImpressionRecorded += () =>
      {
        Debug.Log("Rewarded ad recorded an impression.");
      };
      // Raised when a click is recorded for an ad.
      ad.OnAdClicked += () =>
      {
        Debug.Log("Rewarded ad was clicked.");
      };
      // Raised when an ad opened full screen content.
      ad.OnAdFullScreenContentOpened += () =>
      {
        Debug.Log("Rewarded ad full screen content opened.");
      };
      // Raised when the ad closed full screen content.
      ad.OnAdFullScreenContentClosed += () =>
      {
        Debug.Log("Rewarded ad full screen content closed.");
      };
      // Raised when the ad failed to open full screen content.
      ad.OnAdFullScreenContentFailed += (AdError error) =>
      {
        Debug.LogError("Rewarded ad failed to open full screen content " +
                       "with error : " + error);
      };
    }

    
    public void DestroyRewardedAd()
    {
      if (_rewardedAd != null) {
        _rewardedAd.Destroy();
        _rewardedAd = null;
      }
    }
    
    /// <summary>
    /// Destroys the banner view.
    /// </summary>
    public void DestroyBannerAd()
    {
      if (_bannerView != null)
      {
        Debug.Log("Destroying banner view.");
        _bannerView.Destroy();
        _bannerView = null;
      }
    }
    public void DestroyInterstitialAd()
    {
      // Clean up the old ad before loading a new one.
      if (_interstitialAd != null)
      {
        _interstitialAd.Destroy();
        _interstitialAd = null;
      }
    }

    public void DestroyAllAds()
    {
      DestroyBannerAd();
      DestroyInterstitialAd();
      DestroyRewardedAd();
    }
}
