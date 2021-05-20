using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdManager : IManager {
	//使用單例模式
	private static AdManager m_AdManager = null;
	private AdManager() {}
	public static AdManager GetAdManager
	{
		get{
			if(m_AdManager == null)
			{
				m_AdManager = new AdManager();
			}
			return m_AdManager;
		}
	}

	static AdBanner m_AdBanner;
	static AdInterstitial m_AdInterstitial;
	static AdRewardedVideo m_AdRewardedVideo;
	public string AppID;
	public bool IsTestMod = false;
	public static AdRewardedVideo.BonusAdsSupplier bonusAdsSupplier = Grid.getGrid.BonusAdsSupplier;

	private void Awake()
    {
        if (m_AdManager == null)
        {
            m_AdManager = this;
            DontDestroyOnLoad(this);
        }
        else if (m_AdManager != this)
        {
            Destroy(gameObject);
        }
	
	}

	private void Start()
    {
        m_AdBanner = GameObject.Find("AdBanner").GetComponent<AdBanner>();
        m_AdInterstitial = GameObject.Find("AdInterstitial").GetComponent<AdInterstitial>();
		m_AdRewardedVideo = GameObject.Find("AdRewardedVideo").GetComponent<AdRewardedVideo>();

		ReloadADS();
    }


	private void Update()
	{
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

	void ReloadADS(){
		m_AdBanner.loadBanner(IsTestMod);
		m_AdInterstitial.LoadJumpAds(IsTestMod);
		m_AdRewardedVideo.loadRewardedVideo(IsTestMod);
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

    //Rewarded Ads function------------------by google and Unity
	public static void ShowBonusAds()
	{
		m_AdRewardedVideo.ShowRewarded();
	}

	public static bool IsBonusAdsLoaded()
	{
		return m_AdRewardedVideo.IsAdLoad();
	}
   
	public static void GiveAdsBonus()
	{
		Destroy(Grid.getGrid.GO_lastBox);
        Destroy(FindObjectOfType<NormalGroup>().gameObject);
		if (Grid.getGrid.mainLava.transform.position.y <= 3)
        {
			iTween.MoveAdd(Grid.getGrid.mainLava.gameObject, iTween.Hash("easetype", iTween.EaseType.easeInOutExpo,
			                                                             "amount", new Vector3(0, -Grid.getGrid.mainLava.transform.position.y, 0),
                                                                                "time", 0.5f));
        }
        else
        {
			iTween.MoveAdd(Grid.getGrid.mainLava.gameObject, iTween.Hash("easetype", iTween.EaseType.easeInOutExpo,
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
