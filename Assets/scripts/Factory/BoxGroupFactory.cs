using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BoxGroupFactory  
{
   
    public enum GroupType
    {
        normalGroup,itemGroup
    }
    public enum ItemActionType
    {
        BentchAction,none
    }
    public GameObject m_target;
    
    public abstract GameObject CreatGroupByName(GroupType name);
    public void setTarget(GameObject target)
    {
        m_target = target;
    }

    

}

public class CreatGroupFactoryByName : BoxGroupFactory
{
    GameObject[] m_mans;

    public Button btn_left;
    public Button btn_right;
    public Button btn_down;
    public Button btn_row;

    public CreatGroupFactoryByName(GameObject[] mans,Button left,Button right,Button down,Button row)
    {
        btn_left = left;
        btn_right = right;
        btn_down = down;
        btn_row = row;
        m_mans = mans;
    }
    
    public override GameObject CreatGroupByName(GroupType name)
    {
        //Group groupTemp;
        switch (name)
        {
			case GroupType.itemGroup:
				return new ConcreteItemGroup(m_mans, btn_left, btn_right, btn_down, btn_row).Create(m_target);
            case GroupType.normalGroup:
				return new ConcreteNormalGeoup(m_mans, btn_left, btn_right, btn_down, btn_row).Create(m_target);
            default:
                Debug.LogError(name + "無法生產物件");
                break;
        }
        return null;
    }
    //Group setGroup(Group group)
    //{
    //    group.mans = m_mans;
    //    group.btn_row = btn_row;
    //    group.btn_down = btn_down;
    //    group.btn_left = btn_left;
    //    group.btn_right = btn_right;
    //    group.setMoveType(new NormalMove(m_target));
    //    group.m_moveAct.AddBtnListener();
    //    return group;
    //}

	//Actionabl GetActionabl()
    //{
    //    string ImageName = m_target.GetComponentInChildren<SpriteRenderer>().sprite.name;
    //    switch (ImageName)
    //    {
    //        case "bench":
    //            return new BentchAction(m_target);
    //        case "bomb":
    //            return new BombAction(m_target);
    //        case "spring":
    //            return new SpringAction(m_target);
    //        default:
    //            Debug.LogError("GetActionabl is wrong!");
    //            return null;
    //    }

    //}

	//Actionabl GetManActionabl()
    //{
    //    string ImageName = m_target.GetComponentInChildren<SpriteRenderer>().sprite.name;
    //    switch (ImageName)
    //    {
    //        case "bench":
				//return new ManStopAction(m_mans[0]);
    //        case "bomb":
				//return new ManStopAction(m_mans[0]);
    //        case "spring":
				//return new ManStopAction(m_mans[0]);
    //        default:
    //            Debug.LogError("GetActionabl is wrong!");
    //            return null;
    //    }

    //}
}


public abstract class Creator
{
	public GameObject[] m_mans;

    public Button btn_left;
    public Button btn_right;
    public Button btn_down;
    public Button btn_row;
	public Group groupTemp;
	public GameObject m_target;

	public Creator(GameObject[] mans, Button left, Button right, Button down, Button row)
	{
		btn_left = left;
        btn_right = right;
        btn_down = down;
        btn_row = row;
        m_mans = mans;
	}

	public abstract GameObject Create(GameObject target);
	public Group setGroup(Group group)
    {
        group.mans = m_mans;
        group.btn_row = btn_row;
        group.btn_down = btn_down;
        group.btn_left = btn_left;
        group.btn_right = btn_right;
        group.setMoveType(new NormalMove(m_target));
        group.m_moveAct.AddBtnListener();
        return group;
    }
}


public class ConcreteNormalGeoup : Creator
{
	public ConcreteNormalGeoup(GameObject[] mans, Button left, Button right, Button down, Button row) : base(mans, left, right, down, row)
	{
	}

	public override GameObject Create(GameObject target)
	{
		m_target = target;
		groupTemp = m_target.AddComponent<NormalGroup>();
		groupTemp = setGroup(groupTemp);
		groupTemp.setGroupType(Group.groupType.Normal);
		return m_target;
	}
}

public class ConcreteItemGroup : Creator
{
	public ConcreteItemGroup(GameObject[] mans, Button left, Button right, Button down, Button row) : base(mans, left, right, down, row)
	{
	}

	public override GameObject Create(GameObject target)
	{
		m_target = target;
		groupTemp = m_target.AddComponent<ItemGroup>();
        m_target.GetComponent<ItemGroup>().setActionType(GetActionabl());
        m_target.GetComponent<ItemGroup>().setManItemAct(GetManActionabl());
        groupTemp = setGroup(groupTemp);
		return m_target;
	}

	Actionabl GetActionabl()
    {
        string ImageName = m_target.GetComponentInChildren<SpriteRenderer>().sprite.name;
        switch (ImageName)
        {
            case "bench":
				groupTemp.setGroupType(Group.groupType.bench);
                return new BentchAction(m_target);
            case "bomb":
				groupTemp.setGroupType(Group.groupType.bomb);
                return new BombAction(m_target);
            case "spring":
				groupTemp.setGroupType(Group.groupType.spring);
                return new SpringAction(m_target);
			case "bag":
				groupTemp.setGroupType(Group.groupType.bag);
				groupTemp.setTag();
				return new BagAction(m_target);
            default:
                Debug.LogError("GetActionabl is wrong!");
                return null;
        }

    }

	Actionabl GetManActionabl()
    {
        string ImageName = m_target.GetComponentInChildren<SpriteRenderer>().sprite.name;
        switch (ImageName)
        {
            case "bench":
                return new ManStopAction(m_mans[0]);
            case "bomb":
                return new ManStopAction(m_mans[0]);
            case "spring":
                return new ManStopAction(m_mans[0]);
			case "bag":
				return new ManStopAction(m_mans[0]);
            default:
                Debug.LogError("GetManActionabl is wrong!");
                return null;
        }

    }
}

