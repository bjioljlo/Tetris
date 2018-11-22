using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login_Button : IButton {

	public override void ClickAction()
	{
		playerInfo temp = new playerInfo(PlayerManager.In_Account.text,PlayerManager.In_Passwd.text);
		PlayerManager.login_player(temp);
	}
}
