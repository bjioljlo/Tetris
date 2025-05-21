using UnityEngine;
using System.IO;

public class Tools : UnityEditor.AssetModificationProcessor
{

	static ShopManager mShopManager;

    [System.Obsolete]
    static ShopManager m_ShopManager { get { return setShopMgr(); } }

    [System.Obsolete]
    public static void setShopFileNames(int FileType, string path)
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

	[System.Obsolete]
	static ShopManager setShopMgr()
	{
		if (mShopManager == null)
		{
			mShopManager = Object.FindObjectOfType<ShopManager>().GetComponent<ShopManager>();
		}
		return mShopManager;
	}
}
