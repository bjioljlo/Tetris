using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdBanner : MonoBehaviour {

	public enum BannerSize
    {

        BANNER,
        MEDIUM_RECTANGLE,
        FULL_BANNER,
        LEADERBOARD,
        SMART_BANNER
    }

    public static AdBanner ins;

    public string unitId;
    public BannerSize size = BannerSize.BANNER;
    public AdPosition position = AdPosition.Top;

    private BannerView bannerView;
    private Dictionary<BannerSize, AdSize> adSize = new Dictionary<BannerSize, AdSize>(){
        {BannerSize.BANNER , AdSize.Banner},
        {BannerSize.MEDIUM_RECTANGLE , AdSize.MediumRectangle},
        {BannerSize.FULL_BANNER , AdSize.IABBanner},
        {BannerSize.LEADERBOARD , AdSize.Leaderboard},
        {BannerSize.SMART_BANNER , AdSize.SmartBanner}
    };

	private bool Is_showing = false;
	public bool IsShow { get{ return Is_showing; } }

	private bool IsTest;

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

	public void loadBanner(bool istest)
    {
		IsTest = istest;
        this.RequestBanner();
    }

	private void RequestBanner()
	{
		if (!string.IsNullOrEmpty(this.unitId))
        {

			if (this.bannerView != null)
            {
				this.bannerView.Destroy();
				this.bannerView = null;
            }

            this.bannerView = new BannerView(this.unitId, this.adSize[this.size], this.position);

			// Called when an ad request has successfully loaded.
            this.bannerView.OnAdLoaded += this.HandleOnAdLoaded;

            AdRequest.Builder _builder = new AdRequest.Builder();

            if (Debug.isDebugBuild) _builder.AddTestDevice(AdCommon.DeviceIdForAdmob);
			if (IsTest) _builder.AddTestDevice(AdCommon.DeviceIdForAdmob);

            this.bannerView.LoadAd(_builder.Build());
			Debug.Log("RequestBanner");
        }
	}

	public void ShowBanner()
	{
		if(this.bannerView != null)
		{
			this.bannerView.Show();
			Is_showing = true;
		}
	}

	public void HideBanner()
	{
		if(this.bannerView != null)
		{
			this.bannerView.Hide();
			Is_showing = false;
		}
	}

	void HandleOnAdLoaded(object sender, EventArgs args)
    {
		
    }
}
