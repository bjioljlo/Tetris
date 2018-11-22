using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class IMainBehavier : MonoBehaviour
{
    
    virtual public void IGameUpdate() { }
    virtual public void IPauseUpdate() { }


	private void Update()
	{
        if (Grid.getGrid.IsStart)
            IGameUpdate();
        if (!Grid.getGrid.IsPause)
            IPauseUpdate();
            
	}

}
