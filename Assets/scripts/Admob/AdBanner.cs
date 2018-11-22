using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

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

	public void loadBanner()
    {
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

            AdRequest.Builder _builder = new AdRequest.Builder();

            if (Debug.isDebugBuild) _builder.AddTestDevice(AdCommon.DeviceIdForAdmob);

            this.bannerView.LoadAd(_builder.Build());
        }
	}

	public void ShowBanner()
	{
		if(this.bannerView != null)
		{
			this.bannerView.Show();
		}
	}

	public void HideBanner()
	{
		if(this.bannerView != null)
		{
			this.bannerView.Hide();
		}
	}
}
