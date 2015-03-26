using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AIdeploy : MonoBehaviour
{
    public GameObject[] EnemyShips = new GameObject[3];
	private GameObject EnemyShipsHolder;
	private List<string> usedTiles; 
    private ShipProperties ShipProperetiesScript;

	void Awake () 
    {
		EnemyShipsHolder = new GameObject("EnemyShips");
        foreach(GameObject eship in EnemyShips)
        {
            GameObject newInstance;
            newInstance=(GameObject)Instantiate(eship);
            newInstance.name = eship.name;
			newInstance.transform.SetParent(EnemyShipsHolder.transform);
        }
	}
    public void ButtonPressed()
    {
        StartCoroutine(DeployEnemyShips());
    }
    private IEnumerator DeployEnemyShips()
    {
        usedTiles = new List<string>();
        EnemyShips = GameObject.FindGameObjectsWithTag("EnemyShip");

        foreach (GameObject eship in EnemyShips)
           setPositionOfShip(eship);
        yield return new WaitForFixedUpdate();
        DisableBoxColliders();
        GameObject.Find("TerrainControl").GetComponent<BattleInitiate>().ShipsDeployed();
        // Disable button
		GameObject.Find ("Battle").GetComponent<Button>().interactable = false;
    }
    /*
     * setPosition method finds a position and rotation for the enemy ship that passes on method's input
     */
    private void setPositionOfShip(GameObject EnemyShip)
    {
        bool isVertical = false;
        bool shipCollision = false;
        List<string> currentTiles;
        ShipProperetiesScript = EnemyShip.GetComponent<ShipProperties>();
        int col, row;
        do
        {
            shipCollision = false;
            currentTiles = new List<string>();
            if (Random.Range(0, 360) > 180)
            {
                ShipProperetiesScript.ChangeShipRotation(Rotation.vertical);
                col = Mathf.FloorToInt(Random.Range(0, 10));
                row = Mathf.FloorToInt(Random.Range(0, 11-ShipProperetiesScript.getShipSize()));
                isVertical=true;
            }
            else
            {
                ShipProperetiesScript.ChangeShipRotation(Rotation.horizontal);
                col = Mathf.FloorToInt(Random.Range(0, 11-ShipProperetiesScript.getShipSize()));
                row = Mathf.FloorToInt(Random.Range(0, 10));
            }
            currentTiles.AddRange(GenerateUsedShipTiles(row,col,isVertical));
            Debug.Log("current tiles "+ ShipProperetiesScript.toString(currentTiles));
            foreach (string tile in currentTiles) 
            {
                if(usedTiles.Contains(tile))
                    shipCollision=true;
            }
        } while (shipCollision);
        EnemyShip.transform.position = ShipProperetiesScript.ShipPosition(EnemyShip.transform.rotation.z, Utilities.IntToTileName(row, col,true));
        usedTiles.AddRange(currentTiles);
        Debug.Log("row" + row + " col" + col + " " + ShipProperetiesScript.name + " usedTile contains " + ShipProperetiesScript.toString(usedTiles)+" shipCollision "+shipCollision);
        
    }
    /*
     * GenerateUsedShipTiles method return a List<String> that contains the current elected ship tiles
     * int col is the elected column
     * int row is the elected row 
     * bool isVertical is the elected orientation of ship
     */
    private List<string> GenerateUsedShipTiles(int row,int col,bool isVertical)
    {
        List<string> returnList = new List<string>();
        int i = ShipProperetiesScript.getShipSize() - 1;
        while (i >= 0)
        {
            if (isVertical)
                returnList.Add(Utilities.IntToTileName(row + i, col,true));
            else
                returnList.Add(Utilities.IntToTileName(row, col+i,true));
            i--;
        }
        return returnList;
    }
    /*
     * DisableBoxColliders method disables the colliders of the enemy ships
     */
    private void DisableBoxColliders()
    {
        foreach (GameObject eship in EnemyShips)
            eship.GetComponent<BoxCollider2D>().enabled = false;
    }
}
