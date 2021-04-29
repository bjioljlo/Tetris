using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchADS_Button : IButton
{
    public override void moveIn()
    {
        
    }

    public override void moveOut()
    {
        
    }

	public override void ClickAction()
	{
		//      Destroy(Grid.getGrid.GO_lastBox);
		//Destroy(FindObjectOfType<NormalGroup>().gameObject);
		//      if(FindObjectOfType<mainLava>().transform.position.y <= 3)
		//      {
		//          iTween.MoveAdd(FindObjectOfType<mainLava>().gameObject, iTween.Hash("easetype", iTween.EaseType.easeInOutExpo,
		//                                                                              "amount", new Vector3(0, -FindObjectOfType<mainLava>().transform.position.y , 0),
		//                                                                              "time", 0.5f));
		//      }
		//      else
		//      {
		//          iTween.MoveAdd(FindObjectOfType<mainLava>().gameObject, iTween.Hash("easetype", iTween.EaseType.easeInOutExpo,
		//                                            "amount", new Vector3(0, -3, 0),
		//                                            "time", 0.5f));
		//      }

		//      FindObjectOfType<Restart_Button>().moveOut();
		//      FindObjectOfType<Best_Text>().moveOut();
		//      FindObjectOfType<Score_Text>().moveOut();
		//      FindObjectOfType<mainGameover_IUI>().moveOut();
		//      FindObjectOfType<Pause_Button>().moveIn();
		//SoundManager.PlayBGM();
		//Grid.getGrid.IsPause = false;
		//FindObjectOfType<Spawner>().spawnNext();
		if (AdManager.IsBonusAdsLoaded())
            {
                AdManager.ShowBonusAds();
            }

	}
}
