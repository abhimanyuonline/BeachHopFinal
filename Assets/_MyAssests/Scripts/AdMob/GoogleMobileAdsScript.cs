using System;
using System.Collections;
using System.Collections.Generic;
// using GoogleMobileAds.Api;
using UnityEngine;

public class GoogleMobileAdsScript : MonoBehaviour
{
    // private string _adInterstitialAdId = "ca-app-pub-6452881415006935/9331073457";
    // private string _adBannerAdId = "ca-app-pub-6452881415006935/6812721321";
    //
    // InterstitialAd _interstitialAd;
    // BannerView _bannerView;
    //
    //
    // private void Start()
    // {
    //   DestroyBannerAd();
    //   DestroyInterstitialAd();
    //   
    //   LoadInterstitialAd();
    //   LoadBannerAd();
    //   CreateBannerView();
    // }
    //
    // /// <summary>
    // /// Shows the interstitial ad.
    // /// </summary>
    // public void ShowInterstitialAd()
    // {
    //   if (_interstitialAd != null && _interstitialAd.CanShowAd())
    //   {
    //     Debug.Log("Showing interstitial ad.");
    //     _interstitialAd.Show();
    //   }
    //   else
    //   {
    //     Debug.LogError("Interstitial ad is not ready yet.");
    //   }
    // }
    // /// <summary>
    // /// Loads the interstitial ad.
    // /// </summary>
    // public void LoadInterstitialAd()
    // {
    //   // // Clean up the old ad before loading a new one.
    //   // if (_interstitialAd != null)
    //   // {
    //   //   _interstitialAd.Destroy();
    //   //   _interstitialAd = null;
    //   // }
    //
    //   Debug.Log("Loading the interstitial ad.");
    //
    //   // create our request used to load the ad.
    //   var adRequest = new AdRequest();
    //
    //   // send the request to load the ad.
    //   InterstitialAd.Load(_adInterstitialAdId, adRequest,
    //     (InterstitialAd ad, LoadAdError error) =>
    //     {
    //       // if error is not null, the load request failed.
    //       if (error != null || ad == null)
    //       {
    //         Debug.LogError("interstitial ad failed to load an ad " +
    //                        "with error : " + error);
    //         return;
    //       }
    //
    //       Debug.Log("Interstitial ad loaded with response : "
    //                 + ad.GetResponseInfo());
    //
    //       _interstitialAd = ad;
    //     });
    // }
    //
    // public void CreateBannerView()
    // {
    //   Debug.Log("Creating banner view");
    //
    //   // If we already have a banner, destroy the old one.
    //   // if (_bannerView != null)
    //   // {
    //   //   DestroyBannerAd();
    //   // }
    //
    //   // Create a 320x50 banner at top of the screen
    //   _bannerView = new BannerView(_adBannerAdId, AdSize.SmartBanner, AdPosition.Bottom);
    // }
    //
    // /// <summary>
    // /// Creates the banner view and loads a banner ad.
    // /// </summary>
    // public void LoadBannerAd()
    // {
    //   // create an instance of a banner view first.
    //   if(_bannerView == null)
    //   {
    //     CreateBannerView();
    //   }
    //
    //   // create our request used to load the ad.
    //   var adRequest = new AdRequest();
    //
    //   // send the request to load the ad.
    //   Debug.Log("Loading banner ad.");
    //   _bannerView.LoadAd(adRequest);
    // }
    //
    // /// <summary>
    // /// Destroys the banner view.
    // /// </summary>
    // public void DestroyBannerAd()
    // {
    //   if (_bannerView != null)
    //   {
    //     Debug.Log("Destroying banner view.");
    //     _bannerView.Destroy();
    //     _bannerView = null;
    //   }
    // }
    // public void DestroyInterstitialAd()
    // {
    //   // Clean up the old ad before loading a new one.
    //   if (_interstitialAd != null)
    //   {
    //     _interstitialAd.Destroy();
    //     _interstitialAd = null;
    //   }
    // }
}
