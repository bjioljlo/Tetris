using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause_Button : IButton {
	

	public override void ClickAction()
	{
        if (Grid.getGrid.IsPause == true)
        {
            FindObjectOfType<Restart_Button>().moveOut();
            FindObjectOfType<Setting_Button>().moveOut();
            FindObjectOfType<Comment_Button>().moveOut();
            FindObjectOfType<Best_Text>().moveOut();
            FindObjectOfType<Score_Text>().moveOut();
			FindObjectOfType<Coin_Text>().moveOut();
			SoundManager.PlayBGM();
            Grid.getGrid.IsPause = false;
        }

        else if (Grid.getGrid.IsPause == false)
        {
            FindObjectOfType<Restart_Button>().moveIn();
            FindObjectOfType<Setting_Button>().moveIn();
            FindObjectOfType<Comment_Button>().moveIn();
            FindObjectOfType<Best_Text>().moveIn();
            FindObjectOfType<Score_Text>().moveIn();
			FindObjectOfType<Coin_Text>().moveIn();
			SoundManager.StopBGM();
            Grid.getGrid.IsPause = true;
        }
	}

}
