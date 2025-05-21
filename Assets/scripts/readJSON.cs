using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;

public class readJSON : MonoBehaviour {
	public class PlayerData
    {
		public int ID;
        public int Age;
        public string Name;
		public string Address;
		public string PhoneNum;
		public string Gender;
		public string BirthDay;
		public int HighScore;
		public string Password;
    }
	public class TopRankInfo
	{
		public int Rank;
		public int ID;
		public string Name;
		public int HighScore;
	}

	public string WebserverLocation = "http://218.161.4.121:81/test/";
	public string Download = "DownloadDB.php";
	public string update = "UpdateDB.php";
	public string AddNew = "AddNew.php";
	public string HighScore = "HighScore.php";
	public string Top10 = "TopRank.php";
	public string login = "login.php";
	public string logout = "logout.php"; 

	public InputField In_addname;
	public Text text_ShowName;
	public Text text_Age;
	public Text text_Add;
	public Text text_PhNum;
	public Text text_Gender;
	public Text text_Birthday;
	public Text text_LoginStatInfo;
	public Text text_Messege;

	public InputField In_Age;
	public InputField In_Add;
	public InputField In_PhNum;
	public Dropdown In_Gender;
	public InputField In_Birthday;
	public InputField In_highScore;
	public InputField In_Passwd;

	public UnityWebRequest m_webRequest;

	public string sessionID;
	private Dictionary<string, string> headers = new Dictionary<string, string>();

	public int num;
	public string Username;
	public bool isOpen;
	public string Addusername;
	public int AdduserNum;
	public string Messege;
	public List<PlayerData> AllPlayerData = new List<PlayerData>();
	public List<TopRankInfo> TopList = new List<TopRankInfo>();
	public PlayerData MyPlayerData = null;
    
	public float UpdatePlayerTimer = 5f;
	public float LastPlayerUpdate = 0;

	private void Awake()
	{
		Download = WebserverLocation + Download;
		update = WebserverLocation + update;
		AddNew = WebserverLocation + AddNew;
		HighScore = WebserverLocation + HighScore;
		Top10 = WebserverLocation + Top10;
		login = WebserverLocation + login;
		logout = WebserverLocation + logout;
		getUIObj();
	}

	private void Start()
	{      
		m_webRequest = new UnityWebRequest();
		StartCoroutine(downloadjson());
		LastPlayerUpdate = Time.time;

	}

    void getUIObj()
	{
		In_addname = GameObject.Find("In_addname").GetComponent<InputField>();
		In_Add = GameObject.Find("In_Add").GetComponent<InputField>();
		In_Age = GameObject.Find("In_Age").GetComponent<InputField>();
		In_PhNum = GameObject.Find("In_PhNum").GetComponent<InputField>();
		In_Passwd = GameObject.Find("In_Passwd").GetComponent<InputField>();
		In_Birthday = GameObject.Find("In_Birthday").GetComponent<InputField>();
		In_highScore = GameObject.Find("In_highScore").GetComponent<InputField>();
		In_Gender = GameObject.Find("In_Gender").GetComponent<Dropdown>();
		text_Add = GameObject.Find("Text_Add").GetComponent<Text>();
		text_Age = GameObject.Find("Text_Age").GetComponent<Text>();
		text_PhNum = GameObject.Find("Text_PhNum").GetComponent<Text>();
		text_Gender = GameObject.Find("Text_Gender").GetComponent<Text>();
		text_Messege = GameObject.Find("Text_Messege").GetComponent<Text>();
		text_ShowName = GameObject.Find("Text_ShowName").GetComponent<Text>();
		text_Birthday = GameObject.Find("Text_Birthday").GetComponent<Text>();
		text_LoginStatInfo = GameObject.Find("Text_LoginStatInfo").GetComponent<Text>();
	}
    


	void Update()
	{

		if(MyPlayerData!=null)
		{
			text_ShowName.text = "ID:"+MyPlayerData.Name;
			text_Age.text = "Age:" +MyPlayerData.Age.ToString();
			text_Add.text = "Address:" +MyPlayerData.Address;
			text_PhNum.text ="Phone:" + MyPlayerData.PhoneNum;
			text_Gender.text = "Gender:" +MyPlayerData.Gender;
			text_Birthday.text = "Birthday:" +MyPlayerData.BirthDay;
			text_LoginStatInfo.text = "已經登入";
	
		}
        else
		{
			text_ShowName.text = "ID:";
            text_Age.text = "Age:";
            text_Add.text = "Address:";
            text_PhNum.text = "Phone:";
            text_Gender.text = "Gender:";
            text_Birthday.text = "Birthday:";
			text_LoginStatInfo.text = "尚未登入";
		}

		text_Messege.text = Messege;
	}

	private void OnGUI()
	{		
		if(MyPlayerData != null)
		{
			Addusername = MyPlayerData.Name;
			AdduserNum = MyPlayerData.Age;
		}


		if(GUILayout.Button("LogIn"))
		{
			if(In_addname.text != "" && In_Passwd.text != "")
			{
				StartCoroutine(loginSession());
			}
			else
			{
				Debug.LogWarning("帳號或密碼有誤！");
			}

		}
		if(GUILayout.Button("LogOut"))
		{
			StartCoroutine(logoutsession());
		}
		if (GUILayout.Button("update"))
		{

			if(MyPlayerData != null)
			{
				MyPlayerData.Name = In_addname.text;
                MyPlayerData.Gender = getGender(In_Gender.value);
                if (In_Age.text != "")
                    MyPlayerData.Age = Int32.Parse(In_Age.text);
                if (In_Add.text != "")
                    MyPlayerData.Address = In_Add.text;
                if (In_PhNum.text != "")
                    MyPlayerData.PhoneNum = In_PhNum.text;
                if (In_Birthday.text != "")
                    MyPlayerData.BirthDay = In_Birthday.text;
			}
			else
            {
				Messege = "你必須先登入才可以使用此功能";
                return;
            }
			StartCoroutine(updatejson());


        }
		if (GUILayout.Button("CreatePlayer"))
        {
			if(MyPlayerData != null)
			{
				Debug.LogWarning("你已經登入帳號");
				return;
			}
			MyPlayerData = new PlayerData();
			if (In_addname.text != "")
			{
				MyPlayerData.Name = In_addname.text;
			}
			else
			{
				Messege = "姓名未填";
				MyPlayerData = null;
				return;
			}

            MyPlayerData.Gender = getGender(In_Gender.value);
            
            if (In_Age.text != "")
			{
                MyPlayerData.Age = Int32.Parse(In_Age.text);
			}
            else
            {
                Messege = "年齡未填";
				MyPlayerData = null;
                return;
            }

            if (In_Add.text != "")
			{
                MyPlayerData.Address = In_Add.text;
			}
			else
            {
                Messege = "地址未填";
				MyPlayerData = null;
                return;
            }

            if (In_PhNum.text != "")
			{
                MyPlayerData.PhoneNum = In_PhNum.text;
			}
			else
            {
                Messege = "電話未填";
				MyPlayerData = null;
                return;
            }

            if (In_Birthday.text != "")
			{
                MyPlayerData.BirthDay = In_Birthday.text;
			}
			else
            {
                Messege = "生日未填";
				MyPlayerData = null;
                return;
            }

			if (In_Passwd.text != "")
            {
				MyPlayerData.Password = In_Passwd.text;
            }
            else
            {
                Messege = "密碼未填";
                MyPlayerData = null;
                return;
            }
            
			StartCoroutine(AddNewjson());
			//StartCoroutine(loginSession());
			//StartCoroutine(downloadjson());
        }
		if (GUILayout.Button("HighScore"))
        {
			if(MyPlayerData != null)
			{
				MyPlayerData.HighScore = Int32.Parse(In_highScore.text);
				StartCoroutine(updateScorejson());
			}
        }
		if(GUILayout.Button("GetTop 10 List"))
		{
			StartCoroutine(Top10json());
		}
	}




	public string jsonSave(PlayerData data)
	{
		string json = JsonConvert.SerializeObject(data);
		return json;
	}

	public string jsonSaveString(string data)
	{
		string json = JsonConvert.SerializeObject(data);
		return json;
	}



	public IEnumerator logoutsession()
	{
		var www = UnityWebRequest.Get(logout);
		yield return www.SendWebRequest();
		if(www.downloadHandler.text != "")
		{
			Debug.LogError(www.downloadHandler.text);
			yield break;
		}
		MyPlayerData = null;
		clearAll_inputfile();
	}

	public IEnumerator downloadjson()
	{

		var www = UnityWebRequest.Get(Download);
		if (headers.ContainsKey("COOKIE"))
        {
			www.SetRequestHeader("COOKIE", headers["COOKIE"]);
        }
		yield return www.SendWebRequest();

		if (www.error != null)
        {
			Debug.LogError(www.error);
            yield return null;
        }

		Debug.LogError(www.downloadHandler.text);

		string[] data22 = JsonConvert.DeserializeObject<string[]>(www.downloadHandler.text);

		AllPlayerData = new List<PlayerData>();

		for (int i = 0; i < data22.Length;i++)
		{
			PlayerData data = JsonConvert.DeserializeObject<PlayerData>(data22[i]);
			AllPlayerData.Add(data);
			Debug.Log("PlayerID:" + AllPlayerData[i].ID.ToString() + "   PlayerName:" + AllPlayerData[i].Name);
		}  
        
		if(MyPlayerData != null)
		{
			StartCoroutine(loginSession());
		}
	}

	public IEnumerator loginSession()
	{
		WWWForm form = new WWWForm();
		form.AddField("loginName", jsonSaveString(In_addname.text));
		form.AddField("loginPasswd", jsonSaveString(In_Passwd.text));
		var www = UnityWebRequest.Post(login, form);
		yield return www.SendWebRequest();
		if(www.result == UnityWebRequest.Result.ConnectionError)
		{
			Debug.LogError(www.error);
			yield return null;
		}

		//接收ＬＯＧＩＮ回傳的資料
        
		string[] data22 = JsonConvert.DeserializeObject<string[]>(www.downloadHandler.text);
		if(data22 == null)
		{
			Debug.LogWarning("帳號密碼錯誤！！");
			yield break;
		}
		for (int i = 0; i < data22.Length;i++)
        {
            PlayerData data = JsonConvert.DeserializeObject<PlayerData>(data22[i]);
			MyPlayerData = data;
			Debug.Log("PlayerID:" + data.ID.ToString() + "   PlayerName:" + data.Name);
        }  


		GetSessionID(www.GetResponseHeaders());//得到ＷＥＢ的ＳＥＳＳＩＯＮ表頭黨
	}

	private void GetSessionID(Dictionary<string,string> resposeHeaders)
	{
		foreach(KeyValuePair<string,string> header in resposeHeaders)
		{
			Debug.Log(string.Format("{0}:{1}", header.Key, header.Value));
			if(header.Key == "Set-Cookie")
			{
				string[] cookies = header.Value.Split(';');
				for (int i = 0; i < cookies.Length;i++)
				{
					if(cookies[i].Split('=')[0] == "PHPSESSID" && !this.headers.ContainsKey("COOKIE"))
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

	public IEnumerator Top10json()
    {
		var www = UnityWebRequest.Get(Top10);
        yield return www.SendWebRequest();

		if (!WebResposeCod(www))
			yield return null;

		//if (www.isNetworkError)
		if (www.result == UnityWebRequest.Result.ConnectionError)
        {
			Debug.LogError("UnityWebReqError:" + www.error);
            yield return null;
        }


		if(www.downloadHandler.text == "[[]]" || 
		www.downloadHandler.text == "Error in selectingTable 'Test.TopRank' doesn't exist")
		{
			Debug.LogWarning("DtatBase is 忙碌");
			yield break;
		}



        string[] data22 = JsonConvert.DeserializeObject<string[]>(www.downloadHandler.text);

		TopList = new List<TopRankInfo>();
		string show = "";
        for (int i = 0; i < data22.Length; i++)
        {
			TopRankInfo temp = JsonConvert.DeserializeObject<TopRankInfo>(data22[i]);
			TopList.Add(temp);
			show = show + "Name:" + temp.Name + " " + "Score:" + temp.HighScore + "\n";
        }
		Messege = show;
    }

	public IEnumerator updatejson()
	{
		if(MyPlayerData == null)
		{
			Debug.LogError("No player data to update!!");
			yield return null;
		}
		WWWForm form = new WWWForm();
		form.AddField("unitydata", jsonSave(MyPlayerData));
		var www = UnityWebRequest.Post(update, form);
		if (headers.ContainsKey("COOKIE"))
        {
            www.SetRequestHeader("COOKIE", headers["COOKIE"]);
        }
		yield return www.SendWebRequest();
		if (www.error != null)
        {
			Debug.LogError(www.result);
            yield return null;
        }

	}

	public IEnumerator updateScorejson()
    {
        if (MyPlayerData == null)
        {
            Debug.LogError("No player data to update!!");
            yield return null;
        }
        WWWForm form = new WWWForm();
        form.AddField("unitydata", jsonSave(MyPlayerData));
		var www = UnityWebRequest.Post(HighScore, form);
        yield return www.SendWebRequest();
        if (www.error != null)
        {
            Debug.LogError(www.result);
            yield return null;
        }

    }

	public IEnumerator AddNewjson()
    {
        if (MyPlayerData == null)
        {
			Debug.LogError("No player data to Add!!");
            yield return null;
        }
        WWWForm form = new WWWForm();
        form.AddField("unitydata", jsonSave(MyPlayerData));
		var www = UnityWebRequest.Post(AddNew, form);
		yield return www.SendWebRequest(); 
		if (www.result == UnityWebRequest.Result.ConnectionError)
        {
			Debug.LogError(www.error);
            yield return null;
        }

		if(www.downloadHandler.text != "")
		{
			Debug.LogWarning(www.downloadHandler.text);
			MyPlayerData = null;
			yield break;
		}

		StartCoroutine(loginSession());
        StartCoroutine(downloadjson());
    }

    void clearAll_inputfile()
	{
		In_addname.text = "";
		In_Add.text = "";
		In_Age.text = "";
		In_PhNum.text = "";
		In_Passwd.text = "";
		In_Birthday.text = "";
		In_highScore.text = "";
	}


	public string getGender(int num)
	{
		if(num == 0)
		{
			return "male";
		}
		else
		{
			return "female";
		}
	}

	public bool WebResposeCod(UnityWebRequest unityWebRequest)
	{
		if (unityWebRequest.responseCode == 200)
			return true;
		if(unityWebRequest.responseCode == -1)
		{
			Debug.LogError("UnityWebReqest is not work!!");
			return false;
		}
		Debug.LogError("Web error: " + unityWebRequest.responseCode);
		return false;
	}

}
