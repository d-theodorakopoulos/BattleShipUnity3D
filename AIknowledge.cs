using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Directions {up = 0,down = 1,left = 2,right = 3};
public enum Orientation {Horizontal = 0,Vertical = 1};

public class AIknowledge : MonoBehaviour
{
    private TerrainControl terrainControlScript;
    private HitSystem HitSystemScript;
	private AIadvancedTools AIadvancedToolsScript;
    private List<string> availableTiles = new List<string>();
	public List<string> successfullHittedTiles = new List<string>();
    private HitStatus lastEnemyHitStatus = HitStatus.Miss;
	private string currentSelectedTile;
    public Directions searchDirection = Directions.left;
    public Orientation searchOrientation = Orientation.Horizontal;
    public string AnchorTile = null;
    public string lastSelectedTile = null;
	public int cleaningThreashold;
	public int cleaningTilesRate;
	public int turnCount;

    void Start()
    {
        terrainControlScript = GameObject.Find("TerrainControl").GetComponent<TerrainControl>();
        HitSystemScript = GameObject.Find("TerrainControl").GetComponent<HitSystem>();
		AIadvancedToolsScript = GetComponent<AIadvancedTools>();
        for(int i=0; i<10; i++)
        {
            for (int j = 0; j < 10; j++)
                availableTiles.Add(Utilities.IntToTileName(i, j, false));
        }
		turnCount = 100;
    }
	void Update() 
    {
	    if(terrainControlScript.getSide() == Player.Computer)
        {
			turnCount--;
			lastEnemyHitStatus = HitSystemScript.getAILastHitStatus();
			//////////// Cleaning Trash Tiles ///////////
			if(lastEnemyHitStatus == HitStatus.Miss && 
			   availableTiles.Count <= cleaningThreashold && turnCount % cleaningTilesRate == 0)
			{
				Debug.Log("Cleaning Tiles");
				availableTiles = new List<string>(AIadvancedToolsScript.DeleteTrashTiles(availableTiles));
			}
            ///////////////// NoHit statement, select a new tile to hit //
            if (lastEnemyHitStatus.Equals(HitStatus.Miss))                                                
				MissStateProcess();    

			///////////////// Hit/ChangeOrientation statement, search neighbour tiles to hit //                          
            else if (lastEnemyHitStatus.Equals(HitStatus.Hit) || lastEnemyHitStatus.Equals(HitStatus.ChangeOrientation))  
				HitStateProcess();

            ///////////////// NoNeighbourHit statement, stop search this direction and go to the opposite direction //
            else if(lastEnemyHitStatus.Equals(HitStatus.NoNeighbourHit)) 
				NoNeighbourHitProcess();

			else //if (enemyHitStatus.Equals(HitStatus.ShipSunk)
				ShipSunkStateProcess();

            HitSystemScript.HitShip();
        }
	}
    /*
     * selectTileToHit() method elects a tile for computer,to hit
     * Return the (String) name of the Tile 
     */
	private string selectTileToHit()
    {
        currentSelectedTile = availableTiles[Random.Range(0,availableTiles.Count)];
        lastSelectedTile = currentSelectedTile;
        availableTiles.Remove(currentSelectedTile);
        return currentSelectedTile;
    }
    /*
     * Go To left Tile returns the name of the left Tile of the current
     */
	private string GoToLeftTile(string currentTile)
    { 
        lastSelectedTile = Utilities.ChangeTileCol(currentTile, -1);
        if (availableTiles.Contains(lastSelectedTile))
            availableTiles.Remove(lastSelectedTile);
        else
		{
			if(availableTiles.Contains(Utilities.ChangeTileCol(AnchorTile,1)))
			{
				lastSelectedTile = GoToRightTile(AnchorTile); // if the left tile does not exist in availableTiles list, a new tile will be select 
				searchDirection = Directions.right;
			}
			else
			{
				ChangeOrientation();
				if(searchDirection == Directions.up)
					GoToUpTile(AnchorTile);
				else
					GoToDownTile(AnchorTile);
			}
		}
		return lastSelectedTile;
    }
    /*
     * Go To right Tile returns the name of the right Tile of the current
     */
	private string GoToRightTile(string currentTile)
    {
		lastSelectedTile = Utilities.ChangeTileCol(currentTile, 1);
		if (availableTiles.Contains(lastSelectedTile))
			availableTiles.Remove(lastSelectedTile);
		else
		{
			if(availableTiles.Contains(Utilities.ChangeTileCol(AnchorTile,-1)))
			{
				lastSelectedTile = GoToLeftTile(AnchorTile); // if the right tile does not exist in availableTiles list, a new tile will be select 
				searchDirection = Directions.left;
			}
			else
			{
				ChangeOrientation();
				if(searchDirection == Directions.up)
					GoToUpTile(AnchorTile);
				else
					GoToDownTile(AnchorTile);
			}
		}
		return lastSelectedTile;
    }
    /*
     * Go To Upper Tile returns the name of the upper Tile of the current
     */
    private string GoToUpTile(string currentTile)
    {
		lastSelectedTile = Utilities.ChangeTileRow(currentTile, -1);
		if (availableTiles.Contains(lastSelectedTile))
			availableTiles.Remove(lastSelectedTile);
		else
		{
			if(availableTiles.Contains(Utilities.ChangeTileRow(AnchorTile,1)))
			{
				lastSelectedTile = GoToDownTile(AnchorTile); // if the Upper tile does not exist in availableTiles list, a new tile will be select 
				searchDirection = Directions.down;
			}
			else
			{
				ChangeOrientation();
				if(searchDirection == Directions.left)
					GoToLeftTile(AnchorTile);
				else
					GoToRightTile(AnchorTile);
			}
		}
		return lastSelectedTile;
    }
    /*
     * Go To lower Tile returns the name of the lower Tile of the current
     */
    private string GoToDownTile(string currentTile)
    {
		lastSelectedTile = Utilities.ChangeTileRow(currentTile, 1);
		if (availableTiles.Contains(lastSelectedTile))
			availableTiles.Remove(lastSelectedTile);
		else
		{
			if(availableTiles.Contains(Utilities.ChangeTileRow(AnchorTile, -1)))
			{
				lastSelectedTile = GoToUpTile(AnchorTile); // if the Upper tile does not exist in availableTiles list, a new tile will be select 
				searchDirection = Directions.up;
			}
			else
			{
				ChangeOrientation();
				if(searchDirection == Directions.left)
					GoToLeftTile(AnchorTile);
				else
					GoToRightTile(AnchorTile);
			}
		}
		return lastSelectedTile;
    }

    public void ChangeOrientation()
    {
        if (searchOrientation == Orientation.Horizontal)
        {
            searchOrientation = Orientation.Vertical;
            searchDirection = (Directions)Random.Range(0, 2);
        }
        else
        {
            searchOrientation = Orientation.Horizontal;
            searchDirection = (Directions)Random.Range(2, 4);
        }
    }
	private void MissStateProcess()
	{
		currentSelectedTile = selectTileToHit();   
		AnchorTile = currentSelectedTile;
		RandomOrientationAndDirection();
		terrainControlScript.CheckTileSelectionLimit(currentSelectedTile); 
	}

	private void HitStateProcess()
	{
		if (lastEnemyHitStatus.Equals(HitStatus.ChangeOrientation))
			lastSelectedTile = AnchorTile;
		
		if(searchOrientation == Orientation.Horizontal)
		{
			if (searchDirection == Directions.left)
			{
				currentSelectedTile = GoToLeftTile(lastSelectedTile);
				terrainControlScript.CheckTileSelectionLimit(currentSelectedTile);
			}
			else //if (searchDirection == Directions.right)
			{
				currentSelectedTile = GoToRightTile(lastSelectedTile);
				terrainControlScript.CheckTileSelectionLimit(currentSelectedTile);
			}
		}
		else if(searchOrientation == Orientation.Vertical)
		{
			if (searchDirection == Directions.up)
			{
				currentSelectedTile = GoToUpTile(lastSelectedTile);
				terrainControlScript.CheckTileSelectionLimit(currentSelectedTile);
			}
			else //if (searchDirection == Directions.down)
			{
				currentSelectedTile = GoToDownTile(lastSelectedTile);
				terrainControlScript.CheckTileSelectionLimit(currentSelectedTile);
			}
		} 
	}

	private void NoNeighbourHitProcess()
	{
		if(searchDirection == Directions.left)
		{
			searchDirection = Directions.right;
			terrainControlScript.CheckTileSelectionLimit(GoToRightTile(AnchorTile));
		}
		else if(searchDirection == Directions.right)
		{
			searchDirection = Directions.left;
			terrainControlScript.CheckTileSelectionLimit(GoToLeftTile(AnchorTile));
		}
		else if (searchDirection == Directions.up)
		{
			searchDirection = Directions.down;
			terrainControlScript.CheckTileSelectionLimit(GoToDownTile(AnchorTile));
		}
		else //if (searchDirection == Directions.down)
		{
			searchDirection = Directions.up;
			terrainControlScript.CheckTileSelectionLimit(GoToUpTile(AnchorTile));
		}
	}
	private void ShipSunkStateProcess()
	{
		if(successfullHittedTiles.Count <= 0)
		{
			RandomOrientationAndDirection();
			currentSelectedTile = selectTileToHit();
			AnchorTile = currentSelectedTile;
			terrainControlScript.CheckTileSelectionLimit(currentSelectedTile);
		}
		else// there is hitted tile in successfullHittedTiles List
		{
			currentSelectedTile = successfullHittedTiles[0];
			AnchorTile = currentSelectedTile;
			lastSelectedTile = currentSelectedTile;
			HitStateProcess();
		}
	}

    private void RandomOrientationAndDirection()
    {
        int num = Random.Range(0, 2);
        searchOrientation = (Orientation)num;
        if (searchOrientation == Orientation.Horizontal)
            searchDirection = (Directions)Random.Range(2, 4);
        else
            searchDirection = (Directions)Random.Range(0, 2);
    }

	public void RemoveTilesThatSunk(List<string> shipTiles)
	{
		Utilities.RemoveListItems<string>(successfullHittedTiles,shipTiles);
	}

	public void AddSuccessfullHit(string tile)
	{
		successfullHittedTiles.Add(tile);
	}
}
