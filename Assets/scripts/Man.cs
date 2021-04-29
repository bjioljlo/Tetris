using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class Man : IMainBehavier {

    Score_Text score_text;
    Best_Text best_text;
	Coin_Text coin_text;

	public Actionabl itemAction;

	public ManMoveable ManMove;

	public Group itemGroup;

	public ManMoveFactory CreatManMoveFac;

	public Slider HpBar;
	public GameObject Hpcolor;
	public GameObject HpText;

	AudioClip Sound_Jump;
	AudioClip Sound_EatBag;

	SpriteRenderer m_sprite;

	private void Awake()
	{
		Grid.getGrid.Man = this;
	}

	public void setManMoveable(ManMoveable manMoveable)
	{
		ManMove = manMoveable;
	}

	private void Start()
	{
        score_text = FindObjectOfType<Score_Text>();
        best_text = FindObjectOfType<Best_Text>();
		coin_text = FindObjectOfType<Coin_Text>();
		Grid.getGrid.LoadFile();
        best_text.SetText(Grid.getGrid.BestScore.ToString());
		coin_text.SetText(Grid.getGrid.Coin.ToString());

        Grid.getGrid.Score = 0;
        //score_text.SetText(Grid.getGrid.Score.ToString());

		setManMoveable(new NormalManMove(this.gameObject,null));

		CreatManMoveFac = new CreateMoveableFactory();

		HpBar = FindObjectOfType<Slider>();
		setHpBar((float)Grid.getGrid.BagLeftBox);

		Sound_Jump = Resources.Load<AudioClip>("Sound_Jump");
		Sound_EatBag = Resources.Load<AudioClip>("Sound_EatBag");

		m_sprite = transform.GetComponentInChildren<SpriteRenderer>();
	}


	//public void DebugManAddSpeed(string str)
   // {
   //     try
   //     {
   //         float temp = float.Parse(str);
   //         if (temp > 0)
			//{
			//	Grid.getGrid.ManSpeed = temp;
			//	setManMoveable(new NormalManMove(this.gameObject, null));
			//}
    //    }
    //    catch
    //    {
    //        Debug.LogError("輸入錯誤 " + str);
    //    }
    //}

	public override void IPauseUpdate()
	{
		if (Grid.getGrid.GO_lastBox != null)
			return;
		if (ManMove.tempitw == null)
        {
			if (ManMove.IsEnd)//道具結束的標記，結束後回歸一般模式
            {
				setManMoveable(new NormalManMove(this.gameObject, ManMove));
            }
			ManMove.Move();
			AddScore();//加分
        }
        else
        {
			//吃到道具做動作
			if(itemAction != null)
			{
				itemAction.ActionStart();//人物吃到道具的道具反應
				setManMoveable(CreatManMoveFac.CreatManMove(this.gameObject, ManMove, itemGroup));//設定吃到道具之後的效果
				if(itemGroup.Type == Group.groupType.bag)
				{
					AddBagBox();
				}
				if(itemGroup.Type == Group.groupType.coin)
				{
					AddCoinBox();
				}
				itemAction = null;
				return;
			}


            //移動方向有BOX擋住，將人物退回原位
			if(Grid.getGrid.grids[(int)ManMove.New_Pos.x][(int)ManMove.New_Pos.y] != null)
            {
				if (Grid.getGrid.grids[(int)ManMove.New_Pos.x][(int)ManMove.New_Pos.y].parent.tag == "itemBox")
                    return;
				ManMove.tempitw.isRunning = false;
				Destroy(ManMove.tempitw);
				ManMove.Move();
            }
        }
  
	}

    void AddScore()
    {
		if ((int)ManMove.Old_Pos.y > Grid.getGrid.Score)
		{
			SoundManager.m_Effect.PlayOneShot(Sound_Jump);
		}
		Grid.getGrid.Score = (int)ManMove.Old_Pos.y;
        //score_text.SetText(Grid.getGrid.Score.ToString());
  //      if (Grid.getGrid.Score > Grid.getGrid.BestScore)
		//{
		//	//best_text.SetText(Grid.getGrid.Score.ToString());
		//	Grid.getGrid.BestScore = Grid.getGrid.Score;
		//}
            
    }

	public void AddBagBox()
	{
		Grid.getGrid.BagLeftBox = Grid.getGrid.BagLeftBox + Grid.getGrid.Height;
		setHpBar((float)Grid.getGrid.BagLeftBox);
		SoundManager.m_Effect.PlayOneShot(Sound_EatBag);
	}

	public void AddCoinBox()
	{
		PlayerManager.AddWithCoin(Grid.getGrid.CoinNumber);
		Grid.getGrid.Coin = PlayerManager.get_main_playerInfo().GoldCoin;
		//coin_text.SetText(Grid.getGrid.Coin.ToString());
	}

	public void AddCoinBox(int num)
	{
		PlayerManager.AddWithCoin(num);
        Grid.getGrid.Coin = PlayerManager.get_main_playerInfo().GoldCoin;
        //coin_text.SetText(Grid.getGrid.Coin.ToString());
	}

	public void setHpBar(float hpNum)
	{
		if (hpNum <= 0)
		{
			Grid.getGrid.GameOver();
			return;
		}
			

		int HpNum = (int)hpNum;
		float HpColorNum = HpNum / 10;
		int HpBarNum = HpNum % 10;
		if(HpBarNum == 0)
		{
			HpColorNum--;
			HpBarNum = 10;
		}

		HpBar.value = HpBarNum;

		HpText.GetComponent<Text>().text = HpNum.ToString();
		if(HpColorNum>5)
		{
			Hpcolor.GetComponent<Image>().color = new Color((255 - (HpColorNum * 25)) / 255, 1, 0);
		}
		else
		{
			Hpcolor.GetComponent<Image>().color = new Color(1, HpColorNum * 25 / 255, 0);
		}

	}

	public void setItemAction(Actionabl actionabl)
	{
		itemAction = actionabl;
	}

	public void setItemGroup(Group group)
	{
		itemGroup = group;
	}


	public void setManSkin(Sprite nowSkin)
	{
		m_sprite.sprite = nowSkin;
	}
}
