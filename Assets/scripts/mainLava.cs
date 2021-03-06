﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class mainLava : IMainBehavier {
    public GameObject Go_Man;
    public float LavaAddSpeed = 0.01f;
    public float LavaStartSpeed = 0.05f;
    public float LavaNowSpeed = 0.01f;
    public InputField DebugText;

    private void Start()
    {
        Go_Man = GameObject.FindGameObjectWithTag("man");

        DebugText = GameObject.Find("LavaAddSpeed").GetComponent<InputField>();
        DebugText.onValueChanged.AddListener(DebugAddSpeed);
        DebugText.text = LavaAddSpeed.ToString();
    }
    public override void IPauseUpdate()
    {
        if (Grid.getGrid.IsStart)
        {
            if (Grid.getGrid.Score != 0)
                LavaNowSpeed = LavaStartSpeed + (((int)Grid.getGrid.Score) * LavaAddSpeed);
            transform.position += new Vector3(0, LavaNowSpeed * Time.deltaTime, 0);
        }

        if((int)transform.position.y == (int)Go_Man.transform.position.y + 1)
        {
            Grid.getGrid.GameOver();
        }
    }

	public void setLavaSkin(Sprite m_sprite)
	{
		SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
		foreach(SpriteRenderer child in spriteRenderers)
		{
			child.sprite = m_sprite;
		}
	}

    public void DebugAddSpeed(string str)
    {
        try
        {
            float temp = float.Parse(str);
            if (temp > 0)
                LavaAddSpeed = temp;
        }
        catch
        {
            Debug.LogError("輸入錯誤 " + str);
        }
    }

}
