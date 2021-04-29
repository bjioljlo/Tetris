using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buy_Button : IButton {

	ShopItemData m_ShopItemData;
	ShopItemKind shopItemKind;
	public override void ClickAction()
	{
		base.ClickAction();
		Debug.Log("Click buy " + this.gameObject.name);
		Debug.Log(m_ShopItemData.IsOpen);
		ClickBuySkin();

  //      //從這裡看
		//switch(shopItemKind)
		//{
		//	case ShopItemKind.Man:
		//		{
		//			FindObjectOfType<Man>().setManSkin(m_ShopItemData.ShopImage);
  //                  break;
		//		}
		//	case ShopItemKind.Box:
		//		{
		//			Spawner spawner = FindObjectOfType<Spawner>();
		//			spawner.BoxSkin = m_ShopItemData.ShopImage;
		//			break;
		//		}
		//	case ShopItemKind.Lava:
		//		{
		//			mainLava m_mainLava = FindObjectOfType<mainLava>();
		//			m_mainLava.setLavaSkin(m_ShopItemData.ShopImage);
		//			break;
		//		}
		//	default:
		//		break;

		//}

	}

	public void setShopItemData(ShopItemData shopItemData,ShopItemKind m_shopItemKind)
	{
	    m_ShopItemData = shopItemData;
        shopItemKind = m_shopItemKind;
		foreach(ShopItemData child in PlayerManager.get_main_playerInfo().ItemDatas)
		{
			if(m_ShopItemData.ShopNumber == child.ShopNumber)
			{
				m_ShopItemData.ShopPrice = -999;
				transform.Find("Text").GetComponent<Text>().text = "已購買";
				return;
			}
		}
		transform.Find("Text").GetComponent<Text>().text = m_ShopItemData.ShopPrice.ToString();
	}
	public void ClickBuySkin()
	{
		PlayerManager.BuyWithCoin(m_ShopItemData,shopItemKind);
		Grid.getGrid.Coin = PlayerManager.get_main_playerInfo().GoldCoin;
		//Coin_Text coin_text = FindObjectOfType<Coin_Text>();
        //coin_text.SetText(Grid.getGrid.Coin.ToString());
		setShopItemData(m_ShopItemData, shopItemKind);
	}
}
