using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ManMoveable
{
	public int dirX = 1;
	public int dirY = 0;
	public Vector2 Old_Pos;
	public Vector2 New_Pos;

	public iTween tempitw = null;

	public float ManSpeed = Grid.getGrid.ManSpeed;

	public GameObject m_target;
	public bool IsEnd = false;

	public int climbHeigh = 2;

	public ManMoveable(GameObject target,ManMoveable manMoveable)
	{
		setTarget(target);
		if(manMoveable != null)
		{
			Old_Pos = manMoveable.Old_Pos;
			New_Pos = manMoveable.New_Pos;
			dirX = manMoveable.dirX;
			dirY = manMoveable.dirY;
			tempitw = manMoveable.tempitw;
			climbHeigh = manMoveable.climbHeigh + 1;
		}
	}

	public void setTarget(GameObject target)
	{
		m_target = target;
	}

	public abstract void Move();
}

public class NormalManMove : ManMoveable
{
	public NormalManMove(GameObject target, ManMoveable manMoveable) : base(target, manMoveable)
	{
	}

	public override void Move()
	{
		m_target.transform.position = Grid.getGrid.roundVec2(m_target.transform.position);//將物件位置調整為整數
		Old_Pos = m_target.transform.position;
        dirY = 0;//Y方向歸零

        //X方向確認(邊界固定方向)
		if (m_target.transform.position.x >= Grid.getGrid.Width - 1)
            dirX = -1;
		else if (m_target.transform.position.x <= 0)
            dirX = 1;

        //由Ｘ方向來確認Ｙ方向該為多少(這裡沒從地板開始檢查，是從人物同高的地方)
		for (int i = (int)m_target.transform.position.y; i < Grid.getGrid.Height_Now; i++)
        {
			if (Grid.getGrid.grids[(int)m_target.transform.position.x + dirX][i] == null ||
			    Grid.getGrid.grids[(int)m_target.transform.position.x + dirX][i].parent.transform.tag == "itemBox")
            {
				dirY = i - (int)m_target.transform.position.y;
                break;
            }
        }

        //高度大於1就不能走要換方向
        if (dirY >= 2)
        {
            dirX = -dirX;
            return;
        }

        //在已經上去了並且"往前走"的狀況下檢查前方是否有地板,沒地板要換方向(因為前面沒檢查是否有地板line:48)
		if (m_target.transform.position.y > 0 && dirY == 0 &&
		    Grid.getGrid.grids[(int)m_target.transform.position.x + dirX][(int)m_target.transform.position.y - 1] == null)
        {
            dirX = -dirX;
            return;
        }

        //在“往前走“＋“有地板”的狀況下檢查前方地板是否是固定的方塊,如果不是固定方塊換方向
		if (m_target.transform.position.y > 0 && dirX != 0)
        {
            for (int i = -1; i < 1; i++)
            {
				if (Grid.getGrid.grids[(int)m_target.transform.position.x + dirX][(int)m_target.transform.position.y + i] != null &&
				    Grid.getGrid.grids[(int)m_target.transform.position.x + dirX][(int)m_target.transform.position.y + i].parent.transform.tag == "movingBox")
                {
                    dirX = -dirX;
                    return;
                }
            }

        }


        New_Pos = Old_Pos + new Vector2(dirX, dirY);//紀錄新位置

		iTween.MoveAdd(m_target, iTween.Hash("easetype", iTween.EaseType.easeInOutExpo,
                                              "amount", new Vector3(dirX, dirY, 0),
                                              "time", ManSpeed));

		tempitw = m_target.GetComponent<iTween>();

        //人物圖片換方向
        if (dirX < 0)
			m_target.GetComponentInChildren<SpriteRenderer>().flipX = true;
        else if (dirX > 0)
			m_target.GetComponentInChildren<SpriteRenderer>().flipX = false;
	}

}

public class StopManMove : ManMoveable
{
	int StopTimes = 0;
	public StopManMove(GameObject target, ManMoveable manMoveable) : base(target, manMoveable)
	{
		setStopTime(3);
	}

	public override void Move()
	{
			iTween.MoveAdd(m_target, iTween.Hash("easetype", iTween.EaseType.easeInOutExpo,
		                                         "amount", new Vector3(0, 0, 0),
												  "time", ManSpeed));
		tempitw = m_target.GetComponent<iTween>();
		if (StopTimes > 1)
        {
            StopTimes--;
        }
        else
        {
            IsEnd = true;
        } 
	}

	public void setStopTime(int times)
	{
		StopTimes = times;
	}
}


public class StrongManMove : ManMoveable
{

	public StrongManMove(GameObject target, ManMoveable manMoveable) : base(target, manMoveable)
    {
		
    }

    public override void Move()
    {
        m_target.transform.position = Grid.getGrid.roundVec2(m_target.transform.position);//將物件位置調整為整數
        Old_Pos = m_target.transform.position;
        dirY = 0;//Y方向歸零

        //X方向確認(邊界固定方向)
		if (m_target.transform.position.x >= Grid.getGrid.Width - 1)
            dirX = -1;
        else if (m_target.transform.position.x <= 0)
            dirX = 1;

        //由Ｘ方向來確認Ｙ方向該為多少(這裡沒從地板開始檢查，是從人物同高的地方)
        for (int i = (int)m_target.transform.position.y; i < Grid.getGrid.Height_Now; i++)
        {
            if (Grid.getGrid.grids[(int)m_target.transform.position.x + dirX][i] == null ||
                Grid.getGrid.grids[(int)m_target.transform.position.x + dirX][i].parent.transform.tag == "itemBox")
            {
                dirY = i - (int)m_target.transform.position.y;
                break;
            }
        }

        //高度大於1就不能走要換方向
		if (dirY >= climbHeigh)
        {
            dirX = -dirX;
            return;
        }

        //在已經上去了並且"往前走"的狀況下檢查前方是否有地板,沒地板要換方向(因為前面沒檢查是否有地板line:48)
        if (m_target.transform.position.y > 0 && dirY == 0 &&
            Grid.getGrid.grids[(int)m_target.transform.position.x + dirX][(int)m_target.transform.position.y - 1] == null)
        {
            dirX = -dirX;
            return;
        }

        //在“往前走“＋“有地板”的狀況下檢查前方地板是否是固定的方塊,如果不是固定方塊換方向
        if (m_target.transform.position.y > 0 && dirX != 0)
        {
            for (int i = -1; i < 1; i++)
            {
                if (Grid.getGrid.grids[(int)m_target.transform.position.x + dirX][(int)m_target.transform.position.y + i] != null &&
                    Grid.getGrid.grids[(int)m_target.transform.position.x + dirX][(int)m_target.transform.position.y + i].parent.transform.tag == "movingBox")
                {
                    dirX = -dirX;
                    return;
                }
            }

        }


        New_Pos = Old_Pos + new Vector2(dirX, dirY);//紀錄新位置

        iTween.MoveAdd(m_target, iTween.Hash("easetype", iTween.EaseType.easeInOutExpo,
                                              "amount", new Vector3(dirX, dirY, 0),
                                              "time", ManSpeed));

        tempitw = m_target.GetComponent<iTween>();

        //人物圖片換方向
        if (dirX < 0)
            m_target.GetComponentInChildren<SpriteRenderer>().flipX = true;
        else if (dirX > 0)
            m_target.GetComponentInChildren<SpriteRenderer>().flipX = false;
    }

}
