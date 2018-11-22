using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System;

public class Webserver : MonoBehaviour{
	string WebserverIP = "122.116.102.141";
	string create_webname = "creat_player.php";
	string login_webname = "login.php";
	string logout_webname = "logout.php";
	string highscore_webname = "update_highscore.php";
	string top10_webname = "get_top10.php";
    
	//PlayerManager m_playerManager;

	//-----------------網路上copy的
	public const int NotReachable = 0;                   // 沒有網路
    public const int ReachableViaLocalAreaNetwork = 1;   // 網路Wifi,網路線。
    public const int ReachableViaCarrierDataNetwork = 2; // 網路3G,4G。
	//Google IP
    string googleTW = "203.66.155.50";
    //YahooTW IP
    string yahooTW = "116.214.12.74";
	//-----------------網路上copy的

    //---------session參數
	public string sessionID;//登錄之後用來跟database查詢資料的ＩＤ
    private Dictionary<string, string> headers = new Dictionary<string, string>();//標頭檔


    void Start()
    {
        // IPhone, Android
        int nStatus = ConnectionStatus();
        Debug.Log("ConnectionStatus : " + nStatus);
        if (nStatus > 0)
            Debug.Log("有連線狀態");
        else
            Debug.Log("無連線狀態");

		StartCoroutine(PingConnect("218.161.4.121"));
    }

	public void set_playerManager(PlayerManager playerManager)
	{
		//m_playerManager = playerManager;
	}

	public void get_top10()
	{
		StartCoroutine(GetTop10_json());
	}

	public void update_highscore(playerInfo info)
	{
		StartCoroutine(UpdateHighscore_json(info));
	}

	public void login_player(playerInfo info)
	{
		StartCoroutine(Login_json(info));
	}

	public void logout_player()
	{
		StartCoroutine(Logout_json());
	}

	public void create_player(playerInfo info)
	{
		StartCoroutine(Addplayer_json(info));
	}

	IEnumerator GetTop10_json()
	{
		string url = "http://" + WebserverIP + "/" + top10_webname;
		var www = UnityWebRequest.Get(url);
        yield return www.Send();

		//接收新增的回傳資料
        string str_temp = www.downloadHandler.text;
        int errorNumber;

        if (int.TryParse(str_temp, out errorNumber))
        {
            Get_db_logerror(errorNumber);
            yield break;
        }
        else
        {
			PlayerManager.set_AllTop10(JsonConvert.DeserializeObject<playerInfo[]>(www.downloadHandler.text));
			//Debug.LogError("get top10 from sql");
        }

	}

	IEnumerator UpdateHighscore_json(playerInfo playerInfo)
	{
		string url = "http://" + WebserverIP + "/" + highscore_webname;
		WWWForm form = new WWWForm();
		form.AddField("playerinfo", playerdate_to_json(playerInfo));
		var www = UnityWebRequest.Post(url,form);
		yield return www.Send();

		//接收新增的回傳資料
        string str_temp = www.downloadHandler.text;
        int errorNumber;

        if (int.TryParse(str_temp, out errorNumber))
        {
            Get_db_logerror(errorNumber);
            PlayerManager.set_mainPlayer(null);
            yield break;
        }
        else
        {
            PlayerManager.set_mainPlayer(JsonConvert.DeserializeObject<playerInfo>(www.downloadHandler.text));
        }
        //GetSessionID(www.GetResponseHeaders());//得到ＷＥＢserver的ＳＥＳＳＩＯＮ表頭黨
        PlayerManager.set_Board();
	}

	IEnumerator Logout_json()
	{
		string url = "http://" + WebserverIP + "/" + logout_webname;
		var www = UnityWebRequest.Get(url);
		yield return www.Send();
        if (www.downloadHandler.text != "")
        {
            Debug.LogError("logout fail!!" + www.downloadHandler.text);
            yield break;
        }
		PlayerManager.set_mainPlayer(null);
		PlayerManager.set_Board();
		PlayerManager.SavePlayerInfo_Local(PlayerManager.get_main_playerInfo());
	}

	IEnumerator Login_json(playerInfo playerInfo)
	{
		string url = "http://" + WebserverIP + "/" + login_webname;
		WWWForm form = new WWWForm();
		form.AddField("playerinfo", playerdate_to_json(playerInfo));
		var www = UnityWebRequest.Post(url, form);
		yield return www.Send();

		//接收新增的回傳資料
		string str_temp = www.downloadHandler.text;
		int errorNumber;

		if (int.TryParse(str_temp, out errorNumber))
        {
			Get_db_logerror(errorNumber);
			PlayerManager.set_mainPlayer(null);
			yield break;
        }
        else
        {
			PlayerManager.set_mainPlayer(JsonConvert.DeserializeObject<playerInfo>(www.downloadHandler.text));
        }
		GetSessionID(www.GetResponseHeaders());//得到ＷＥＢserver的ＳＥＳＳＩＯＮ表頭黨
		PlayerManager.set_Board();
		Grid.getGrid.LoadFile();
		PlayerManager.SavePlayerInfo_Local(PlayerManager.get_main_playerInfo());
	}

	IEnumerator Addplayer_json(playerInfo playerInfo)
	{
		string url = "http://" + WebserverIP + "/" + create_webname;
		WWWForm form = new WWWForm();
		form.AddField("playerinfo", playerdate_to_json(playerInfo));
		var www = UnityWebRequest.Post(url, form);
		yield return www.Send();

		//接收新增的回傳資料
		string str_temp = www.downloadHandler.text;
		int errorNumber;
		if(int.TryParse(str_temp,out errorNumber))
		{
			Get_db_logerror(errorNumber);
			PlayerManager.set_mainPlayer(null);
			yield break;
		}
		else
		{
			PlayerManager.set_mainPlayer(JsonConvert.DeserializeObject<playerInfo>(www.downloadHandler.text));
		}
		GetSessionID(www.GetResponseHeaders());//得到ＷＥＢserver的ＳＥＳＳＩＯＮ表頭黨
		PlayerManager.set_Board();
		Grid.getGrid.LoadFile();
		PlayerManager.SavePlayerInfo_Local(PlayerManager.get_main_playerInfo());
	}

	string playerdate_to_json(playerInfo playerInfo)
	{
		string str = JsonConvert.SerializeObject(playerInfo);
		return str;
	}



	void GetSessionID(Dictionary<string, string> resposeHeaders)
    {
        foreach (KeyValuePair<string, string> header in resposeHeaders)
        {
            Debug.Log(string.Format("{0}:{1}", header.Key, header.Value));
            if (header.Key == "Set-Cookie")
            {
                string[] cookies = header.Value.Split(';');
                for (int i = 0; i < cookies.Length; i++)
                {
                    if (cookies[i].Split('=')[0] == "PHPSESSID" && !this.headers.ContainsKey("COOKIE"))
                    {
                        this.sessionID = cookies[i];
                        this.headers.Add("COOKIE", this.sessionID);
                        Debug.Log(headers["COOKIE"]);
                        break;
                    }
                }
            }
        }
    }

	static int ConnectionStatus()
	{
		int nStatus;

        if (Application.internetReachability == NetworkReachability.NotReachable)
            nStatus = NotReachable;
        else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            nStatus = ReachableViaLocalAreaNetwork;
        else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
            nStatus = ReachableViaCarrierDataNetwork;
        else
            nStatus = -1;

        return nStatus;
	}

	IEnumerator PingConnect(string webadd)
    {
        //Ping網站
		Ping ping = new Ping(webadd);
  
        int nTime = 0;

        while (!ping.isDone)
        {
            yield return new WaitForSeconds(0.1f);

            if (nTime > 20) // time 2 sec, OverTime
            {
                nTime = 0;
                Debug.Log("連線失敗 : " + ping.time);
            }
            nTime++;
        }
        yield return ping.time;

        Debug.Log("連線成功");
    }

    void Get_db_logerror(int errorcode)
	{
		foreach( int m_errorcod in Enum.GetValues(typeof(errorcode_type)) )
		{
			if(errorcode == m_errorcod)
			{
				Debug.LogError(Enum.GetName(typeof(errorcode_type),m_errorcod));
				break;
			}
		}
	}
}

public enum errorcode_type
{
	Good = 0,
	unitydata_is_wrong = 1,
    no_name_or_passwd_or_highscore = 2,
    has_this_name = 3,
    db_data_is_not_right = 4,
    db_has_not_account = 5,
    passwd_not_right = 6,
    logout_fail = 10,
    top10_data_not_ready = 15
}
