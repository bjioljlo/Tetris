using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdRewardedVideo : MonoBehaviour {

	public static AdRewardedVideo ins;

	public string unitId;

	private RewardBasedVideoAd rewardBasedVideo = null;

	bool IsBonusAdsWatched = false;

	bool IsTest;

	public BonusAdsSupplier bonusAdsSupplier = BonusAdsSupplier.Google;
	public enum BonusAdsSupplier
	{
		Google,Unity
	}

   
    

	private void Awake()
    {
        if (ins == null)
        {
            ins = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (ins != this)
        {
            Destroy(gameObject);
        }
    }

	public void loadRewardedVideo(bool istest)
	{
		IsTest = istest;
		this.RequestRewardBasedVideo();
	}

	private void RequestRewardBasedVideo()
    {
		Debug.Log("RequestRewardBasedVideo start");
		if (!string.IsNullOrEmpty(this.unitId))
		{
			
			if (this.rewardBasedVideo == null)
			{
				this.rewardBasedVideo = RewardBasedVideoAd.Instance;
				// Called when the user should be rewarded for watching a video.
				this.rewardBasedVideo.OnAdRewarded += this.HandleRewardBasedVideoRewarded;
				// Called when the ad is closed.
                rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
				rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
				rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailLoaded;
			}

			AdRequest.Builder _builder = new AdRequest.Builder();

			if (Debug.isDebugBuild) _builder.AddTestDevice(AdCommon.DeviceIdForAdmob);
			if (IsTest) _builder.AddTestDevice(AdCommon.DeviceIdForAdmob);

            // Load the rewarded video ad with the request.
			this.rewardBasedVideo.LoadAd(_builder.Build(),unitId);

			Debug.Log("RequestRewardBasedVideo Done");
		}
      
    }

	public void ShowRewarded()
	{

		if(this.rewardBasedVideo != null && this.rewardBasedVideo.IsLoaded())
		{
			rewardBasedVideo.Show();
			return;
		}
		if (this.rewardBasedVideo == null) Debug.Log("rewardAds is null!");
		if (!this.rewardBasedVideo.IsLoaded()) Debug.Log("rewardAds is not loaded!");
	}

	public bool IsAdLoad()
	{
		return this.rewardBasedVideo.IsLoaded();
	}

	void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
		string type = args.Type;
		double amount = args.Amount;
		IsBonusAdsWatched = true;
    }

	void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
		if(IsBonusAdsWatched)
		{
			GiveAdBonusGoogle();
			IsBonusAdsWatched = false;
		}
		this.RequestRewardBasedVideo();
    }

	void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
	{
		Debug.Log("ADS LoadStart!!!");
	}
	void HandleRewardBasedVideoFailLoaded(object sender, AdFailedToLoadEventArgs args)
    {
		string type = args.Message;
		Debug.Log(type);
    }

    void GiveAdBonusGoogle()
	{
		AdManager.GiveAdsBonus();
	}
}
