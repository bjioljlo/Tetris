﻿using UnityEngine;

public class mainLava : IMainBehavier
{
    public GameObject Go_Man;
    public float LavaAddSpeed = Grid.getGrid.LavaAddSpeed;
    public float LavaStartSpeed = Grid.getGrid.LavaStartSpeed;
    public float LavaNowSpeed = Grid.getGrid.LavaNowSpeed;

    private void Awake()
    {
        Grid.getGrid.mainLava = this;
    }

    private void Start()
    {
        LavaAddSpeed = Grid.getGrid.LavaAddSpeed;
        LavaStartSpeed = Grid.getGrid.LavaStartSpeed;
        LavaNowSpeed = Grid.getGrid.LavaNowSpeed;
        Go_Man = GameObject.FindGameObjectWithTag("man");
    }
    public override void IPauseUpdate()
    {
        if (Grid.getGrid.IsStart)
        {
            if (Grid.getGrid.Score != 0)
                LavaNowSpeed = LavaStartSpeed + (((int)Grid.getGrid.Score) * LavaAddSpeed);
            transform.position += new Vector3(0, LavaNowSpeed * Time.deltaTime, 0);
        }

        if ((int)transform.position.y == (int)Go_Man.transform.position.y + 1)
        {
            Grid.getGrid.GameOver();
        }
    }

    public void setLavaSkin(Sprite m_sprite)
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer child in spriteRenderers)
        {
            child.sprite = m_sprite;
        }
    }
}
