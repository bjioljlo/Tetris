using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Grid {
	//使用單例模式
	private static Grid m_Grid = null;
	private Spawner m_Spawner = null;
    private Grid(){}
    public static Grid getGrid
    {
		get
		{
			if(m_Grid == null)
			{
				m_Grid = new Grid();
			}
			return m_Grid;
		}
    }


	public int Width = 8;//寬度
	public int Height = 20;//每一次新增的高度
    public int Height_Now = 0;//目前grid的記錄高度
    public int InitTimes = 0;//背景增加次數
    //public static Transform[,] grids = new Transform[Width, Height];
    public List<List<Transform>> grids = new List<List<Transform>>();
    public bool IsStart = false;//和遊戲開始有關的更新開關
	public bool IsPause = true;//和暫停有關的更新開關
    public int Score = 0;//目前分數
	int m_BestScore = 0;
	public int BestScore 
	{ 
		get { 
			return m_BestScore; 
		} set { 
			m_BestScore = value;
			PlayerManager.get_main_playerInfo().Highscore = m_BestScore;
		}
	}//最好成績
	public int Coin = 0;//當下金幣數量
	public GameObject GO_lastBox = null;
    public float ManSpeed = 0.5f;//人物行走速度
    public float BoxSpeed = 1f;//方塊下降速度
	public int BagLeftBox = 20;//包包中剩餘的方塊數量
	public int WatchAdsTimer = 5;//可以接關的秒數
	public int CoinNumber = 5;//吃到金幣增加數量

    


	public void ResetGrid()
	{
		m_Grid = null;
	}

	public void LoadFile()
    {
		if(PlayerManager.get_main_playerInfo() != null)
		{
			BestScore = PlayerManager.get_main_playerInfo().Highscore;
			Coin = PlayerManager.get_main_playerInfo().GoldCoin;
		}
		else
		{
			BestScore = PlayerPrefs.GetInt("BestScore");
		}
		Best_Text best_Text = GameObject.FindObjectOfType<Best_Text>();
        best_Text.SetText(BestScore.ToString());
    }

    void SaveFile()
    {
		//有登入帳號
		if (PlayerManager.get_main_playerInfo() != null && PlayerManager.get_main_playerInfo().Name != "Player")
		{
			//---更新database的highscore
			playerInfo temp_playerInfo = PlayerManager.get_main_playerInfo();
			if(temp_playerInfo.Highscore > BestScore)
			{
				return;
			}
			temp_playerInfo.Highscore = BestScore;
			PlayerManager.set_mainPlayer(temp_playerInfo);
			PlayerManager.update_highscore(PlayerManager.get_main_playerInfo());
			//---更新database的highscore
		}
		else//無登入帳號
		{
			//PlayerPrefs.SetInt("BestScore", BestScore);
			PlayerManager.SavePlayerInfo_Local(PlayerManager.get_main_playerInfo());
		}
        
    }

	public void setSpawner(Spawner spawner)
	{
		m_Spawner = spawner;
	}

	public void InitGrids()
    {
        Height_Now = Height * (InitTimes + 1);
        for (int x = 0; x < Width; x++)
        {
            grids.Add(new List<Transform>());
            for (int y = Height_Now - Height; y < Height_Now; y++)
            {
                grids[x].Add(null);
            }
        }
        InitTimes++;
		if(IsPause)//init結束後才可以開始動作
		{
			IsPause = false;
		}

    }
    

	public void GameOver()
    {
		IsPause = true;
		if (!m_Spawner)
			Debug.LogError("m_spawner is null!!!");
		m_Spawner.GameOver();
		SoundManager.StopBGM();
        
        if (Score > BestScore)
        {
            BestScore = Score;
        }
		SaveFile();
    }


	public Vector2 roundVec2(Vector2 v)
	{
		return new Vector2 (Mathf.Round (v.x), Mathf.Round (v.y));
	}

	public bool insideBorder(Vector2 pos)
	{
		return ((int)pos.x >= 0 &&
			(int)pos.x < Width &&
			(int)pos.y >= 0);
	}

	public void deleteRow(int y) {
		Debug.Log ("deleteRow()");
		if (!m_Spawner)
            Debug.LogError("m_spawner is null!!!");
		for (int x = 0; x < Width; ++x) {
			m_Spawner.Destroy(grids[x][y].gameObject);
			//Destroy (grids [x][y].gameObject);
            grids [x][y] = null;
		}
	}

	public void decreaseRow(int y) {
		Debug.Log ("decreaseRow(" + y + ")");
		for (int x = 0; x < Width; ++x) {
            if (grids[x][y] != null) {
				// Move one towards bottom
                grids[x][y-1] = grids[x][y];
                grids[x][y] = null;

				// Update Block position
                grids[x][y-1].position += new Vector3(0, -1, 0);
			}
		}
	}

	public void decreaseRowsAbove(int y) {
		Debug.Log ("decreaseRowsAbove(" + y + ")");
        for (int i = y; i < Height_Now; ++i)
			decreaseRow(i);
	}

	public bool isRowFull(int y) {
		Debug.Log ("isRowFull(" + y + ")");
		for (int x = 0; x < Width; ++x)
            if (grids[x][y] == null)
				return false;
		return true;
	}

	public void deleteFullRows() {
		Debug.Log ("deleteFullRows()");
        for (int y = 0; y < Height_Now; ++y) {
			if (isRowFull(y)) {
				deleteRow(y);
				decreaseRowsAbove(y+1);
				--y;
			}
		}
	}
}
