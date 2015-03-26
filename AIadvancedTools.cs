using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIadvancedTools : MonoBehaviour
{
	private int smallestShipSize;
	private List<string> newReturnedAvailableTilesList = new List<string>();
	private bool leftSideBlocked;
	private bool rightSideBlocked;
	private bool upSideBlocked;
	private bool downSideBlocked;
	
	public List<string> DeleteTrashTiles(List<string> tiles)
	{
		TileScript currentTileScript;
		smallestShipSize = FindSmallestShip();

		newReturnedAvailableTilesList.Clear();
		AnalyzeBoard(tiles);

		foreach(string tile in tiles)
		{	
			currentTileScript = GameObject.Find(tile).GetComponent<TileScript>();
			if(currentTileScript.features.horizontalCount < smallestShipSize && currentTileScript.features.verticalCount < smallestShipSize)
			{
				GameObject.Find(currentTileScript.name).GetComponent<TileScript>().DisableTile(false);
				GameObject.Find(currentTileScript.name).GetComponent<SpriteRenderer>().color = Color.magenta;
			}
			else if((currentTileScript.features.horizontalCount == smallestShipSize && currentTileScript.features.verticalCount < smallestShipSize) ||
			        (currentTileScript.features.horizontalCount < smallestShipSize && currentTileScript.features.verticalCount == smallestShipSize))
			{	
				newReturnedAvailableTilesList.Add(currentTileScript.name);
				GameObject.Find (currentTileScript.name).GetComponent<SpriteRenderer>().color = Color.cyan;
			}

			else if((currentTileScript.features.horizontalCount == smallestShipSize +1 && currentTileScript.features.verticalCount < smallestShipSize) ||
			        (currentTileScript.features.horizontalCount < smallestShipSize && currentTileScript.features.verticalCount == smallestShipSize + 1))
			{	
				newReturnedAvailableTilesList.Add(currentTileScript.name);
				GameObject.Find(currentTileScript.name).GetComponent<SpriteRenderer>().color = Color.blue;
			}
			else
				newReturnedAvailableTilesList.Add(currentTileScript.name);
		}
		return newReturnedAvailableTilesList;
	}

	private void AnalyzeBoard(List<string> tiles)
	{
		TileScript currentTileScript;	
		foreach(string tile in tiles)
		{
			currentTileScript = GameObject.Find(tile).GetComponent<TileScript>();
			leftSideBlocked = false;
			rightSideBlocked = false;
			upSideBlocked = false;
			downSideBlocked = false;
			
			int horizontalCount = 1;
			int verticalCount = 1;
			for(int i= 1; i < smallestShipSize + 2; i++)
			{
				// Right Search //
				if(tiles.Contains(Utilities.ChangeTileCol(tile,i)) && !rightSideBlocked)//if not contains the tile
					horizontalCount++;
				else
					rightSideBlocked = true;
				 /////////////////
				// Left Search //
				if(tiles.Contains(Utilities.ChangeTileCol(tile,-i)) && !leftSideBlocked)
					horizontalCount++;
				else
					leftSideBlocked = true;
				 ///////////////
				// Up Search //
				if(tiles.Contains(Utilities.ChangeTileRow(tile,-i)) && !upSideBlocked)
					verticalCount++;
				else
					upSideBlocked = true;
				 /////////////////
				// Down Search // 
				if(tiles.Contains(Utilities.ChangeTileRow(tile,i)) && !downSideBlocked)
					verticalCount++;
				else
					downSideBlocked = true;
			}
			currentTileScript.features = new TileScript.TileFeatures(horizontalCount,verticalCount);
			//Debug.Log("Tile "+ currentTileScript.name+" Horizontal "+ currentTileScript.features.horizontalCount+ " Vertical "+ verticalCount+ " weight "+ (horizontalCount+verticalCount));
		}
	}

	private int FindSmallestShip()
	{
		int min = 5;
		foreach(GameObject ship in GameObject.FindGameObjectsWithTag("Ship"))
		{
			if(ship.GetComponent<ShipProperties>().getShipSize() < min && 
			   ship.GetComponent<ShipProperties>().getShipStatus() == ShipStatus.InBattle)

				min = ship.GetComponent<ShipProperties>().getShipSize();
		}
		Debug.Log(min);
		return min;
	}
}
