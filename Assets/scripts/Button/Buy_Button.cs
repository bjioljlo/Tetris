using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buy_Button : IButton {

	ShopItemData m_ShopItemData;
	ShopItemKind shopItemKind;
	public override void ClickAction()
	{
		base.ClickAction();
		Debug.Log("Click buy " + this.gameObject.name);
		Debug.Log(m_ShopItemData.IsOpen);


        //從這裡看
		switch(shopItemKind)
		{
			case ShopItemKind.Man:
				{
					FindObjectOfType<Man>().setManSkin(m_ShopItemData.ShopImage);
                    break;
				}
			case ShopItemKind.Box:
				{
					Spawner spawner = FindObjectOfType<Spawner>();
					spawner.BoxSkin = m_ShopItemData.ShopImage;
					break;
				}
			case ShopItemKind.Lava:
				{
					mainLava m_mainLava = FindObjectOfType<mainLava>();
					m_mainLava.setLavaSkin(m_ShopItemData.ShopImage);
					break;
				}
			default:
				break;

		}

	}

	public void setShopItemData(ShopItemData shopItemData,ShopItemKind m_shopItemKind)
	{
		m_ShopItemData = shopItemData;
		shopItemKind = m_shopItemKind;
	}
}
