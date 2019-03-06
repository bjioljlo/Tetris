using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
[System.Serializable]
public class ShopItemDataList : SerializableDictionary.Storage<List<ShopItemData>> { }
[System.Serializable]
public class ShopMenu : SerializableDictionary<ShopItemKind, List<ShopItemData>, ShopItemDataList> { }
[System.Serializable]
public class ShopItemData
{
	public bool IsOpen;
	public string ShopNumber, ShopName, ShopKind, ShopID;
	public int ShopPrice;
	public Sprite ShopImage;
	public GameObject ShopObj;
	public void setShopImage(string name)
	{
		ShopImage = Resources.Load<Sprite>(name);
		if (ShopImage == null)
		{
			ShopImage = Resources.Load<Sprite>("start");
		}
	}
	public void setShopObj(string name)
	{
		ShopObj = Resources.Load<GameObject>("prefabs/" + name);
		if (ShopObj == null)
		{
			ShopObj = Resources.Load<GameObject>("prefabs/Shop_boxcolor");

		}
	}
	void setShopPrice(GameObject vShopObj)
	{
		GameObject obj_price = vShopObj.transform.Find("Button/Text").gameObject;
		obj_price.GetComponent<Text>().text = ShopPrice.ToString();
	}
}
[System.Serializable]
public enum ShopItemKind
{
	None,
	Man,
	Box,
	Lava
}

public class ShopManager : IManager
{

	[SerializeField]
	ShopMenu m_ShopItemMenu;
	IDictionary<ShopItemKind, List<ShopItemData>> ShopMenu
	{
		get { return m_ShopItemMenu; }
		set { m_ShopItemMenu.CopyFrom(value); }
	}

	static ShopMenu m_static_ShopItemMenu = null;
	static Dictionary<ShopItemKind, List<GameObject>> m_ShopItemMenu_ShowOn = new Dictionary<ShopItemKind, List<GameObject>>();
	public string[] fileNames;

	public override void awake_function()
	{

		m_static_ShopItemMenu = m_ShopItemMenu;

		base.awake_function();
	}

	private void Start()
	{
        foreach (string child in fileNames)
        {
            if (m_ShopItemMenu == null) m_ShopItemMenu = new ShopMenu();
			m_ShopItemMenu.Add(GetShopItemKind(child), 
			                   CSV2ListReader(child));
			
        }

	}

	private void Update()
	{
		if(m_static_ShopItemMenu.Count == 0 && m_ShopItemMenu.Count != 0)
		    m_static_ShopItemMenu = m_ShopItemMenu;
	}

	public static ShopMenu getShopMenuDictionary()
	{
		
		return m_static_ShopItemMenu;
	}

	static List<ShopItemData> GetShopItemDatas(ShopItemKind kind)
	{
		foreach (KeyValuePair<ShopItemKind,List<ShopItemData>> child in getShopMenuDictionary())
		{
			if (child.Key == kind) return child.Value;
		}

		Debug.Log("No [" + kind.ToString() + "] shopList!!");
		return null;
	}

	public static List<GameObject> GetShopItemGameObj(ShopItemKind kind)
	{
		
		List<ShopItemData> List_Temp = GetShopItemDatas(kind);
		if (List_Temp == null) return null;

		List<GameObject> List_ObjTemp = new List<GameObject>();

		foreach(ShopItemData child in List_Temp)
		{
			if (child.IsOpen)
			{
				List_ObjTemp.Add(Instantiate(child.ShopObj));
				List_ObjTemp[List_ObjTemp.Count - 1].transform.Find("Image").GetComponent<Image>().sprite = child.ShopImage;
				Buy_Button Temp_buy_Button = List_ObjTemp[List_ObjTemp.Count - 1].transform.Find("Button").gameObject.AddComponent<Buy_Button>();
				Temp_buy_Button.setShopItemData(child,kind);
                
			}
		}
		if (m_ShopItemMenu_ShowOn.ContainsKey(kind)) m_ShopItemMenu_ShowOn.Remove(kind);
		m_ShopItemMenu_ShowOn.Add(kind, List_ObjTemp);
		return List_ObjTemp;
	}

	public static bool CompareShopItemData(ShopItemKind kind,List<GameObject> Old_shopItemDatas)
	{
		List<GameObject> New_List_Temp = m_ShopItemMenu_ShowOn[kind];
		if (Old_shopItemDatas.Count != New_List_Temp.Count) return false;
		if (New_List_Temp != Old_shopItemDatas) return false;
		return true;
	}


	ShopItemKind GetShopItemKind(string KindName)
	{
		if (KindName.Contains("ManSkinData")) return ShopItemKind.Man;
		if (KindName.Contains("BoxSkinData")) return ShopItemKind.Box;
		if (KindName.Contains("LavaSkinData")) return ShopItemKind.Lava;
		return ShopItemKind.None;
        
	}

	public static ShopItemData GetShopData_ByShopNumber(string vshopNum)
	{
		foreach (KeyValuePair<ShopItemKind, List<ShopItemData>> child in getShopMenuDictionary())
		{
			foreach(ShopItemData child2 in child.Value)
			{
				if(child2.ShopNumber == vshopNum)
				{
					return child2;
				}
			}
		}
		Debug.Log("No [" + vshopNum + "] shopItem!!");
		return null;
	}

    //將ＣＳＶ檔轉換成誠是可以用的ＬＩＳＴ
	List<ShopItemData> CSV2ListReader(string fileName)
    {
		CSVReader cSVReader = new CSVReader();
		List<ShopItemData> m_Datas = null;
		//var result = CSVReader.ParseCSV(File.ReadAllText(@"/Users/furukazu/data.csv"));

		// read from Resource
		// in this case, you place the csv file at Assets/Resources/CSV/data.csv 
		Debug.Log(fileName);
		var res = cSVReader.ParseCSV((Resources.Load("ExcelData/" + fileName) as TextAsset).text);

		if (m_Datas == null) m_Datas = new List<ShopItemData>();
        m_Datas.Clear();

        foreach (var line in res)
        {//讀出行的ＬＩＳＴ
            int Temp = 0;
            foreach (var col in line)
            {//讀出列的資料
                //Debug.Log("Line:" + res.IndexOf(line) + " col:" + col + " colNumber: " + line.IndexOf(col));
                if (res.IndexOf(line) == 0) break;
                if (m_Datas.Count < res.IndexOf(line))
                {
					m_Datas.Add(new ShopItemData());
                }
                switch (Temp)
                {
                    case 4:
						int.TryParse(col, out m_Datas[res.IndexOf(line) - 1].ShopPrice);
                        break;
                    case 7:
                        if (col == "True") m_Datas[res.IndexOf(line) - 1].IsOpen = true;
                        else m_Datas[res.IndexOf(line) - 1].IsOpen = false;
                        break;
                    default:
						if (Temp == 0) m_Datas[res.IndexOf(line) - 1].ShopID = col;
                        if (Temp == 1) m_Datas[res.IndexOf(line) - 1].ShopNumber = col;
                        if (Temp == 2) m_Datas[res.IndexOf(line) - 1].ShopName = col;
                        if (Temp == 3) m_Datas[res.IndexOf(line) - 1].ShopKind = col;
                        //if (Temp == 4) m_Datas[res.IndexOf(line) - 1].ShopPrice = col;
						if (Temp == 5) m_Datas[res.IndexOf(line) - 1].setShopImage(col);
						if (Temp == 6) m_Datas[res.IndexOf(line) - 1].setShopObj(col);
                        break;
                }
                Temp++;
            }
        }
        return m_Datas;
    }
}




