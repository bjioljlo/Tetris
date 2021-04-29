using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour {
	public GameObject[] groups;
    public GameObject[] items;
	public GameObject bag;
	public GameObject coin;
    public Button Btn_ADS;
    public Button Btn_Setting;
    public Button Btn_CloseSetting;
    public Button Btn_Shop;
    public Button Btn_CloseShop;
	public Button Btn_Account;
	public Button Btn_CloseAccount;
    //人物
    GameObject[] mans;

	public bool IsDownItem = false;

    public Button btn_left;
    public Button btn_right;
    public Button btn_down;
    public Button btn_row;

    //InputField DebugBoxSpeed;

	public Text txt_WatchAdsTime;
	public Button btn_WatchAds;
	int m_WatchAdsTime;
	public Sprite BoxSkin = null;

	public void Awake()
    {
		Grid.getGrid.InitGrids();
		Grid.getGrid.setSpawner(this);
    }

	// Use this for initialization
	void Start () {

        Btn_ADS = GameObject.Find("Btn_ADS").GetComponent<Button>();

        Btn_Setting = GameObject.Find("Btn_Setting").GetComponent<Button>();
        Btn_Setting.onClick.AddListener(OpenSetting);

        Btn_CloseSetting = GameObject.Find("Btn_CloseSetting").GetComponent<Button>();
        Btn_CloseSetting.onClick.AddListener(CloseSetting);

        Btn_Shop = GameObject.Find("Btn_Shop").GetComponent<Button>();
        Btn_Shop.onClick.AddListener(OpenShop);

        Btn_CloseShop = GameObject.Find("Btn_CloseShop").GetComponent<Button>();
        Btn_CloseShop.onClick.AddListener(CloseShop);

		Btn_Account = GameObject.Find("Btn_Comic").GetComponent<Button>();
		Btn_Account.onClick.AddListener(OpenAccount);

		Btn_CloseAccount = GameObject.Find("Btn_CloseAccount").GetComponent<Button>();
		Btn_CloseAccount.onClick.AddListener(CloseAccount);

        mans = GameObject.FindGameObjectsWithTag("man");

        btn_left = GameObject.Find("Btn_left").GetComponent<Button>();
        btn_right = GameObject.Find("Btn_right").GetComponent<Button>();
        btn_down = GameObject.Find("Btn_down").GetComponent<Button>();
        btn_row = GameObject.Find("Btn_Row").GetComponent<Button>();

		txt_WatchAdsTime = GameObject.Find("Txt_WatchAdsTime").GetComponent<Text>();
		m_WatchAdsTime = Grid.getGrid.WatchAdsTimer;
		txt_WatchAdsTime.text = m_WatchAdsTime.ToString();
		txt_WatchAdsTime.gameObject.SetActive(false);

		btn_WatchAds = GameObject.Find("Btn_WatchADS").GetComponent<Button>();
        

	}
	
	public void spawnNext() {

        BoxGroupFactory creatgroup = new CreatGroupFactoryByName(mans, btn_left, btn_right, btn_down, btn_row);

		if(IsDownItem)//物品開關
		{
			int i = Random.Range(0, groups.Length + 1);
            if (i == groups.Length)
            {
                i = Random.Range(0, items.Length);
                creatgroup.setTarget(Instantiate(items[i], transform.position, Quaternion.identity));
                creatgroup.CreatGroupByName(BoxGroupFactory.GroupType.itemGroup);
            }
            else
            {
                creatgroup.setTarget(Instantiate(groups[i], transform.position, Quaternion.identity));
                creatgroup.CreatGroupByName(BoxGroupFactory.GroupType.normalGroup);
            }

		}
		else
		{
			int i = Random.Range(0, groups.Length);
			creatgroup.setTarget(Instantiate(groups[i], transform.position, Quaternion.identity));
			creatgroup.setBoxSkin(BoxSkin);
            creatgroup.CreatGroupByName(BoxGroupFactory.GroupType.normalGroup);
            
		}

	}

	public void spawnBag()
	{
        BoxGroupFactory creatgroup = new CreatGroupFactoryByName(mans, btn_left, btn_right, btn_down, btn_row);


		int times = Random.Range(0, 5 - Grid.getGrid.InitTimes + 1);
		for (int i = 0; i <= times;i++)
		{
			int x = Random.Range(0, Grid.getGrid.Width);
			int y = Random.Range(Grid.getGrid.Height_Now - Grid.getGrid.Height, Grid.getGrid.Height_Now);
			creatgroup.setTarget(Instantiate(bag, new Vector3(x, y, 0), Quaternion.identity));
			creatgroup.CreatGroupByName(BoxGroupFactory.GroupType.itemGroup);
		}
	}

	public void spawnCoin()
	{
		BoxGroupFactory creatgroup = new CreatGroupFactoryByName(mans, btn_left, btn_right, btn_down, btn_row);

		int times = Random.Range(0, 5 - Grid.getGrid.InitTimes + 1);
		for (int i = 0; i <= times;i++)
		{
			int x = Random.Range(0, Grid.getGrid.Width);
			int y = Random.Range(Grid.getGrid.Height_Now - Grid.getGrid.Height, Grid.getGrid.Height_Now);
			creatgroup.setTarget(Instantiate(coin, new Vector3(x, y, 0), Quaternion.identity));
			creatgroup.CreatGroupByName(BoxGroupFactory.GroupType.itemGroup);
		}
	}

	public void OpenAccount()
	{
		FindObjectOfType<mainAccount_Account>().moveIn();
		FindObjectOfType<Comic_Button>().moveOut();
	}

	public void CloseAccount()
    {
		FindObjectOfType<mainAccount_Account>().moveOut();
		FindObjectOfType<Comic_Button>().moveIn();
    }

	public void OpenShop()
    {
        FindObjectOfType<mainShop_Shop>().moveIn();
        FindObjectOfType<Shop_Button>().moveOut();
    }

    public void CloseShop()
    {
        FindObjectOfType<mainShop_Shop>().moveOut();
        FindObjectOfType<Shop_Button>().moveIn();
    }


    public void OpenSetting()
    {
        FindObjectOfType<mainSetting_Setting>().moveIn();
        FindObjectOfType<Setting_Button>().moveOut();
    }

    public void CloseSetting()
    {
        FindObjectOfType<mainSetting_Setting>().moveOut();
        FindObjectOfType<Setting_Button>().moveIn();
    }

	public void GameOver()
	{
		if (FindObjectOfType<iTween>())//刪除ＭＡＮ中的ＩＴＷＥＥＮ
			Destroy(FindObjectOfType<iTween>());

		FindObjectOfType<Restart_Button>().moveIn();
		FindObjectOfType<Best_Text>().moveIn();
		FindObjectOfType<Score_Text>().moveIn();
		FindObjectOfType<mainGameover_IUI>().moveIn();
		FindObjectOfType<Pause_Button>().moveOut();
		FindObjectOfType<Coin_Text>().moveIn();

		if (AdManager.IsJumpAdsLoaded()){
			AdManager.ShowJumpAds();	
		}
		else{
			Debug.Log("ads is not ready");
		}


		if(m_WatchAdsTime >= 5 && btn_WatchAds.IsActive())
		{
			InvokeRepeating("WatchAdsTimer", 1.2f, 1f);
			txt_WatchAdsTime.gameObject.SetActive(true);
		}

    }

	 void WatchAdsTimer()
	{
		m_WatchAdsTime = m_WatchAdsTime - 1;
		if(m_WatchAdsTime > 0)
		{
			txt_WatchAdsTime.text = m_WatchAdsTime.ToString();
		}
		else
		{
			m_WatchAdsTime = Grid.getGrid.WatchAdsTimer;
			txt_WatchAdsTime.text = m_WatchAdsTime.ToString();
			txt_WatchAdsTime.gameObject.SetActive(false);
			btn_WatchAds.gameObject.SetActive(false);
			CancelInvoke("WatchAdsTimer");
		}
	}

	public void Destroy(GameObject gameObject)
	{
		Destroy(gameObject);
	}
}
