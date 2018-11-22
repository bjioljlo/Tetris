using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdManager : IManager {
	static AdBanner m_AdBanner;
	static AdInterstitial m_AdInterstitial;
	static UnityAds m_UnityAds;

	private void Start()
	{
		m_AdBanner = GameObject.Find("AdBanner").GetComponent<AdBanner>();
		m_AdInterstitial = GameObject.Find("AdInterstitial").GetComponent<AdInterstitial>();
		m_UnityAds = gameObject.AddComponent<UnityAds>();

		m_AdBanner.loadBanner();
		m_AdInterstitial.LoadJumpAds();
	}

	private void Update()
	{
		if(Grid.getGrid.IsPause == false && Grid.getGrid.IsStart == true)
		{
			m_AdBanner.HideBanner();
		}
		else
		{
			m_AdBanner.ShowBanner();
		}
	}

	public static void ShowJumpAds()
	{
		m_AdInterstitial.ShowInterstitial();
	}

	public static bool IsJumpAdsLoaded()
	{
		return m_AdInterstitial.IsAdLoaded();
	}

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
        SoundManager.PlayBGM();
        Grid.getGrid.IsPause = false;
        FindObjectOfType<Spawner>().spawnNext();
	}
}
