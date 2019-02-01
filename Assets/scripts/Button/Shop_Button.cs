using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_Button : IButton {

	ctl_ScrollView[] array_scrollView = null;
	GameObject @object;
	public override void startInit()
	{
		base.startInit();
		@object = GameObject.Find("Shop");
		array_scrollView = @object.GetComponentsInChildren<ctl_ScrollView>();
	}

	public override void ClickAction()
	{
		
		base.ClickAction();
		foreach (ctl_ScrollView child in array_scrollView)
        {
            child.setScrollView_content();
        }
	}
}
