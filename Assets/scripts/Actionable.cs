using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actionabl
{
    //控制的物件
    public GameObject m_target;
    //執行特效
    public abstract void ActionStart();
	public Actionabl(GameObject target)
	{
		m_target = target;
	}

}

public class BagAction : Actionabl
{
	public Effectable eatEffect;
	public BagAction(GameObject target) : base(target)
	{
		setEatEffect(new ItemEatEffect(target));
	}

	public override void ActionStart()
	{
		if (eatEffect != null)
			eatEffect.show();
	}
	public void setEatEffect(Effectable effectable)
    {
        eatEffect = effectable;
    }
}

public class ManStopAction : Actionabl
{
	public Effectable stopEffect;
	public ManStopAction(GameObject target) : base(target)
	{
		setStopEffect(new StopEffect(target));
	}

	public override void ActionStart()
	{
		if (stopEffect != null)
			stopEffect.show();
	}
	public void setStopEffect(Effectable effectable)
	{
		stopEffect = effectable;
	}
}

public class BentchAction : Actionabl
{
	public Effectable eatEffect;

	public BentchAction(GameObject target):base(target)
    {
		setEatEffect(new ItemEatEffect(target));
    }
    public override void ActionStart()
    {
		if(eatEffect != null)
		{
			eatEffect.show();
		}
    }

	public void setEatEffect(Effectable effectable)
	{
		eatEffect = effectable;
	}
}

public class BombAction : Actionabl
{
	public Effectable eatEffect;
	public Effectable bombEffect;
	public BombAction(GameObject target) : base(target)
	{
		setBombEffect(new BombEffect(target));
		setEatEffect(new ItemEatEffect(target));
	}

	public override void ActionStart()
	{
		if (bombEffect != null)
			bombEffect.show();
		if (eatEffect != null)
			eatEffect.show();
	}

	public void setBombEffect(Effectable effectable)
	{
		bombEffect = effectable;
	}
	public void setEatEffect(Effectable effectable)
	{
		eatEffect = effectable;
	}
}

public class SpringAction : Actionabl
{
	public Effectable eatEffect;
	public Effectable springEffect;
	public SpringAction(GameObject target) : base(target)
	{
		setSpringEffect(new SpringEffect(target));
		setEatEffect(new ItemEatEffect(target));
	}

	public override void ActionStart()
	{
		if (springEffect != null)
			springEffect.show();
		if (eatEffect != null)
			eatEffect.show();
	}

	public void setSpringEffect(Effectable effectable)
	{
		springEffect = effectable;
	}
	public void setEatEffect(Effectable effectable)
	{
		eatEffect = effectable;
	}
}