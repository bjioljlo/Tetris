using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effectable {
    public GameObject m_target;//物品gameobj
	public float delayTime;
	public float showTime;
    public Effectable (GameObject target)
    {
		setTarget(target);
    }

    public abstract void show();

	public void setTarget(GameObject target)
	{
		m_target = target;
	}

	public void setDelayTime(float time)
	{
		delayTime = time;
	}

	public void setShowTime(float time)
	{
		showTime = time;
	}
}

public class JumpEffect : Effectable
{
	public int jumpHeight;
    public JumpEffect(GameObject target) : base(target)
    {
		setShowTime(0.5f);
		setDelayTime(0);
    }

    public override void show()
    {
		iTween.MoveAdd(m_target, iTween.Hash("easetype", iTween.EaseType.easeInOutExpo,
		                                     "amount", new Vector3(0, jumpHeight, 0),
		                                                "time", showTime,
		                                                "delay",delayTime));
    }

	public void setJumpHeight(int height)
	{
		jumpHeight = height;
	}
 
}

public class StopEffect : Effectable
{
    public StopEffect(GameObject target) : base(target)
    {
		
    }

    public override void show()
    {
		Debug.Log("停止");

    }
}

public class BombEffect : Effectable
{
	public BombEffect(GameObject target) : base(target)
	{
	}

	public override void show()
	{
		Debug.Log("炸彈");
	}
}

public class SpringEffect : Effectable
{
	public SpringEffect(GameObject target) : base(target)
	{
	}

	public override void show()
	{
		Debug.Log("彈簧");
	}
}

public class ItemEatEffect : Effectable
{
	public float alphaNumber;
	public ItemEatEffect(GameObject target) : base(target)
	{
		setShowTime(0.5f);
        setDelayTime(0);
        setAlpha(0);	
	}

	public override void show()
	{
		Debug.Log("吃到");
		iTween.MoveAdd(m_target, iTween.Hash("easetype", iTween.EaseType.easeInOutBounce,
                                                       "amount", new Vector3(0, 1, 0),
                                                        "time", showTime,
                                                        "delay", delayTime));
        iTween.FadeTo(m_target, iTween.Hash("time", showTime,
                                            "delay", delayTime,
                                            "alpha", alphaNumber,
		                                    "easetype", iTween.EaseType.easeInBack));
	}
	public void setAlpha(float alpha)
    {
        alphaNumber = alpha;
    }
}

