using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum HitStatus { Hit,Miss,NoNeighbourHit,ChangeOrientation,ShipSunk };

public class HitSystem : MonoBehaviour
{
    private HitStatus AILastHitStatus = HitStatus.Miss;
    private TerrainControl terrainControlScript;
	private AIknowledge aiKnowledgeScript;
	private TileScript tileScript;
	private string tileName;
	private MessageControl messageControlScript;

    void Start()
    {
        //StartCoroutine("CheckGameStatus");
        terrainControlScript = GameObject.Find("TerrainControl").GetComponent<TerrainControl>();
		aiKnowledgeScript = GameObject.Find("EnemyControl").GetComponent<AIknowledge>();
		messageControlScript = GameObject.Find("GameControl").GetComponent<MessageControl>();
    }
    /*
     * CheckGameStatus method check the game status
     * if the game status changes reveal the winner and finalize the terrain
     */
    private void CheckGameStatus()
    {
        if(terrainControlScript.getGameStatus() != BattleStatus.Battle)
        {
			messageControlScript.showWarningMessage(terrainControlScript.getGameStatus().ToString());
            GameObject.Find("TerrainControl").GetComponent<TerrainControl>().finalizeTerrain();
        }
    }
    /*
     * CheckShips method check all ships if they were sunk
     * Return an enum depending of the ships status
     */
    public void CheckShips()
    {
        GameObject[] EnemyShips = GameObject.FindGameObjectsWithTag("EnemyShip");
        GameObject[] PlayerShips = GameObject.FindGameObjectsWithTag("Ship");
        int enemyShipsHealth=0, playerShipsHealth=0;

        foreach(GameObject EnemyShip in EnemyShips)
            enemyShipsHealth += EnemyShip.GetComponent<ShipProperties>().getShipHealth();
        foreach (GameObject PlayerShip in PlayerShips)
            playerShipsHealth += PlayerShip.GetComponent<ShipProperties>().getShipHealth();

        if (enemyShipsHealth > 0 && playerShipsHealth > 0)
			terrainControlScript.setBattleStatus(BattleStatus.Battle);
        else if (enemyShipsHealth == 0)
		{
			terrainControlScript.setBattleStatus(BattleStatus.PlayerWins);
			CheckGameStatus();
		}
        else
		{	
			terrainControlScript.setBattleStatus(BattleStatus.EnemyWins);
			CheckGameStatus();
		}
    }

    public void HitShip()
    {
       	tileName = terrainControlScript.getObjName();

        if(terrainControlScript.getGameStatus() == BattleStatus.Deploy)
        {
			messageControlScript.showWarningMessage("We are in Deploy Stage\ndeploy you ships first!");
            return ;
        }
        else if(tileName==null)
        {
			messageControlScript.showWarningMessage("Select a Tile and press Hit!");
            return ;
        }
        else
        { 
            tileScript = GameObject.Find(tileName).GetComponent<TileScript>();
            string shipName = tileScript.getShipName();

            if (shipName != null)
            {
                //Debug.Log("We hit");
				if(terrainControlScript.getSide() == Player.Computer)
					aiKnowledgeScript.AddSuccessfullHit(tileName);
                GameObject.Find(shipName).GetComponent<ShipProperties>().damageShip();
                tileScript.DisableTile(true);
                terrainControlScript.CheckTileSelectionLimit(tileName);
                if (terrainControlScript.getSide() == Player.Computer)
                {
                    AILastHitStatus = HitStatus.Hit;

					if (GameObject.Find(shipName).GetComponent<ShipProperties>().getShipStatus() == ShipStatus.KIA)
						AILastHitStatus = HitStatus.ShipSunk;
                }
            }
            else
            {
                //Debug.Log("No Ship hit");
                tileScript.DisableTile(false);
                terrainControlScript.CheckTileSelectionLimit(tileName);
                if (terrainControlScript.getSide() == Player.Computer)
                {
                    if (AILastHitStatus == HitStatus.Hit ||
					   (AILastHitStatus == HitStatus.ShipSunk && aiKnowledgeScript.successfullHittedTiles.Count > 0))
                        AILastHitStatus = HitStatus.NoNeighbourHit;
                    else if (AILastHitStatus == HitStatus.NoNeighbourHit)
                    {
                        AILastHitStatus = HitStatus.ChangeOrientation;
                        aiKnowledgeScript.ChangeOrientation();
                    }
                    else if (AILastHitStatus == HitStatus.ChangeOrientation) 
                        AILastHitStatus = HitStatus.NoNeighbourHit;
                    else
                        AILastHitStatus = HitStatus.Miss;

                }
            }
			if (terrainControlScript.getSide() == Player.Computer)
            	Debug.Log(AILastHitStatus);
        }
        terrainControlScript.changeSide(); // Change Player side after a Hit
    }

    public HitStatus getAILastHitStatus()
    {
        return AILastHitStatus;
    }
}
