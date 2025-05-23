﻿using UnityEngine;
using UnityEngine.UI;

public abstract class ISetting : IUI
{
    public Scrollbar scrollbar_BGM;
    public Scrollbar scrollbar_Effect;
    public Dropdown dropdown_BonusAdsSupplier;

    public override void startInit()
    {
        scrollbar_BGM = GameObject.Find("Scrollbar_BGM").GetComponent<Scrollbar>();
        scrollbar_Effect = GameObject.Find("Scrollbar_Effect").GetComponent<Scrollbar>();
        scrollbar_BGM.onValueChanged.AddListener(SoundManager.changeBGM_value);
        scrollbar_Effect.onValueChanged.AddListener(SoundManager.changeEffect_value);
        SoundManager.changeBGM_value(scrollbar_BGM.value);
        SoundManager.changeEffect_value(scrollbar_Effect.value);

        base.startInit();
    }
}
