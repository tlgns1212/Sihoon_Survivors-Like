using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using static Define;

public class AdsManager
{
    public enum AdsStateType
    {
        None,
        Failed,
        Success,
    }

    private Action _rewardedCallback;
    private RewardedAd _rewardedAd;

    public void Init()
    {
        List<String> deviceIds = new List<String>() { AdRequest.TestDeviceSimulator };
        
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            RequestAndLoadRewardedAd();
        });
    }
    
    public void RequestAndLoadRewardedAd()
    {
        Debug.Log("@>> Requesting Rewarded Ad");
        string adUnitId = "unused";
        
        RewardedAd.Load(adUnitId, CreateAdRequest(), (RewardedAd ad, LoadAdError loadError) =>
        {
            if (loadError != null)
            {
                Debug.Log("@>> Rewarded ad failed to load with error : " + loadError.GetMessage());
                return;
            }
            else if (ad == null)
            {
                Debug.Log("@>> Rewarded ad failed to load");
                return;
            }

            Debug.Log("@>> Rewarded ad loaded");
            _rewardedAd = ad;

            ad.OnAdFullScreenContentOpened += () =>
            {
                Debug.Log("@>>Rewarded ad opening");
            };
            ad.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("@>>Rewarded ad closed");
                RequestAndLoadRewardedAd();
            };
            ad.OnAdImpressionRecorded += () =>
            {
                Debug.Log("@>>Rewarded ad recorded an impression");
            };
            ad.OnAdClicked += () =>
            {
                Debug.Log("@>>Rewarded ad recorded a click");
            };
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.Log("@>>Rewarded ad failed to show with wrror! : " + error.GetMessage());
            };
            ad.OnAdPaid += (AdValue adValue) =>
            {
                string msg = string.Format("{0} (currency: {1}, value {2}", "Rewarded ad received a paid event", adValue.CurrencyCode, adValue.Value);
                Debug.Log(msg);
            };
        });
    }

    private Coroutine _coroutine;
    public void ShowRewardedAd(Action callback)
    {
        if (_rewardedAd != null)
        {
            _rewardedAd.Show((Reward reward) =>
            {
                CoroutineManager.StartCoroutine(CoRewardEnd(callback));
                Debug.Log("Rewarded ad granted a reward :L " + reward.Amount);
            });
        }
        else
        {
            Debug.Log("Rewarded ad is not ready yet");
        }
    }

    private IEnumerator CoRewardEnd(Action callback)
    {
        yield return new WaitForEndOfFrame();
        if (Managers.Game.DicMission.TryGetValue(MissionTarget.ADWatching, out MissionInfo mission))
        {
            mission.Progress++;
        }
        callback?.Invoke();
    }

    private AdRequest CreateAdRequest()
    {
        AdRequest adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");
        return adRequest;
    }
}
