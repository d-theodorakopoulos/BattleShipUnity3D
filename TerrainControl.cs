using UnityEngine;
using System.Collections;

public enum Player { Human,Computer};
public enum BattleStatus {Deploy,Battle,PlayerWins,EnemyWins};

public class TerrainControl : MonoBehaviour
{
    public GameObject playerTilePrefab;
	public GameObject enemyTilePrefab;
	public int clicklimit = 1;
	private GameObject TileHolder;
    private int tilesClicked = 0;
    private Player side=Player.Human;
	private string objName=null;
	private BattleStatus gameStatus = BattleStatus.Deploy;
	private float firstTileOffset = 0;

	void Awake()
	{
		TileHolder = new GameObject("TileHolder");
		//TileHolder.transform.position = new Vector3(0,0,0);
	}

	void Start()
    {
		/*if (ScreenManager.getNewLocalScale() != 0)
		{
			tile.transform.localScale = new Vector3(ScreenManager.getNewLocalScale(),
			                                        ScreenManager.getNewLocalScale());

			foreach (GameObject ship in GameObject.FindGameObjectsWithTag("Ship"))
			{
				ship.transform.localScale = new Vector3(ScreenManager.getNewLocalScale(),
				                                        ScreenManager.getNewLocalScale());
			}
			foreach (GameObject eship in GameObject.FindGameObjectsWithTag("EnemyShip"))
			{
				eship.transform.localScale = new Vector3(ScreenManager.getNewLocalScale(),
				                                         ScreenManager.getNewLocalScale());
			}
		}*/
        Vector3 spawnPosition = Utilities.ScreenToWorld(0,Screen.height -30, 10);
        float endPoint=CreateTerrain(spawnPosition, true);

        spawnPosition = Utilities.ScreenToWorld(endPoint + 15,Screen.height -30, 10); 
        CreateTerrain(spawnPosition,false);
		// Correct first tile's offset depending screen's aspect ratio //
		firstTileOffset = (Vector3.Distance(Utilities.ScreenToWorld( new Vector3(0,0,0)), Utilities.ScreenToWorld( new Vector3(Screen.width,0,0)))-
		                   Vector3.Distance(GameObject.Find("ERow0ECol0").transform.position, GameObject.Find("Row0Col9").transform.position)) / 2;
		TileHolder.transform.position = new Vector3 (firstTileOffset,0,0);

		foreach(GameObject ship in GameObject.FindGameObjectsWithTag("Ship"))
		{
			ship.GetComponent<ShipPlayerAbilities>().FirstPlaceOfShip(GameObject.Find("ERow9ECol0").transform);
		}
    }
	void OnApplicationQuit()
	{
		enemyTilePrefab.transform.localScale = new Vector3(1,1,1);
		playerTilePrefab.transform.localScale = new Vector3(1,1,1);
		foreach (GameObject ship in GameObject.FindGameObjectsWithTag("Ship"))
		{
			ship.transform.localScale = new Vector3(1,1,1);
		}
		foreach (GameObject eship in GameObject.FindGameObjectsWithTag("EnemyShip"))
		{
			eship.transform.localScale = new Vector3(1,1,1);
		}
	}
	void Update()
	{
		if (Input.GetKey("escape"))
		{
			Debug.Log("Quit");
			Application.Quit();
		}
	}
    public bool CheckTileSelectionLimit(string name)
    {
        if(side==Player.Computer)
            Debug.Log(name +" "+tilesClicked);
        if (name.Equals(objName))
        {
            tilesClicked--;
            objName = null;
            return false;
        }
        else if (tilesClicked >= clicklimit)
        {
            return true;
        }
        else
        {
            tilesClicked++;
            objName = name;
            return false;
        }
    }
	private float CreateTerrain(Vector3 spawnPoint,bool enemy)
    {
        GameObject newTile;
        Quaternion rotation = new Quaternion(0, 0, 0, 0);
        Vector3 startPoint=spawnPoint;
		//float spaceOfTiles = tile.renderer.bounds.size.x + ScreenManager.getNewLocalScale();
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (enemy)
                {
					newTile = (GameObject)Instantiate(enemyTilePrefab, spawnPoint, rotation);
                    newTile.name = "ERow" + j + "ECol" + i;
                }
                else
				{
					newTile = (GameObject)Instantiate(playerTilePrefab, spawnPoint, rotation);
					newTile.name = "Row" + j + "Col" + i;
				}
				newTile.transform.SetParent(TileHolder.transform);

				spawnPoint.y -= 2.2f;//spaceOfTiles;
            }
			spawnPoint.x += 2.2f;//spaceOfTiles;
            spawnPoint.y = startPoint.y;
        }
        return Utilities.WorldToScreen(spawnPoint.x,0,0).x;
    }

    public void finalizeTerrain()
    {
        GameObject.Find("EnemyControl").SetActive(false);
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach(GameObject tile in tiles)
        {
            tile.GetComponent<TileScript>().enabled = false;
            tile.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
    public string getObjName()
    {
        return objName;
    }
    public BattleStatus getGameStatus()
    {
        return gameStatus;
    }
    public void setBattleStatus(BattleStatus newStatus)
    {
		gameStatus = newStatus;
    }
    public Player getSide()
    {
        return side;
    }
    public void changeSide()
    {
        if (side == Player.Computer)
            side = Player.Human;
        else
            side = Player.Computer;
    }
    public string PlayerSideToTileLayer()
    {
        if (side == Player.Computer)
            return "Tile";
        else
            return "EnemyTile";
    }
}
