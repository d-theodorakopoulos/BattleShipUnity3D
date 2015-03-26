using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Rotation { vertical=270,horizontal=0};
public enum ShipStatus { InBattle, KIA};

public class ShipProperties : MonoBehaviour 
{
    public bool isSelected = false;
    public bool isDocked = true;
    public bool collisionWithShip = false;
	public bool wrongSideDock = false;
    public List<string> tileNames = new List<string>();
    private ShipStatus StatusOfShip = ShipStatus.InBattle;
	private int shipSize = 0;
	private int shipHealth = 0;

    void Awake()
    {
        CalculateShipSize();
    }
    public string toString(List<string> list)
    {
        string s = "{";
        foreach (string str in list)
            s += str + "}{";
        s.Remove(s.Length - 1);
        return s;
    }
	private void CalculateShipSize()
    {
        shipSize = Mathf.FloorToInt(GetComponent<Renderer>().bounds.size.x / 2);
        shipHealth = shipSize;
        //Debug.Log(shipSize);
    }
    public void damageShip()
    {
        shipHealth--;
        if (shipHealth <= 0)
        {
            Debug.Log(name + " was sank");
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.7f);
            StatusOfShip = ShipStatus.KIA;
			if(gameObject.tag == "Ship")
				GameObject.Find("EnemyControl").GetComponent<AIknowledge>().RemoveTilesThatSunk(tileNames);
			GameObject.Find("TerrainControl").GetComponent<HitSystem>().CheckShips();
        }
    }
    public int getShipHealth()
    {
        return shipHealth;
    }
    public int getShipSize()
    {
        return shipSize;
    }
    public ShipStatus getShipStatus()
    {
        return StatusOfShip;
    }
    public void ChangeShipRotation(Rotation r)
    {
        Quaternion horizontal = Quaternion.Euler(0, 0, 0);
        Quaternion vertical = Quaternion.Euler(0, 0, 270f);
        if (r==Rotation.horizontal)
            transform.rotation = horizontal;
        else
            transform.rotation = vertical;
    }

    public Vector3 ShipPosition(float rotation)
    {
        if (rotation == 0)
        	return transform.position = GameObject.Find(tileNames[0]).transform.position + new Vector3(getShipSize() - 0.7f, 0, 0);
        else
            return transform.position = GameObject.Find(tileNames[0]).transform.position + new Vector3(0, -getShipSize() + 0.7f, 0);
    }
    public Vector3 ShipPosition(float rotation,string firstTile)
    {
        if (rotation == 0)
            return transform.position = GameObject.Find(firstTile).transform.position + new Vector3(getShipSize() - 0.7f, 0, 0);
        else
            return transform.position = GameObject.Find(firstTile).transform.position + new Vector3(0, -getShipSize() + 0.7f, 0);
    }
}
