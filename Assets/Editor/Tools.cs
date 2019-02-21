using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class Tools : UnityEditor.AssetModificationProcessor {

	static ShopManager mShopManager;
	static ShopManager m_ShopManager { get { return setShopMgr();} }

    

	public static void setShopFileNames(int FileType,string path)
	{
		string typeName = "";
		switch (FileType)
		{
			case 0:
				typeName = "*.json";
				break;
			case 1:
				typeName = "*.csv";
				break;
			case 2:
				typeName = "*.xml";
				break;
		}
     

		string[] Temp_fileNames = Directory.GetFiles(Application.dataPath + "/Resources/ExcelData", typeName);

        for (int i = 0; i < Temp_fileNames.Length; i++)
        {
            Temp_fileNames[i] = Path.GetFileNameWithoutExtension(Temp_fileNames[i]);
        }

        m_ShopManager.fileNames = Temp_fileNames;

        Debug.Log(path);
	}

	static ShopManager setShopMgr()
	{
		if(mShopManager == null)
		{
			mShopManager = Transform.FindObjectOfType<ShopManager>().GetComponent<ShopManager>();
		}
		return mShopManager;
	}
}
