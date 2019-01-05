using UnityEngine;
using System.Collections;
using System;

public class Mgr_LocalNotification : IManager
{
	public static Mgr_LocalNotification ins;

	public string RepeatTitle = "";
	public string OnceTitle = "";
	public string RepeatContext = "";
	public string OnceContext = "";
	public Color32 OnceColor = new Color32();
	public Color32 RepeatColor = new Color32();

	public class TimeUnit
	{
		public static int
		Second = 1000,
		Minute = Second * 60,
		Hour = Minute * 60,
		Day = Hour * 24,
		Week = Day * 7,
		Month = Week * 4,
		Year = Month * 12;
	}


	void Awake()
	{
		if(ins == null)
		{
			ins = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (ins != this)
		{
			Destroy(gameObject);
		}
		ClearAll();
	}

	void OnApplicationFocus(bool focus)
	{
		if (focus)
		{
			ClearAll();
		}
		else
		{
			setEveryDayLocalNotification();
		}
	}

	//void OnApplicationQuit()
	//{
	//	setEveryDayLocalNotification();
	//}

	public void setOpenBonusLocalNotification(int sec)
	{
		DateTime dateTimeNow = new DateTime();
		dateTimeNow = DateTime.Now;
		DateTime dateTimeFire = dateTimeNow.AddSeconds(sec);
		TimeSpan timeSpan = dateTimeFire.Subtract(dateTimeNow);
		OneTime(timeSpan, OnceTitle, OnceContext, OnceColor);
		Debug.Log("setOpenBonusLocalNotification");
	}

	void setEveryDayLocalNotification()
	{
		DateTime dateTimeNow = new DateTime();
		dateTimeNow = DateTime.Now;
		DateTime dateTimeNext;
		if (dateTimeNow.Hour >= 9)//設定時間超過九點會從明天開始
		{
			dateTimeNext = new DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day + 1, 9, 0, 0);
		}
		else
		{
			dateTimeNext = new DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day, 9, 0, 0);
		}
        
		TimeSpan timeSpan = dateTimeNext.Subtract(dateTimeNow);
		TimeSpan oneDay = dateTimeNext.AddDays(1).Subtract(dateTimeNext);
		Repeating(timeSpan,oneDay, 
		          RepeatTitle, RepeatContext, RepeatColor);
		Debug.Log("setEveryDayLocalNotification");
	}

	long Date_to_miniSec(DateTime dateTime)
	{
		long temp = dateTime.Second * TimeUnit.Second;
		temp += dateTime.Minute * TimeUnit.Minute;
		temp += dateTime.Hour * TimeUnit.Hour;
		temp += dateTime.Day * TimeUnit.Day;
		temp += dateTime.Month * TimeUnit.Month;
		temp += dateTime.Year * TimeUnit.Year;
		return temp;
	}

	public void OneTime(long time, string title, string context, Color32 bgcolor)
	{
		LocalNotification.SendNotification(1, time, title, context, bgcolor);
	}
	public void OneTime(TimeSpan time, string title, string context, Color32 bgcolor)
	{
		LocalNotification.SendNotification(time, title, context, bgcolor);
	}

	public void OneTimeBigIcon(long time, string title, string context, Color32 bgcolor)
	{
		LocalNotification.SendNotification(3, time, title, context, bgcolor, true, true, true, "notify_icon_big");
	}

	public void OneTimeWithActions()
	{
		LocalNotification.Action action1 = new LocalNotification.Action("background", "In Background", this);
		action1.Foreground = false;
		LocalNotification.Action action2 = new LocalNotification.Action("foreground", "In Foreground", this);
		LocalNotification.SendNotification(1, 5000, "Title", "Long message text with actions", new Color32(0xff, 0x44, 0x44, 255), true, true, true, null, "boing", "default", action1, action2);
	}

	public void Repeating(long Delaytime, long timeout, string title, string context, Color32 bgcolor)
	{
		LocalNotification.SendRepeatingNotification(2, Delaytime, timeout, title, context, bgcolor);
	}
	public void Repeating(TimeSpan time, TimeSpan timeout, string title, string context, Color32 bgcolor)
	{
		LocalNotification.SendRepeatingNotification(time, timeout, title, context, bgcolor);
	}

	public void Stop(int id)
	{
		LocalNotification.CancelNotification(id);
	}

	public void ClearAll()
	{
		LocalNotification.ClearNotifications();
	}

	public void OnAction(string identifier)
	{
		Debug.Log("Got action " + identifier);
	}
}
