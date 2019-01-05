using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdRewardedVideo : MonoBehaviour {

	public static AdRewardedVideo ins;

	public string unitId;

	private RewardBasedVideoAd rewardBasedVideo;

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
		if (!string.IsNullOrEmpty(this.unitId))
		{
			Debug.Log("RequestRewardBasedVideo start");
			if (this.rewardBasedVideo == null)
			{
				this.rewardBasedVideo = RewardBasedVideoAd.Instance;
				// Called when the user should be rewarded for watching a video.
				this.rewardBasedVideo.OnAdRewarded += this.HandleRewardBasedVideoRewarded;
				// Called when the ad is closed.
                rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
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

    void GiveAdBonusGoogle()
	{
		AdManager.GiveAdsBonus();
	}
}
