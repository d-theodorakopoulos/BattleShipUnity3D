using UnityEngine;
using System.Collections;

public class BattleInitiate : MonoBehaviour
{
    public void ShipsDeployed()
    {
        GameObject []ships;
        GameObject []tiles;
		TerrainControl terrainControlScript = GameObject.Find("TerrainControl").GetComponent<TerrainControl>();
		
        terrainControlScript.setBattleStatus(BattleStatus.Battle);
		GetComponent<HitSystem>().enabled = true;

        ships=GameObject.FindGameObjectsWithTag("Ship");
        foreach (GameObject ship in ships)
        {
            ship.GetComponent<BoxCollider2D>().enabled = false;
            ship.GetComponent<ShipPlayerAbilities>().enabled = false;
        }

        tiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach(GameObject tile in tiles)
        {  
			tile.GetComponent<BoxCollider2D>().size = new Vector2(2f, 2f);
			tile.GetComponent<BoxCollider2D>().offset = new Vector2(0f, 0f);
		}
    }
}
