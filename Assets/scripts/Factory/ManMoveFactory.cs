using UnityEngine;

public abstract class ManMoveFactory
{

	public abstract ManMoveable CreatManMove(GameObject target, ManMoveable manMoveable, Group group);
}

public class CreateMoveableFactory : ManMoveFactory
{
	public override ManMoveable CreatManMove(GameObject target, ManMoveable manMoveable, Group group)
	{
		switch (group.Type)
		{
			case Group.groupType.Normal:
				return new NormalManMove(target, manMoveable);
			case Group.groupType.bench:
				return new StopManMove(target, manMoveable);
			case Group.groupType.bomb:
				return new StopManMove(target, manMoveable);
			case Group.groupType.spring:
				return new StrongManMove(target, manMoveable);
			case Group.groupType.bag:
				return new NormalManMove(target, manMoveable);
			case Group.groupType.coin:
				return new NormalManMove(target, manMoveable);
			default:
				Debug.LogError("CreateMoveableFactory error!!");
				break;
		}
		return null;
	}
}
