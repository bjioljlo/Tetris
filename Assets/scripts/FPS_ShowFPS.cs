using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS_ShowFPS : IShowFPS {
	public Toggle IsShowFpsToggle;

	public override void ShowFPS()
	{
		if (!GetToggle().isOn)
        {
            m_text_FPS.text = "";
            return;
        }
        base.ShowFPS();
	}

	public Toggle GetToggle()
	{
		if(IsShowFpsToggle == null)
		{
			IsShowFpsToggle = GameObject.Find("Debug_ShowFPS").GetComponent<Toggle>();		
		}

		return IsShowFpsToggle;
	}

}
