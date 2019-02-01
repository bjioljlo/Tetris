using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ctl_ScrollView : MonoBehaviour {

	List<GameObject> List_ShopContentObj = null;
    GameObject content;
	HorizontalLayoutGroup horizontalLayoutGroup = null;
	VerticalLayoutGroup verticalLayoutGroup = null;
	ScrollRect scrollRect = null;
	float contentNumber = 0;

	public ShopItemKind shopkind;

	// Use this for initialization
	void Start () {
		scrollRect = GetComponent<ScrollRect>();
		content = transform.Find("Viewport/Content").gameObject;
		scrollRect.horizontalScrollbar = null;
		scrollRect.verticalScrollbar = null;
		scrollRect.transform.Find("Scrollbar Horizontal").gameObject.SetActive(false);
		scrollRect.transform.Find("Scrollbar Vertical").gameObject.SetActive(false);
		scrollRect.gameObject.GetComponent<Image>().enabled = false;

		if(scrollRect.horizontal)
		{
			horizontalLayoutGroup = content.AddComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroup.childForceExpandWidth = false;
			horizontalLayoutGroup.childForceExpandHeight = true;
			horizontalLayoutGroup.childControlHeight = false;
			horizontalLayoutGroup.childControlWidth = false;
			horizontalLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
			content.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
			content.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
            
            
		}
		if (scrollRect.vertical)
		{
			verticalLayoutGroup = content.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.childForceExpandWidth = true;
			verticalLayoutGroup.childForceExpandHeight = false;
			verticalLayoutGroup.childControlWidth = false;
			verticalLayoutGroup.childControlHeight = false;
			verticalLayoutGroup.childAlignment = TextAnchor.UpperCenter;
			content.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
            content.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);

		}
		content.GetComponent<RectTransform>().sizeDelta = new Vector2(1, 1);
		//setScrollView_content();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
	public void setScrollView_content()
	{
		//先檢查是否有實體化過
		if(List_ShopContentObj != null && ShopManager.CompareShopItemData(shopkind,List_ShopContentObj))
		{
			return;
		}
		List_ShopContentObj = null;
		//實體化
		List_ShopContentObj = ShopManager.GetShopItemGameObj(shopkind);
        //如果沒有東西就跳掉
		if(List_ShopContentObj == null)
		{
			return;
		}

		foreach (GameObject child in List_ShopContentObj)
		{
			//設定內容物
			child.transform.SetParent(content.transform);
			//設定scrollview的框框範圍
			setScrollView_Range(child);
		}      
	}

	void setScrollView_Range(GameObject AddObj)
	{
		//設定scrollview的框框範圍
        if (horizontalLayoutGroup == null)
        {
			contentNumber += AddObj.GetComponent<RectTransform>().rect.height;
			content.GetComponent<RectTransform>().sizeDelta = new Vector2(1, contentNumber);
        }
        else if (verticalLayoutGroup == null)
        {
			contentNumber += AddObj.GetComponent<RectTransform>().rect.width;
            content.GetComponent<RectTransform>().sizeDelta =
				       new Vector2(contentNumber, 1);
        }
        else
        {
            Debug.LogError("Scroll view group is not set!!");
        }
	}
}
