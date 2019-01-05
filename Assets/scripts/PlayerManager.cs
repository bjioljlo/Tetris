using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerManager : IManager {
 
	//使用單例模式
	private static PlayerManager m_PlayerManager = null;
	private PlayerManager() { }
	public static PlayerManager getPlayerManager
    {
        get
        {
			if (m_PlayerManager == null)
            {
				m_PlayerManager = new PlayerManager();
            }
			return m_PlayerManager;
        }
    }

	static playerInfo main_playerInfo;
	static List<playerInfo> top10_playerInfo;
	static Webserver m_webserver;
	static Text txt_playerName;
	static Text txt_playerHighscore;
	static Text txt_MessegeBox;
	public static InputField In_Account;
	public static InputField In_Passwd;
	public static List<GameObject> List_Obj_Top10;
	public static ScrollRect sr_Top10;
	public static float time_now;

	static Button btn_login;
	static Button btn_logout;
    
	private void Awake()
    {
		if (m_PlayerManager == null)
        {
			m_PlayerManager = this;
            DontDestroyOnLoad(this);
        }
		else if (this != m_PlayerManager)
        {
            Destroy(gameObject);
        }

		main_playerInfo = LoadPlayerInfo_Local();
    }

	private void Start()
	{
		m_webserver = gameObject.AddComponent<Webserver>();
        m_webserver.set_playerManager(this);

		InitPlayerManager();

	}

	private void Update()
	{
		if (!txt_playerName)
		{
			InitPlayerManager();
		}

		if(top10_playerInfo == null)
		{
			retop10();
		}

		if(main_playerInfo == null)
		{
			btn_login.gameObject.SetActive(true);
			btn_logout.gameObject.SetActive(false);
		}
		else
		{
			btn_login.gameObject.SetActive(false);
			btn_logout.gameObject.SetActive(true);
		}
	}

	private void InitPlayerManager()
	{
		txt_MessegeBox = GameObject.Find("Txt_LoginMsgBox").GetComponent<Text>();
		txt_playerName = GameObject.Find("Txt_PlayerName").GetComponent<Text>();
		txt_playerHighscore = GameObject.Find("Txt_Highscore").GetComponent<Text>();
		In_Account = GameObject.Find("In_account").GetComponent<InputField>();
		In_Passwd = GameObject.Find("In_password").GetComponent<InputField>();
		sr_Top10 = GameObject.Find("Scv_RanlBoard").GetComponent<ScrollRect>();
		sr_Top10.onValueChanged.AddListener(delegate { retop10(); });

		btn_login = GameObject.Find("Btn_Login").GetComponent<Button>();
		btn_logout = GameObject.Find("Btn_Logout").GetComponent<Button>();

		List_Obj_Top10 = new List<GameObject>();
		for (int i = 0; i < 10;i++)
		{
			GameObject temp = GameObject.Find("Rank_Box (" + i + ")");
			if (temp == null)
			{
				break;
			}
			List_Obj_Top10.Add(temp);
			List_Obj_Top10[i].SetActive(false);
		}

		get_top10();

		time_now = Time.time;
		if(main_playerInfo != null)
		{
			login_player(main_playerInfo);
		}
	}

	public static void SavePlayerInfo_Local(playerInfo playerInfo)
	{
		if(playerInfo == null)
		{
			PlayerPrefs.DeleteAll();
			return;
		}
		PlayerPrefs.SetString("PlayerName", playerInfo.Name);
		PlayerPrefs.SetString("PlayerPasswd", playerInfo.Passwd);
		PlayerPrefs.SetInt("PlayerHighscore", playerInfo.Highscore);
	}

	public static playerInfo LoadPlayerInfo_Local()
	{
		string Name, Passwd;
		int Highscore;
		Name = PlayerPrefs.GetString("PlayerName");
		Passwd = PlayerPrefs.GetString("PlayerPasswd");
		Highscore = PlayerPrefs.GetInt("PlayerHighscore",0);
		playerInfo temp = new playerInfo(Name, Passwd);
		temp.Highscore = Highscore;
		if (Name == "")
		{
			return null;
		}
		return temp;
	}

	public static void retop10()
    {
        if (Time.time - time_now > 1)
        {
            get_top10();
            time_now = Time.time;
			//Debug.LogError("get top10!");
        }
    }
    
	public static Webserver get_webserver()
	{
		return m_webserver;
	}

	public static void set_AllTop10(playerInfo[] playerInfo)
	{
		if (playerInfo == null || playerInfo[0] == null)//如果傳入空資料直接跳出
		{
			return;
		}
		top10_playerInfo = new List<playerInfo>();
		for (int i = 0; i < playerInfo.Length;i++)
		{
			top10_playerInfo.Add(playerInfo[i]);         
		}
		for (int i = 0; i < List_Obj_Top10.Count;i++)
		{
			if(i >= top10_playerInfo.Count)
			{
				List_Obj_Top10[i].SetActive(false);
			}
			else
			{
				List_Obj_Top10[i].SetActive(true);
				List_Obj_Top10[i].transform.Find("Txt_name").GetComponent<Text>().text = top10_playerInfo[i].Name;
                List_Obj_Top10[i].transform.Find("Txt_highscore").GetComponent<Text>().text =
                    top10_playerInfo[i].Highscore.ToString();
                List_Obj_Top10[i].transform.Find("Txt_rank").GetComponent<Text>().text = (i + 1).ToString();
			}

		}
	}

	public static void set_Board()
	{
		if(main_playerInfo == null)
		{
			txt_playerName.text = "";
			txt_playerHighscore.text = "";
		}
		else
		{
			txt_playerName.text = main_playerInfo.Name;
            txt_playerHighscore.text = main_playerInfo.Highscore.ToString();
		}

	}

	public static void set_MessegeBox(string str)
	{
		txt_MessegeBox.text = str;
		Debug.Log("[MessegeBox]: " + str);
	}

	public static void get_top10()
	{
		m_webserver.get_top10();
	}

	public static void update_highscore(playerInfo playerInfo)
	{
		m_webserver.update_highscore(playerInfo);
	}

	public static void create_player(playerInfo playerInfo)
    {
		m_webserver.create_player(playerInfo);
    }

	public static void login_player(playerInfo playerInfo)
	{
		m_webserver.login_player(playerInfo);
	}

	public static void logout_player()
	{
		m_webserver.logout_player();
	}

	public static void set_mainPlayer(playerInfo playerInfo)
    {
        main_playerInfo = playerInfo;
    }	
	public static playerInfo get_main_playerInfo()
	{
		return main_playerInfo;
	}

	static bool IsString(string str)
	{
		int temp;
		if (int.TryParse(str, out temp))
        {
			return false;
        }
        else
        {
			return true;
        }
	}

	public static bool check_playerInfoFormat_right(playerInfo playerInfo)
	{
		if(!IsString(playerInfo.Name))
		{
			set_MessegeBox("your account format woring");
			return false;
		}

		if(!IsString(playerInfo.Passwd))
		{
			set_MessegeBox("your passwd format woring");
			return false;
		}

		return true;
	}

}

public class playerInfo
{
	public playerInfo(string name,string passwd)
	{
		Name = name;
		Passwd = passwd;
	}
	public string ID;
	public string Name;
	public int Highscore;
	public string Passwd;
}
