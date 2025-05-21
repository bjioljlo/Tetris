public class Logout_Button : IButton {
	
	public override void ClickAction()
    {
		PlayerManager.logout_player();
    }
}
