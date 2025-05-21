using UnityEngine;
using UnityEngine.UI;


public class DebugManager : IManager
{
	public static DebugManager m_DebugManager = null;
	static mainLava m_mainLava;
	public static mainLava mainlava
	{
		get
		{
			if (m_mainLava == null)
			{
				m_mainLava = Grid.getGrid.mainLava;
			}
			return m_mainLava;
		}
	}
	static Man m_Man;
	public static Man Man
	{
		get
		{
			if (m_Man == null)
			{
				m_Man = Grid.getGrid.Man;
			}
			return m_Man;
		}
	}

	static int notificationSecond = 5;

	private void Awake()
	{
		if (m_DebugManager == null)
		{
			m_DebugManager = this;
			DontDestroyOnLoad(this);
		}
		else if (m_DebugManager != this)
		{
			Destroy(gameObject);
		}
	}

	public static void SetDebugPage()
	{
		GameObject obj_debug = GameObject.Find("DebugPage");
		if (!Grid.getGrid.DebugPageOn) obj_debug.SetActive(false);
		else InitDebugPage(obj_debug);
	}

	public static void InitDebugPage(GameObject vDebugPage)
	{
		SetInputFieldSymbol(vDebugPage, "Scroll View/Viewport/Content/DebugLine1/LavaAddSpeed",
							Grid.getGrid.LavaAddSpeed.ToString(), DebugLavaAddSpeed);
		SetInputFieldSymbol(vDebugPage, "Scroll View/Viewport/Content/DebugLine2/ManSpeed",
							Grid.getGrid.ManSpeed.ToString(), DebugManAddSpeed);
		SetToggleSymbol(vDebugPage, "Scroll View/Viewport/Content/DebugLine3/Debug_ShowFPS",
						FindObjectOfType<FPS_ShowFPS>().IsShowFps, DebugSetIsShowFps);
		SetInputFieldSymbol(vDebugPage, "Scroll View/Viewport/Content/DebugLine4/BoxSpeed",
							Grid.getGrid.BoxSpeed.ToString(), DebugBoxSpeedChange);
		SetToggleSymbol(vDebugPage, "Scroll View/Viewport/Content/DebugLine5/Debug_IsHasItemDown",
						FindObjectOfType<Spawner>().IsDownItem, DebugSetItemDown);
		SetToggleSymbol(vDebugPage, "Scroll View/Viewport/Content/DebugLine6/Debug_show",
						FindObjectOfType<Debug_log>().IsdebugAreaOn, DebugSetDebugLog);
		SetButtonSymbol(vDebugPage, "Scroll View/Viewport/Content/DebugLine8/btn_localNotification",
						DebugSetLocalNotification);
		SetToggleSymbol(vDebugPage, "Scroll View/Viewport/Content/DebugLine9/Debug_IsUseWebserver",
						PlayerManager.get_webserver().IsWebserverOn, DebugSetIsUseWebserver);
		SetButtonSymbol(vDebugPage, "Scroll View/Viewport/Content/DebugLine10/btn_AddCoin",
						DebugAddCoinBox);
		SetButtonSymbol(vDebugPage, "Scroll View/Viewport/Content/DebugLine11/btn_ResetPlayerInfo",
						DebugResetPlayerInfo);
	}

	static void SetInputFieldSymbol(GameObject @object, string path, string showtext, UnityEngine.Events.UnityAction<string> unityAction)
	{
		@object.transform.Find(path).GetComponent<InputField>().text = showtext;
		@object.transform.Find(path).GetComponent<InputField>().onEndEdit.AddListener(unityAction);
	}
	static void SetToggleSymbol(GameObject @object, string path, bool show, UnityEngine.Events.UnityAction<bool> unityAction)
	{
		@object.transform.Find(path).GetComponent<Toggle>().isOn = show;
		@object.transform.Find(path).GetComponent<Toggle>().onValueChanged.AddListener(unityAction);
	}
	static void SetDropdownSymbol(GameObject @object, string path, int InitOption, UnityEngine.Events.UnityAction<int> unityAction)
	{
		@object.transform.Find(path).GetComponent<Dropdown>().value = InitOption;
		@object.transform.Find(path).GetComponent<Dropdown>().onValueChanged.AddListener(unityAction);
	}
	static void SetButtonSymbol(GameObject @object, string path, UnityEngine.Events.UnityAction unityAction)
	{
		@object.transform.Find(path).GetComponent<Button>().onClick.AddListener(unityAction);
	}

	static void DebugManAddSpeed(string str)
	{
		try
		{
			float temp = float.Parse(str);
			if (temp > 0)
			{
				Grid.getGrid.ManSpeed = temp;
				Man.setManMoveable(new NormalManMove(Man.gameObject, null));
			}
		}
		catch
		{
			Debug.LogError("輸入錯誤 " + str);
		}
	}
	static void DebugLavaAddSpeed(string str)
	{
		try
		{
			float temp = float.Parse(str);
			if (temp > 0)
				mainlava.LavaAddSpeed = temp;
		}
		catch
		{
			Debug.LogError("輸入錯誤 " + str);
		}
	}
	static void DebugBoxSpeedChange(string str)
	{
		try
		{
			float temp = float.Parse(str);
			if (temp > 0)
				Grid.getGrid.BoxSpeed = temp;
		}
		catch
		{
			Debug.LogError("輸入錯誤 " + str);
		}
	}
	static void DebugSetIsShowFps(bool IsOn)
	{
		FindObjectOfType<FPS_ShowFPS>().IsShowFps = IsOn;
	}
	static void DebugSetItemDown(bool IsOn)
	{
		FindObjectOfType<Spawner>().IsDownItem = IsOn;
	}
	static void DebugSetIsUseWebserver(bool IsOn)
	{
		PlayerManager.setWebserver(IsOn);
	}
	static void DebugSetDebugLog(bool IsOn)
	{
		FindObjectOfType<Debug_log>().IsdebugAreaOn = IsOn;
	}
	static void DebugSetLocalNotification()
	{
		NotificationManager.SetLocalNotification("再度成功", "本地推播成功發送", System.DateTime.Now.AddSeconds(5));
	}
	static void DebugAddCoinBox()
	{
		Man.AddCoinBox(100);
	}
	static void DebugResetPlayerInfo()
	{
		PlayerManager.ResetPlayerInfo();
	}
}
