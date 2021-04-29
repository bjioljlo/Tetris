using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Notifications.Android;

public class NotificationManager : IManager
{
    public static NotificationManager m_NotificationManager = null;
    static AndroidNotificationChannel m_channel;
    static string ChannelID = "channel_01";
    static string ChannelName = "LocalNotification";
    static Importance ChannelImportance = Importance.High;
    static string ChannelDescription = "Generic notifications";
    void Awake() {
        if(m_NotificationManager == null){
            m_NotificationManager = this;
            DontDestroyOnLoad(this);
        }
        else if(m_NotificationManager != this){
            Destroy(gameObject);
        }
    }
    void Start() {
        InitialNotificationChannel();
    }
    static void InitialNotificationChannel(){
        m_channel = new AndroidNotificationChannel(){
            Id = ChannelID,
    		Name = ChannelName,
    		Importance = ChannelImportance,
    		Description = ChannelDescription,
        };
        AndroidNotificationCenter.RegisterNotificationChannel(m_channel);
    }

    public static void SetLocalNotification(string title,string text,DateTime fireTime,string channelID = null){
        var notification = new AndroidNotification();
		notification.Title = title;
		notification.Text = text;
		notification.FireTime = fireTime;
        if(channelID == null) channelID = ChannelID;
		AndroidNotificationCenter.SendNotification(notification,channelID);
    }
}
