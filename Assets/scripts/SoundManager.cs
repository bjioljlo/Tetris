using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : IManager {

    public static float BGM_Value = 0;
    public static float Effect_Value = 0;
    public static AudioSource m_BGM,m_Effect,m_BGM2;

	public static void PlayBGM()
	{
		m_BGM.Play();
		m_BGM2.Play();
	}

	public static void StopBGM()
	{
		m_BGM.Stop();
		m_BGM2.Stop();
	}

    public static void changeBGM_value(float value)
    {
        BGM_Value = value;
        m_BGM.volume = BGM_Value;
		m_BGM2.volume = BGM_Value;
    }
    public static void changeEffect_value(float value)
    {
        Effect_Value = value;
        m_Effect.volume = Effect_Value;
    }
    
}
