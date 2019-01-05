using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdManager : IManager {
	static AdBanner m_AdBanner;
	static AdInterstitial m_AdInterstitial;
	static AdRewardedVideo m_AdRewardedVideo;
	static UnityAds m_UnityAds;
	public string AppID;

	public static AdManager ins;

	public bool IsTestMod = false;

	public static AdRewardedVideo.BonusAdsSupplier bonusAdsSupplier;

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

	private void Start()
    {
        m_AdBanner = GameObject.Find("AdBanner").GetComponent<AdBanner>();
        m_AdInterstitial = GameObject.Find("AdInterstitial").GetComponent<AdInterstitial>();
		m_AdRewardedVideo = GameObject.Find("AdRewardedVideo").GetComponent<AdRewardedVideo>();
        m_UnityAds = gameObject.AddComponent<UnityAds>();

       
		m_AdBanner.loadBanner(IsTestMod);
		m_AdInterstitial.LoadJumpAds(IsTestMod);
		m_AdRewardedVideo.loadRewardedVideo(IsTestMod);
    }


	private void Update()
	{
		bonusAdsSupplier = m_AdRewardedVideo.bonusAdsSupplier;

		if(Grid.getGrid.IsPause == false && Grid.getGrid.IsStart == true)
		{
			if(m_AdBanner.IsShow)
			m_AdBanner.HideBanner();
		}
		else
		{
			if(!m_AdBanner.IsShow)
			m_AdBanner.ShowBanner();
		}
	}
    
    //Jump ads function---------------------
	public static void ShowJumpAds()
	{
		m_AdInterstitial.ShowInterstitial();
	}

	public static bool IsJumpAdsLoaded()
	{
		return m_AdInterstitial.IsAdLoaded();
	}

    //Rewarded Ads function------------------by google
	public static void ShowGoogleBonusAds()
	{
		m_AdRewardedVideo.ShowRewarded();
	}

	public static bool IsGoogleBonusAdsLoaded()
	{
		return m_AdRewardedVideo.IsAdLoad();
	}

    //Rewarded Ads function------------------by Unity
	public static void ShowBonusAds()
	{
		m_UnityAds.ShowRewardedAd();
	}
   
	public static void GiveAdsBonus()
	{
		Destroy(Grid.getGrid.GO_lastBox);
        Destroy(FindObjectOfType<NormalGroup>().gameObject);
        if (FindObjectOfType<mainLava>().transform.position.y <= 3)
        {
            iTween.MoveAdd(FindObjectOfType<mainLava>().gameObject, iTween.Hash("easetype", iTween.EaseType.easeInOutExpo,
                                                                                "amount", new Vector3(0, -FindObjectOfType<mainLava>().transform.position.y, 0),
                                                                                "time", 0.5f));
        }
        else
        {
            iTween.MoveAdd(FindObjectOfType<mainLava>().gameObject, iTween.Hash("easetype", iTween.EaseType.easeInOutExpo,
                                              "amount", new Vector3(0, -3, 0),
                                              "time", 0.5f));
        }

        FindObjectOfType<Restart_Button>().moveOut();
        FindObjectOfType<Best_Text>().moveOut();
        FindObjectOfType<Score_Text>().moveOut();
        FindObjectOfType<mainGameover_IUI>().moveOut();
        FindObjectOfType<Pause_Button>().moveIn();      
        Grid.getGrid.IsPause = false;
        FindObjectOfType<Spawner>().spawnNext();
	}
}
