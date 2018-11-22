using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;

public class ItemGroup : Group {
    //物品效果事件
    public Actionabl m_itemAct;
	//人物效果事件
	public Actionabl m_manItemAct;
    public bool IsStop = false;
    public bool IsUsed = false;

    public void setActionType(Actionabl actionabl)
    {
        m_itemAct = actionabl;
    }

	public void setManItemAct(Actionabl actionabl)
	{
		m_manItemAct = actionabl;
	}

	public override void IPauseUpdate()
	{
        if (IsUsed)
            return;
        //檢查是否撞到玩家執行物品效果
        if (!isValidPlayerPos())
        {
            if(!IsStop)//不是停止的道具才要新增下一個方塊
            {
                FindObjectOfType<Spawner>().spawnNext();
                //恢復可以按下往下按鈕
                m_moveAct.downPush = false;
                //移除監聽
                m_moveAct.DeletBtnListener();
            }

			mans[0].GetComponent<Man>().setItemGroup(this.gameObject.GetComponent<ItemGroup>());
			mans[0].GetComponent<Man>().setItemAction(m_manItemAct);
            m_itemAct.ActionStart();//執行效果
            deletThisGroup();//刪除grid中紀錄的格子
            Destroy(this.gameObject, 1f);
            IsUsed = true;//已經使用了
            //Destroy(this.gameObject);//將自己刪除
            return;
        }
        if (IsStop)
            return;
        base.IPauseUpdate();
	}

    public override void setTag()
    {
        IsStop = true;
    }

}
