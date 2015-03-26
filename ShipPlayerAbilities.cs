using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipPlayerAbilities : MonoBehaviour
{
    private TerrainControl TerrainControlScript;
    private ShipProperties ShipPropertiesScript;
    private Vector3 startPosition;

    void Awake()
    {
        TerrainControlScript = GameObject.Find("TerrainControl").GetComponent<TerrainControl>();
        ShipPropertiesScript = GetComponent<ShipProperties>();
    }
    public void FirstPlaceOfShip(Transform lowerTile)
    {
        startPosition = new Vector3(lowerTile.position.x + 2f + GetComponent<Renderer>().bounds.size.x/2 * (ShipPropertiesScript.getShipSize()-3)*2,
		                            lowerTile.position.y - GetComponent<Renderer>().bounds.size.y,
		                            10f);
			//Utilities.ScreenToWorld(Screen.width - 100, Screen.height - 50 * sp.getShipSize(), 10f);
		transform.position = startPosition;
    }
	void FixedUpdate() 
    {
        if(TerrainControlScript.getGameStatus() == BattleStatus.Deploy)
            moveShip();
	}
    /*
     * moveShip() translate the ship when we click it
     * and ensure the right deploy of the ship
     */
    private void moveShip()
    {
        if (ShipPropertiesScript.isSelected)
        {
            transform.position = Utilities.ScreenToWorld(Input.mousePosition.x, Input.mousePosition.y, 10f);
        }
        else if (ShipPropertiesScript.collisionWithShip || ShipPropertiesScript.wrongSideDock
		         || (ShipPropertiesScript.tileNames.Count < ShipPropertiesScript.getShipSize() && transform.position != startPosition))
        {
            transform.position = startPosition;
            transform.rotation = Quaternion.identity;
            ShipPropertiesScript.collisionWithShip = false;
			ShipPropertiesScript.wrongSideDock = false;
        }
        else if (ShipPropertiesScript.isDocked && ShipPropertiesScript.tileNames.Count != 0)
        {
            ShipPropertiesScript.ShipPosition(transform.rotation.z);
        }
        else
            return;
    }
    /*
     * OnMouseDown() method is activated when the mouse cursor click on the collider of a ship.
     */
    void OnMouseDown ()
    {
        ShipPropertiesScript.isSelected = !ShipPropertiesScript.isSelected;
        ShipPropertiesScript.isDocked = !ShipPropertiesScript.isDocked;
    }
    /*
     * OnGUI() method is activated when an event happens
     * When we press the SPACE key, we change the rotation of the selected ship
     */
    void OnGUI()
    {
        Event e = Event.current;
        if (e.Equals(Event.KeyboardEvent("space")) && ShipPropertiesScript.isSelected)
        {
            if (transform.rotation.z == 0)
                ShipPropertiesScript.ChangeShipRotation(Rotation.vertical);
            else
                ShipPropertiesScript.ChangeShipRotation(Rotation.horizontal);
        }
    }
}
