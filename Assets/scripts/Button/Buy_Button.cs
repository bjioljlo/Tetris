using UnityEngine;
using UnityEngine.UI;

public class Buy_Button : IButton
{

	ShopItemData m_ShopItemData;
	ShopItemKind shopItemKind;
	public override void ClickAction()
	{
		base.ClickAction();
		Debug.Log("Click buy " + this.gameObject.name);
		Debug.Log(m_ShopItemData.IsOpen);
		ClickBuySkin();
	}

	public void setShopItemData(ShopItemData shopItemData, ShopItemKind m_shopItemKind)
	{
		m_ShopItemData = shopItemData;
		shopItemKind = m_shopItemKind;
		foreach (ShopItemData child in PlayerManager.get_main_playerInfo().ItemDatas.ToList())
		{
			if (m_ShopItemData.ShopNumber == child.ShopNumber)
			{
				m_ShopItemData.ShopPrice = -999;
				transform.Find("Text").GetComponent<Text>().text = "已購買";
				if (PlayerManager.get_main_playerInfo().PresetItemDatas.Contains(m_ShopItemData))
				{
					transform.Find("Text").GetComponent<Text>().text = "使用中";
					Debug.Log("kind:" + shopItemKind + "/data:" + shopItemData.ShopNumber);
					PlayerManager.BuyWithCoin(m_ShopItemData, shopItemKind);
				}
				return;
			}
		}
		transform.Find("Text").GetComponent<Text>().text = m_ShopItemData.ShopPrice.ToString();
	}
	public void ClickBuySkin()
	{
		PlayerManager.BuyWithCoin(m_ShopItemData, shopItemKind);
		Grid.getGrid.Coin = PlayerManager.get_main_playerInfo().GoldCoin;
		ShopManager.RefreshShopItemGameObj(shopItemKind);
	}
}
