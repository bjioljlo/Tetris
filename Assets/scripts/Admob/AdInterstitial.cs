﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoogleMobileAds.Api;

public class AdInterstitial : MonoBehaviour {

	public static AdInterstitial ins;

	public string unitId;
	private InterstitialAd interstitialAd;

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

	public void LoadJumpAds()
	{
		this.ReRequestInterstitial();
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

            AdRequest.Builder _builder = new AdRequest.Builder();

            if (Debug.isDebugBuild) _builder.AddTestDevice(AdCommon.DeviceIdForAdmob);

            this.interstitialAd.LoadAd(_builder.Build());
        }
	}

	public void ShowInterstitial()
	{
		if (this.interstitialAd != null && this.interstitialAd.IsLoaded())
        {

            this.interstitialAd.Show();
        }
	}

	public bool IsAdLoaded()
	{
		return this.interstitialAd.IsLoaded();
	}
}
