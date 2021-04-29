using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.Advertisements;

public class AdRewardedVideo : UnityAds {

	public static AdRewardedVideo ins;

	public string unitId;
	private RewardBasedVideoAd rewardBasedVideo = null;
	bool IsBonusAdsWatched = false;
	bool IsTest;

	//public static BonusAdsSupplier bonusAdsSupplier = Grid.getGrid.BonusAdsSupplier;
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
		Debug.Log("load RewardedVideo ADS");
		IsTest = istest;
		this.RequestRewardBasedVideo();
		// Initialize the Ads listener and service:
		loadUnityADS(IsTest);
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
		if(AdManager.bonusAdsSupplier == AdRewardedVideo.BonusAdsSupplier.Google){
			if(this.rewardBasedVideo != null && this.rewardBasedVideo.IsLoaded()){
				rewardBasedVideo.Show();
				return;
			}
			else{
            		Debug.Log("Rewarded ad not ready at the moment! Please try again later!");
        	}
		}
		else{
			if(Advertisement.IsReady(UnityPlacementID)){
				Advertisement.Show (UnityPlacementID);
				return;
			}
			else{
            		Debug.Log("Rewarded ad not ready at the moment! Please try again later!");
        	}
		}
		if (this.rewardBasedVideo == null) Debug.Log("rewardAds is null!");
	}

	public bool IsAdLoad()
	{
		if(AdManager.bonusAdsSupplier == AdRewardedVideo.BonusAdsSupplier.Google){
			return this.rewardBasedVideo.IsLoaded();
		}
		else if(AdManager.bonusAdsSupplier == AdRewardedVideo.BonusAdsSupplier.Unity){
			return Advertisement.IsReady(UnityPlacementID);
		}
		else{
			return false;
		}
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
		Debug.Log("google RewardADS LoadStart!!!");
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

	//unity 的回傳
	public override void resultFinished(){
		GiveAdBonusGoogle();
		loadUnityADS(IsTest);
	}
    public override void resultSkipped(){}
    public override void resultFailed(){}
}
