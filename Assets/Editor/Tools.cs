using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class Tools : UnityEditor.AssetModificationProcessor {

	static ShopManager m_ShopManager;
    
	public static AssetMoveResult OnWillMoveAsset(string oldPath, string newPath)
    {
        AssetMoveResult result = AssetMoveResult.DidNotMove;
		//Debug.Log("OnWillMoveAsset  from: " + oldPath + " to: " + newPath);
		setShopFileNames(newPath);
        return result;
    }

	static void OnWillCreateAsset(string path)
	{
		setShopFileNames(path);
	}

	static void setShopFileNames(string path)
	{
		setShopMgr();

        if (m_ShopManager == null) Debug.LogError("null");

        string[] Temp_fileNames = Directory.GetFiles(Application.dataPath + "/Resources/ExcelData", "*.csv");

        for (int i = 0; i < Temp_fileNames.Length; i++)
        {
            Temp_fileNames[i] = Path.GetFileNameWithoutExtension(Temp_fileNames[i]);
        }

        m_ShopManager.fileNames = Temp_fileNames;

        Debug.Log(path);
	}

	static void setShopMgr()
	{
		m_ShopManager = Transform.FindObjectOfType<ShopManager>().GetComponent<ShopManager>();
	}
}
