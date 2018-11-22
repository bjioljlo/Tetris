using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart_Button : IButton {

	public override void ClickAction()
	{
		//      Grid.getGrid.IsStart = false;
		//      Grid.getGrid.IsPause = false;
		//      Grid.getGrid.Score = 0;
		//Grid.getGrid.BagLeftBox = 50;
		Grid.getGrid.ResetGrid();
        SceneManager.LoadScene("gamePage");
	}

}
