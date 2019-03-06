using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class Group : IMainBehavier {
    //記錄前一次下降的時間
    float lastFall = 0;
    // 是否要滿行消除的開關
    bool IsFullDelet = false;
    //人物
    public GameObject[] mans;
    //控制的按鈕
    public Button btn_left;
    public Button btn_right;
    public Button btn_down;
    public Button btn_row;
    //移動的事件
    public Moveable m_moveAct;

	public groupType Type;

	public enum groupType
	{
		Normal,bench,bomb,spring,bag,coin,
	}


	public void setGroupType(groupType type)
	{
		Type = type;
	}

	public groupType getGroupType()
	{
		return Type;
	}

    public void setMoveType(Moveable moveable)
    {
        m_moveAct = moveable;
        m_moveAct.btn_row = btn_row;
        m_moveAct.btn_down = btn_down;
        m_moveAct.btn_left = btn_left;
        m_moveAct.btn_right = btn_right;
    }

    // Use this for initialization
    void Start()
    {
        // Default position not valid? Then it's game over
        if (!isValidGridPos())
        {
            Grid.getGrid.GameOver();
        }
    }


    public override void IPauseUpdate () {
        if (!Grid.getGrid.IsStart)//檢查是否開始了
            return;

		// Move Left
        if (Input.GetKeyDown (KeyCode.LeftArrow)) {
            m_moveAct.goLeft();
		}

		// Move Right
		else if (Input.GetKeyDown (KeyCode.RightArrow)) {
            m_moveAct.goRight();
		}

		// Rotate
		else if (Input.GetKeyDown (KeyCode.UpArrow)) {
            m_moveAct.goRotation();
		}

        //fall with press button
        else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            m_moveAct.goDown();
        }

		// Fall
        else if (Time.time - lastFall >= Grid.getGrid.BoxSpeed || m_moveAct.downPush == true) {
			// Modify position
			transform.position += new Vector3 (0, -1, 0);
			transform.position = Grid.getGrid.roundVec2 (transform.position);

			// See if valid
			if (isValidGridPos ()) {
               
				// It's valid. Update grid.
				updateGrid ();

			}
			else {
				// It's not valid. revert.
				transform.position += new Vector3 (0, 1, 0);

				// Clear filled horizontal lines
                if(IsFullDelet)
                {
                    Grid.getGrid.deleteFullRows(); 
                }

				// Spawn next Group
				FindObjectOfType<Spawner> ().spawnNext ();

                //扣除包包數量
				Grid.getGrid.BagLeftBox = Grid.getGrid.BagLeftBox - 1;
				mans[0].GetComponent<Man>().setHpBar((float)Grid.getGrid.BagLeftBox);

                //將這次的方塊放入紀錄，以便看完廣告後刪除
				//Grid.getGrid.GO_lastBox = this.gameObject;

                //恢復可以按下往下按鈕
                m_moveAct.downPush = false;

                //移除監聽
                m_moveAct.DeletBtnListener();

                //變換Tag成為
                setTag();

			}
			lastFall = Time.time;
		}
	}

    public abstract void setTag();

    bool isValidGridPos() {        
		foreach (Transform child in transform) {
			Vector2 v = Grid.getGrid.roundVec2(child.position);

			// Not inside Border?
			if (!Grid.getGrid.insideBorder(v))
				return false;

			// Block in grid cell (and not part of same group)?
			if (Grid.getGrid.grids [(int)v.x] [(int)v.y] && //有東西
			    Grid.getGrid.grids [(int)v.x] [(int)v.y].parent != transform && //不是自己
			    Grid.getGrid.grids [(int)v.x] [(int)v.y].GetComponentInParent<Group>().getGroupType() == groupType.Normal)//是一般方塊
				return false;
		}
		return true;
	}

	void updateGrid() {
		if(Grid.getGrid.IsPause)
        {
          return;
        }
		// Remove old children from grid
        for (int y = 0; y < Grid.getGrid.Height_Now; ++y) {
			for (int x = 0; x < Grid.getGrid.Width; ++x) {
                if (Grid.getGrid.grids [x][y] != null) {
                    if (Grid.getGrid.grids [x][y].parent == transform) {
                        Grid.getGrid.grids [x][y] = null;
					}
				}
			}
		}

		// Add new children to grid
		foreach (Transform child in transform) {
			Vector2 v = Grid.getGrid.roundVec2(child.position);
			Grid.getGrid.grids[(int)v.x] [(int)v.y] = child;
		}        
	}

    public void deletThisGroup()
    {
        Debug.Log("delet grid record");
        for (int y = 0; y < Grid.getGrid.Height_Now; ++y)
        {
            for (int x = 0; x < Grid.getGrid.Width; ++x)
            {
                if (Grid.getGrid.grids[x][y] != null)
                {
                    if (Grid.getGrid.grids[x][y].parent == transform)
                    {
                        Grid.getGrid.grids[x][y] = null;
                    }
                }
            }
        }
    }


    public bool isValidPlayerPos()
    {
        foreach(GameObject childGO in mans)
        {
            Vector2 v1 = Grid.getGrid.roundVec2(childGO.transform.position);
            foreach (Transform childTR in transform)
            {
                Vector2 v2 = Grid.getGrid.roundVec2(childTR.position);
                if (v1 == v2)
                    return false;
            }
        }
        return true;
    }

    //void AddBtnListener()
    //{
    //    btn_left.onClick.AddListener(moveLeft);
    //    btn_right.onClick.AddListener(moveRight);
    //    btn_row.onClick.AddListener(rotate);
    //    btn_down.onClick.AddListener(fillWithPress);
    //}

    //void DeletBtnListener()
    //{
    //    btn_left.onClick.RemoveListener(moveLeft);
    //    btn_right.onClick.RemoveListener(moveRight);
    //    btn_row.onClick.RemoveListener(rotate);
    //    btn_down.onClick.RemoveListener(fillWithPress);
    //}


    //void moveLeft()
    //{
    //    if (Grid.getGrid.IsPause)
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
    //    if (Grid.getGrid.IsPause)
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
    //    if (Grid.getGrid.IsPause)
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
    //    if (Grid.getGrid.IsPause)
    //        return;
        
    //    Debug.Log("fillWithPress");
    //    downPush = true;
    //}

  
}
