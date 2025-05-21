public class mainSetting_Setting : ISetting {

	public override void startInit()
	{
		base.startInit();
		DebugManager.SetDebugPage();
	}
}