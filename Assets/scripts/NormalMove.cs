using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMove : Moveable{

    public Directable Left;
    public Directable Right;
    public Directable Down;
    public Directable Row;

    public NormalMove(GameObject target)
    {
        m_target = target;
        setLeft(new DirGoLeft(target));
        setRight(new DirGoRight(target));
        setDown(new DirGoDown(target));
        setRow(new DirGoRotation(target));
    }

    public override void AddBtnListener()
    {
        btn_left.onClick.AddListener(goLeft);
        btn_right.onClick.AddListener(goRight);
        btn_down.onClick.AddListener(goDown);
        btn_row.onClick.AddListener(goRotation);
    }

    public override void DeletBtnListener()
    {
        btn_left.onClick.RemoveListener(goLeft);
        btn_right.onClick.RemoveListener(goRight);
        btn_down.onClick.RemoveListener(goDown);
        btn_row.onClick.RemoveListener(goRotation);
    }


    public override void goLeft()
    {
        if (Left != null)
            Left.GO();
    }
    public override void goRight()
    {
        if (Right != null)
            Right.GO();
    }

    public override void goDown()
    {
        if (Down != null)
        {
            Down.GO();
            downPush = true;
        }
            
    }

    public override void goRotation()
    {
        if (Row != null)
            Row.GO();
    }

	public void setLeft(Directable directable)
    {
        Left = directable;
    }

    public void setRight(Directable directable)
    {
        Right = directable;
    }

    public void setDown(Directable directable)
    {
        Down = directable;
    }

    public void setRow(Directable directable)
    {
        Row = directable;
    }
}
