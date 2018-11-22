using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_Button : IButton {

	
	public override AudioClip readSound()
	{
        m_SoundName = "Sound_Shot";
        return base.readSound();
	}

	public override void ClickAction()
	{
        Grid.getGrid.IsStart = true;
        FindObjectOfType<Pause_Button>().moveIn();
        FindObjectOfType<Spawner>().spawnNext();
		FindObjectOfType<Spawner>().spawnBag();
        FindObjectOfType<Start_Button>().moveOut();
        FindObjectOfType<ADS_Button>().moveOut();
        FindObjectOfType<Setting_Button>().moveOut();
        FindObjectOfType<Shop_Button>().moveOut();
        FindObjectOfType<Comic_Button>().moveOut();
        FindObjectOfType<Comment_Button>().moveOut();
		SoundManager.PlayBGM();
	}
}
