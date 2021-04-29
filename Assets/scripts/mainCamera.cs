using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainCamera : IMainBehavier {
    public GameObject Prefab_Background;
    public GameObject NextPos;
    public GameObject Go_Man;
    public List<List<Transform>> grids;

	private void Awake()
	{
		Grid.getGrid.mainCamera = this;
	}

	private void Start()
	{
        NextPos = GameObject.Find("NextPosition");
        Go_Man = GameObject.FindGameObjectWithTag("man");
	}
	public override void IPauseUpdate()
	{

        if (transform.position.y <= Go_Man.transform.position.y)
            FixCameraWithMan();
 
        if ((int)transform.position.y == (int)NextPos.transform.position.y - 10)
            AddNextBackground();

	}

    void AddNextBackground()
    {
        GameObject bgTemp = Instantiate(Prefab_Background,NextPos.transform.position,NextPos.transform.rotation);
        NextPos.SetActive(false);
        NextPos = bgTemp.transform.Find("NextPosition").gameObject;
		Grid.getGrid.InitGrids();
		FindObjectOfType<Spawner>().spawnBag();
		FindObjectOfType<Spawner>().spawnCoin();
    }

    void FixCameraWithMan()
    {
        Vector3 pos = transform.position;
		pos.y = Go_Man.transform.position.y + 1;
        transform.position = pos;
    }
}
