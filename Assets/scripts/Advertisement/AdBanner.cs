using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.Advertisements;

public class AdBanner : UnityAds {

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
    public BannerSize GoogleSize = BannerSize.BANNER;
    public AdPosition GooglePosition = AdPosition.Top;
	public BannerPosition UnityPosition = BannerPosition.CENTER;

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
        this.RequestBanner();//google

		//unity
		Advertisement.Banner.SetPosition (UnityPosition);
		loadUnityADS(IsTest);
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
			MobileAds.Initialize(initStatus => {});
            this.bannerView = new BannerView(this.unitId, this.adSize[this.GoogleSize], this.GooglePosition);
			// Called when an ad request has successfully loaded.
            this.bannerView.OnAdLoaded += this.HandleOnAdLoaded;

            AdRequest.Builder _builder = new AdRequest.Builder();

            if (Debug.isDebugBuild) _builder.AddTestDevice(AdCommon.DeviceIdForAdmob);
			if (IsTest) _builder.AddTestDevice(AdCommon.DeviceIdForAdmob);

            this.bannerView.LoadAd(_builder.Build());

        }
	}

	public void ShowBanner()
	{
		if(this.bannerView != null)
		{
			this.bannerView.Show();//google
			StartCoroutine(ShowBannerWhenInitialized());//unity
			Is_showing = true;
		}
	}

	public void HideBanner()
	{
		if(this.bannerView != null)
		{
			this.bannerView.Hide();//google
			Advertisement.Banner.Hide();//unity
			Is_showing = false;
		}
	}

	IEnumerator ShowBannerWhenInitialized () {
        while (!Advertisement.isInitialized) {
            yield return new WaitForSeconds(0.3f);
        }
        Advertisement.Banner.Show(UnityPlacementID);
    }

//google 的回傳
	void HandleOnAdLoaded(object sender, EventArgs args)
    {
		Debug.Log("google Banner loaded");
    }

//unity 的回傳
	public override void resultFinished(){}
    public override void resultSkipped(){}
    public override void resultFailed(){}
}
