using System.Collections.Generic;
using UnityEngine;

public class SoundManager : IManager
{

	//使用單例模式
	private static SoundManager m_SoundManager = null;
	private SoundManager() { }
	public static SoundManager getSoundManager
	{
		get
		{
			if (m_SoundManager == null)
			{
				m_SoundManager = new SoundManager();
			}
			return m_SoundManager;
		}
	}


	public static float BGM_Value = 0;
	public static float Effect_Value = 0;
	public static AudioSource m_BGM, m_Effect, m_BGM2;
	public static Dictionary<string, AudioClip> Dicry_ClickSounds = new Dictionary<string, AudioClip>();

	private void Awake()
	{
		if (m_SoundManager == null)
		{
			m_SoundManager = this;
			DontDestroyOnLoad(this);
		}
		else if (this != m_SoundManager)
		{
			Destroy(gameObject);
		}
		m_BGM = GameObject.Find("SoundBGM").GetComponent<AudioSource>();
		m_BGM2 = GameObject.Find("SoundBackground").GetComponent<AudioSource>();
		m_Effect = GameObject.Find("SoundEffect").GetComponent<AudioSource>();
	}
	private void Update()
	{
		if (!Grid.getGrid.IsPause && !m_BGM.isPlaying && Grid.getGrid.IsStart)
		{
			PlayBGM();
		}
	}

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

	public static AudioClip getClickSound(string objName, string SoundName)
	{
		if (Dicry_ClickSounds.ContainsKey(SoundName))
		{
			Debug.Log(objName + " get soundName: " + SoundName);
		}
		else
		{
			Debug.Log(objName + " Load soundName: " + SoundName);
			Dicry_ClickSounds.Add(SoundName, Resources.Load<AudioClip>(SoundName));
		}
		return Dicry_ClickSounds[SoundName];
	}
}
