public class FPS_ShowFPS : IShowFPS {
	public bool IsShowFps = false;

	public override void ShowFPS()
	{
		if (!IsShowFps)
        {
            m_text_FPS.text = "";
            return;
        }
        base.ShowFPS();
	}

}
