using UnityEngine.SceneManagement;

public class Restart_Button : IButton {

	public override void ClickAction()
	{
		Grid.getGrid.ResetGrid();
        SceneManager.LoadScene("gamePage");
	}

}
