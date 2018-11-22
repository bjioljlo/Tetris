using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Moveable {
    //往下按鈕是否被按下了
    public bool downPush = false;
    //人物
    public GameObject[] mans;
    //控制的按鈕
    public Button btn_left;
    public Button btn_right;
    public Button btn_down;
    public Button btn_row;
    //控制的物件
    public GameObject m_target;

    public abstract void goLeft();
    public abstract void goRight();
    public abstract void goDown();
    public abstract void goRotation();
    public abstract void AddBtnListener();
    public abstract void DeletBtnListener();

	//bool isValidGridPos()
    //{
    //    foreach (Transform child in transform)
    //    {
    //        Vector2 v = Grid.roundVec2(child.position);

    //        // Not inside Border?
    //        if (!Grid.insideBorder(v))
    //            return false;

    //        // Block in grid cell (and not part of same group)?
    //        if (Grid.grids[(int)v.x][(int)v.y] != null &&
    //            Grid.grids[(int)v.x][(int)v.y].parent != transform)

    //            return false;
    //    }
    //    return true;
    //}

    //void updateGrid()
    //{
    //    // Remove old children from grid
    //    for (int y = 0; y < Grid.Height_Now; ++y)
    //    {
    //        for (int x = 0; x < Grid.Width; ++x)
    //        {
    //            if (Grid.grids[x][y] != null)
    //            {
    //                if (Grid.grids[x][y].parent == transform)
    //                {
    //                    Grid.grids[x][y] = null;
    //                }
    //            }
    //        }
    //    }

    //    // Add new children to grid
    //    foreach (Transform child in transform)
    //    {
    //        Vector2 v = Grid.roundVec2(child.position);
    //        Grid.grids[(int)v.x][(int)v.y] = child;
    //    }
    //}


    //bool isValidPlayerPos()
    //{
    //    foreach (GameObject childGO in mans)
    //    {
    //        Vector2 v1 = Grid.roundVec2(childGO.transform.position);
    //        foreach (Transform childTR in transform)
    //        {
    //            Vector2 v2 = Grid.roundVec2(childTR.position);
    //            if (v1 == v2)
    //                return false;
    //        }
    //    }
    //    return true;
    //}



    //public void moveLeft()
    //{
    //    if (Grid.IsPause)
    //        return;

    //    Debug.Log("moveLeft");
    //    // Modify position
    //    transform.position += new Vector3(-1, 0, 0);

    //    // See if valid
    //    if (isValidGridPos())
    //        // Its valid. Update grid.
    //        updateGrid();
    //    else
    //        // Its not valid. revert.
    //        transform.position += new Vector3(1, 0, 0);
    //}

    //void moveRight()
    //{
    //    if (Grid.IsPause)
    //        return;

    //    Debug.Log("moveRight");
    //    // Modify position
    //    transform.position += new Vector3(1, 0, 0);

    //    // See if valid
    //    if (isValidGridPos())
    //        // It's valid. Update grid.
    //        updateGrid();
    //    else
    //        // It's not valid. revert.
    //        transform.position += new Vector3(-1, 0, 0);
    //}

    //void rotate()
    //{
    //    if (Grid.IsPause)
    //        return;

    //    Debug.Log("rotate");
    //    transform.Rotate(0, 0, -90);

    //    // See if valid
    //    if (isValidGridPos())
    //        // It's valid. Update grid.
    //        updateGrid();
    //    else
    //        // It's not valid. revert.
    //        transform.Rotate(0, 0, 90);
    //}

    //void fillWithPress()
    //{
    //    if (Grid.IsPause)
    //        return;

    //    Debug.Log("fillWithPress");
    //    downPush = true;
    //}
}
