using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IManager : MonoBehaviour
{
	public static IManager ins;
	void Awake()
	{
		if (ins == null)
		{
			ins = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (ins != this)
		{
			Destroy(gameObject);
		}
		awake_function();
	}

	public virtual void awake_function() { }
}
