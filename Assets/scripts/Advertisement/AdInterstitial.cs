using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoogleMobileAds.Api;
using UnityEngine.Advertisements;

public class AdInterstitial : UnityAds {

	public static AdInterstitial ins;

	public string unitId;
	private InterstitialAd interstitialAd;

	bool IsTest;

	private void Awake()
	{
		if(ins == null)
		{
			ins = this;
			DontDestroyOnLoad(gameObject);
		}
		else if(ins != this)
		{
			Destroy(gameObject);
		}
	}

	public void LoadJumpAds(bool istest)
	{
		Debug.Log("load JumpAds ADS");
		IsTest = istest;
		this.ReRequestInterstitial();
		loadUnityADS(IsTest);
	}

	private void ReRequestInterstitial()
	{
		if (!string.IsNullOrEmpty(this.unitId))
        {

            if (this.interstitialAd != null)
            {
                this.interstitialAd.Destroy();
                this.interstitialAd = null;
            }

            this.interstitialAd = new InterstitialAd(this.unitId);

			// Called when the ad is closed.
			this.interstitialAd.OnAdClosed += this.HandleOnAdClosed;
			this.interstitialAd.OnAdLoaded += this.HandleOnAdLoaded;

            AdRequest.Builder _builder = new AdRequest.Builder();

            if (Debug.isDebugBuild) _builder.AddTestDevice(AdCommon.DeviceIdForAdmob);
			if (IsTest) _builder.AddTestDevice(AdCommon.DeviceIdForAdmob);

            this.interstitialAd.LoadAd(_builder.Build());


        }
	}

	public void ShowInterstitial()
	{
		if (this.interstitialAd != null){
			if(this.interstitialAd.IsLoaded()){
				this.interstitialAd.Show();
				return;
			}
			else{
				Debug.Log("google Interstitial ad not ready at the moment! Please try again later!");
			}
		}

		if (Advertisement.IsReady(UnityPlacementID)) {
    		ShowUnityADS();
			PauseGame();
			return;
        } 
        else {
            Debug.Log("Unity Interstitial ad not ready at the moment! Please try again later!");
        }
	}

	public bool IsAdLoaded()
	{
		if(this.interstitialAd.IsLoaded() || Advertisement.IsReady(UnityPlacementID)){return true;}
		else{return false;}
	}

	public void PauseGame()
    {
        Time.timeScale = 0;
        Debug.Log("Pause Game");
    }
	public void ResumeGame()
    {
        Time.timeScale = 1;
        Debug.Log("Resume Game");
    }

	//google 的回傳
	void HandleOnAdClosed(object sender, EventArgs args)
    {
		this.ReRequestInterstitial();
    }
	void HandleOnAdLoaded(object sender,EventArgs args){
		Debug.Log("google interstitialAd is ready");
	}

	//unity 的回傳
	public override void resultFinished(){
		loadUnityADS(IsTest);
		ResumeGame();
	}
    public override void resultSkipped(){}
    public override void resultFailed(){}
}
