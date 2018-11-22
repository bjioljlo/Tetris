using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class IButton : IUI
{
    Button m_button;
    AudioClip m_ClickSound;
    public string m_SoundName = "Click.mp3";

    public virtual void ClickAction() { }

    public override void startInit()
    {
        m_button = GetComponent<Button>();
        m_ClickSound = readSound();

        m_button.onClick.AddListener(PlayClickSound);//載入播放監聽
        m_button.onClick.AddListener(ClickAction);//載入按鈕點擊事件監聽
        base.startInit();
    }

    public virtual AudioClip readSound()//讀取音效檔案
    {
        Debug.Log(name + " get soundName: " + m_SoundName);
        return Resources.Load<AudioClip>(m_SoundName);
    }

    public void PlayClickSound()
    {
        if (m_ClickSound == null)
            Debug.LogError("clicksound is null");
        SoundManager.m_Effect.PlayOneShot(m_ClickSound);
    }


}

