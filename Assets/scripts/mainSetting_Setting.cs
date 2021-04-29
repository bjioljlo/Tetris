using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainSetting_Setting : ISetting {

	public override void startInit()
	{
		base.startInit();
		DebugManager.SetDebugPage();
	}
}