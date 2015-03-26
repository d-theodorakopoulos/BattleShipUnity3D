using UnityEngine;
using System.Collections;

public class ShipEventScript : MonoBehaviour 
{
    private ShipProperties ShipPropertiesScript;

    void Awake()
    {
        ShipPropertiesScript = this.GetComponent<ShipProperties>();
    }
    void OnTriggerEnter2D(Collider2D col)
    {
		if(col.gameObject.tag == "Ship")
		{
			return;
		}
        else if (col.gameObject.tag == "Tile")
        {
            ShipPropertiesScript.tileNames.Add(col.gameObject.name);
            ShipPropertiesScript.tileNames.Sort();
            //Debug.Log(sp.name+ " we have collision with " + sp.toString(sp.tileNames));
        }
    }
	void OnTriggerExit2D(Collider2D col)
	{
		if (col.gameObject.tag == "Ship" && ShipPropertiesScript.isSelected)
		{
			ShipPropertiesScript.collisionWithShip = false;
		}
		if (col.gameObject.tag == "Ship")
			return;
		else if (col.gameObject.tag == "Tile")
		{
			ShipPropertiesScript.tileNames.Remove(col.gameObject.name);
			ShipPropertiesScript.tileNames.Sort();
			//Debug.Log(sp.name+" we have collision with " + sp.toString(sp.tileNames));
		}
	}
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ship" && ShipPropertiesScript.isSelected)
        {
            Debug.Log("You cant dock a ship above an other ship!");
            ShipPropertiesScript.collisionWithShip = true;
        }
		if (col.gameObject.layer == LayerMask.NameToLayer("EnemyTile") && !ShipPropertiesScript.isSelected)
		{
			Debug.Log("You cant dock a ship on enemy tile!");
			ShipPropertiesScript.wrongSideDock = true;
		}
    }
}
