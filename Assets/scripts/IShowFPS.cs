using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class IShowFPS : MonoBehaviour {
    
    #region
    private float m_LastUpdateShowTime = 0f;//上一次更新幀率的時間
    private float m_UpdateShowDeltaTime = 0.016f;//更新幀率的時間間隔
    private int m_FrameUpdate = 0;//幀數
    private float m_FPS = 0;
    public Text m_text_FPS;
	#endregion

    private void Awake()
    {
        Application.targetFrameRate = 60;//設定FPS
    }

    void Start()
	{
        m_LastUpdateShowTime = Time.realtimeSinceStartup;
        m_text_FPS = gameObject.GetComponent<Text>();
	}

	private void Update()
	{
        ShowFPS();
	}

    public virtual void ShowFPS()
    {
        m_FrameUpdate++;
        if (Time.realtimeSinceStartup - m_LastUpdateShowTime >= m_UpdateShowDeltaTime)
        {
            m_FPS = m_FrameUpdate / (Time.realtimeSinceStartup - m_LastUpdateShowTime);
            m_FrameUpdate = 0;
            m_LastUpdateShowTime = Time.realtimeSinceStartup;
        }
        m_text_FPS.text = "FPS:" + m_FPS;
    }
}
