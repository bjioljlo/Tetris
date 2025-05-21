public class CreatePlayer_Button : IButton
{
    public override void ClickAction()
    {
		playerInfo temp = new playerInfo(PlayerManager.In_Account.text, PlayerManager.In_Passwd.text);
		if(PlayerManager.check_playerInfoFormat_right(temp))
		{
			PlayerManager.create_player(temp);
		}      
    }
}

