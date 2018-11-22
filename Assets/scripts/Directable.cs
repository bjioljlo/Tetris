using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Directable {

    public GameObject m_target;
    public Directable(GameObject target)
    {
        m_target = target;
    }
    public abstract void GO();
    public bool isValidGridPos()
    {
        foreach (Transform child in m_target.transform)
        {
            Vector2 v = Grid.getGrid.roundVec2(child.position);

            // Not inside Border?
            if (!Grid.getGrid.insideBorder(v))
                return false;

            // Block in grid cell (and not part of same group)?
            if (Grid.getGrid.grids[(int)v.x][(int)v.y] != null &&
                Grid.getGrid.grids[(int)v.x][(int)v.y].parent != m_target.transform &&
			    Grid.getGrid.grids[(int)v.x][(int)v.y].GetComponentInParent<Group>().getGroupType() == Group.groupType.Normal)//是一般方塊
                return false;
        }
        return true;
    }

    public void updateGrid()
    {
        // Remove old children from grid
        for (int y = 0; y < Grid.getGrid.Height_Now; ++y)
        {
            for (int x = 0; x < Grid.getGrid.Width; ++x)
            {
                if (Grid.getGrid.grids[x][y] != null)
                {
                    if (Grid.getGrid.grids[x][y].parent == m_target.transform)
                    {
                        Grid.getGrid.grids[x][y] = null;
                    }
                }
            }
        }

        // Add new children to grid
        foreach (Transform child in m_target.transform)
        {
            Vector2 v = Grid.getGrid.roundVec2(child.position);
            Grid.getGrid.grids[(int)v.x][(int)v.y] = child;
        }
    }
}


public class DirGoLeft : Directable
{
    public DirGoLeft(GameObject target) : base(target)
    {
    }

    public override void GO()
    {
        if (Grid.getGrid.IsPause)
            return;
		if (m_target == null)
			return;

		if (m_target.GetComponent<Group>().Type == Group.groupType.bag)
			return;

        Debug.Log("moveLeft");
        // Modify position
        m_target.transform.position += new Vector3(-1, 0, 0);

        // See if valid
        if (isValidGridPos())
            // Its valid. Update grid.
            updateGrid();
        else
            // Its not valid. revert.
            m_target.transform.position += new Vector3(1, 0, 0);
    }
}

public class DirGoRight : Directable
{
    public DirGoRight(GameObject target) : base(target)
    {
    }

    public override void GO()
    {
        if (Grid.getGrid.IsPause)
            return;

		if (m_target == null)
            return;

		if (m_target.GetComponent<Group>().Type == Group.groupType.bag)
            return;

        Debug.Log("moveRight");
        // Modify position
        m_target.transform.position += new Vector3(1, 0, 0);

        // See if valid
        if (isValidGridPos())
            // It's valid. Update grid.
            updateGrid();
        else
            // It's not valid. revert.
            m_target.transform.position += new Vector3(-1, 0, 0);
    }
}

public class DirGoDown : Directable
{
    public DirGoDown(GameObject target) : base(target)
    {
    }

    public override void GO()
    {
        if (Grid.getGrid.IsPause)
            return;

		if (m_target == null)
            return;

		if (m_target.GetComponent<Group>().Type == Group.groupType.bag)
            return;

        Debug.Log("fillWithPress");
    }
}

public class DirGoRotation : Directable
{
    public DirGoRotation(GameObject target) : base(target)
    { }
    public override void GO()
    {
        if (Grid.getGrid.IsPause)
            return;

		if (m_target == null)
            return;

		if (m_target.GetComponent<Group>().Type == Group.groupType.bag)
            return;

        Debug.Log("rotate");
        m_target.transform.Rotate(0, 0, -90);

        // See if valid
        if (isValidGridPos())
            // It's valid. Update grid.
            updateGrid();
        else
            // It's not valid. revert.
            m_target.transform.Rotate(0, 0, 90);
    }
}
