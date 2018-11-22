using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Logout_Button : IButton {
	
	public override void ClickAction()
    {
		PlayerManager.logout_player();
    }
}
