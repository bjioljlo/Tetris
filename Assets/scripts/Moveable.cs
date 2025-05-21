using UnityEngine;
using UnityEngine.UI;

public abstract class Moveable
{
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
}
