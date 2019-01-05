using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debug_log : MonoBehaviour {

	public Color32 errorColor = new Color32();
	public Color32 warringColor = new Color32();
	public Color32 logColor = new Color32();

	string myLog;
	//int LogLines = 0;
	Queue myLogQueue = new Queue();
	Toggle IsShowLogToggle;
	GameObject sv_debug_Panel;//scrollview 的元件
	RectTransform debug_context;//scrollview 的context
	Text debug_text;////scrollview 的text
	int debug_LineNumber = 0;//debug的行數
	int debug_TempLines = 999;
	Vector3 localpos = new Vector3();

	// Use this for initialization
	void Start () {
		sv_debug_Panel = GameObject.Find("debug_ScrollView");
		IsShowLogToggle = GameObject.Find("Debug_show").GetComponent<Toggle>();
		debug_context = GameObject.Find("debug_Content").GetComponent<RectTransform>();
		debug_text = GameObject.Find("Debug_Area").GetComponent<Text>();
		localpos = sv_debug_Panel.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		if (!GetToggle().isOn)
        {
            sv_debug_Panel.transform.localPosition = new Vector3(1000, localpos.y);
        }
        else
        {
            sv_debug_Panel.transform.localPosition = localpos;

        }
		setArea(myLog);
	}

	private void OnEnable()
	{
		Application.logMessageReceived += HandleLog;
	}

	private void OnDisable()
	{
		Application.logMessageReceived -= HandleLog;
	}

	void HandleLog(string logString,string stackTrace,LogType type)
	{
		myLog = logString;
		string newString = "";

		if (myLogQueue.Count > 0)
			newString += "\n";

		Color32 temp_color;

		switch(type)
		{
			case LogType.Log:
				temp_color = logColor;
				break;
			case LogType.Warning:
				temp_color = warringColor;
				break;
			default:
				temp_color = errorColor;
				break;
		}

		string r, g, b, a;
		r = string.Format("{0:x2}", temp_color.r);
		g = string.Format("{0:x2}", temp_color.g);
		b = string.Format("{0:x2}", temp_color.b);
		a = string.Format("{0:x2}", temp_color.a);

		newString += "<color=#" +r+g+b+a + ">";

		//if (myLogQueue.Count <= 0)
			newString += "[" + type + "] : " + myLog;
		//else
			//newString += "\n [" + type + "] : " + myLog;

		newString += "</color>";

		myLogQueue.Enqueue(newString);
		if(type == LogType.Exception)
		{
			newString = "\n" + stackTrace;
			myLogQueue.Enqueue(newString);
		}

		debug_LineNumber = myLogQueue.Count;

		myLog = string.Empty;
		foreach (string mylog in myLogQueue)
			myLog += mylog;
	}

	void setArea(string strDebug)
	{
		if (debug_TempLines == debug_LineNumber)
			return;
		debug_context.sizeDelta = new Vector2(debug_context.sizeDelta.x, 28 * debug_LineNumber);
		debug_text.GetComponent<RectTransform>().sizeDelta = debug_context.sizeDelta;
		if(debug_LineNumber>23)
		    debug_context.localPosition = new Vector3(debug_context.localPosition.x , 28 * (debug_LineNumber - 23) , debug_context.localPosition.z);
		debug_text.text = strDebug;
		debug_TempLines = debug_LineNumber;
	}

	Toggle GetToggle()
    {
		if (IsShowLogToggle == null)
        {
			IsShowLogToggle = GameObject.Find("Debug_show").GetComponent<Toggle>();
        }

		return IsShowLogToggle;
    }
}
