using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGroup : Group
{

	public override void IPauseUpdate()
	{
        //檢查是否撞到玩家ＧＡＭＥＯＶＥＲ
        if (!isValidPlayerPos())
        {
            Grid.getGrid.GameOver();
			Grid.getGrid.GO_lastBox = this.gameObject;//將這次的方塊放入紀錄，以便看完廣告後刪除
        }
        base.IPauseUpdate();
	}

    public override void setTag()
    {
        if (transform.tag == "movingBox")
        {
            transform.tag = "stopBox";
            // Disable script
            enabled = false;
        }
    }
}
