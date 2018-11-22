using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IUI : MonoBehaviour
{
    public RectTransform Transform_my;
    public RectTransform Transform_OutPos;
    public RectTransform Transform_InPos;
    public Vector2 InPos;
    public Vector2 OutPos;

    private void Start()
    {
        startInit();
    }

    public virtual void startInit()
    {
        Transform_my = GetComponent<RectTransform>();

        if(Transform_InPos)
        InPos = Transform_InPos.anchoredPosition;

        if(Transform_OutPos)
        OutPos = Transform_OutPos.anchoredPosition;
    }
    public void moveUpdate(Vector2 pos)
    {
        Transform_my.anchoredPosition = pos;
    }

    public virtual void moveOut()
    {
        iTween.ValueTo(gameObject, iTween.Hash("easetype", iTween.EaseType.easeInOutExpo,
                                              "from", InPos,
                                              "to", OutPos,
                                              "onupdate", "moveUpdate",
                                           "time", 0.5f));
    }

    public virtual void moveIn()
    {
        iTween.ValueTo(gameObject, iTween.Hash("easetype", iTween.EaseType.easeInOutExpo,
                                               "from", OutPos,
                                               "to", InPos,
                                              "onupdate", "moveUpdate",
                                           "time", 0.5f));
    }
}


public abstract class IAccount:IUI
{
	
}