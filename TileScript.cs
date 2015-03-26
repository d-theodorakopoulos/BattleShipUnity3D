using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public enum TileStates { Available,Disabled,ShipHit};

public class TileScript : MonoBehaviour 
{
    private string shipName = null;
    private TileStates tileState = TileStates.Available;
    private Sprite whiteTile;
    private TerrainControl terrainControlScript;
    private SpriteRenderer tileSpriteRenderer;
	private MessageControl messageControlScript;
	private RaycastHit2D hit;
	private Vector3 mousePos;
	private bool tileHelpPressed = false;
	public Sprite crossHair;
	public TileFeatures features;
	
	public class TileFeatures
	{
		public int horizontalCount;
		public int verticalCount;
		public int weightOfTile;
		
		public TileFeatures(int horCount,int vertCount)
		{
			horizontalCount = horCount;
			verticalCount = vertCount;
			weightOfTile = horizontalCount + verticalCount;
		}
	}
    void Start()
    {
		terrainControlScript = GameObject.Find("TerrainControl").GetComponent<TerrainControl>();
		messageControlScript = GameObject.Find("GameControl").GetComponent<MessageControl>();
        tileSpriteRenderer = GetComponent<SpriteRenderer>();
        whiteTile = tileSpriteRenderer.sprite;
		features = new TileFeatures(0,0);
    }
	void Update()
	{
		if(Input.GetMouseButtonDown(1))
		{
			tileHelpPressed = !tileHelpPressed;
		}
	}
	void OnMouseEnter()
	{
		if(terrainControlScript.getGameStatus() == BattleStatus.Battle && tileHelpPressed)
		{
			messageControlScript.StopCoroutine("ShowTileName");
			messageControlScript.StartCoroutine("ShowTileName",name+"\nHorCount"+
			                                    			   features.horizontalCount+"\nVerCount"+
			                                    				features.verticalCount);
		}
		if(gameObject.layer == LayerMask.NameToLayer("EnemyTile"))
			tileSpriteRenderer.color = new Color(0.67f, 1f, 0.66f, 1f);
	}
	void OnMouseExit()
	{
		if (gameObject.layer == LayerMask.NameToLayer("EnemyTile") && shipName == null)
			tileSpriteRenderer.color = Color.white;
		else if (gameObject.layer == LayerMask.NameToLayer("EnemyTile") && shipName != null)
			tileSpriteRenderer.color = Color.yellow;
	}

    void OnMouseDown()
	{
		if(terrainControlScript.getGameStatus() == BattleStatus.Deploy)
			return;
        if (gameObject.layer != LayerMask.NameToLayer(terrainControlScript.PlayerSideToTileLayer()) 
		    || terrainControlScript.CheckTileSelectionLimit(name))
        {
			messageControlScript.showWarningMessage("Wrong Side or No other Tiles can be selected!");
            return;
        }
		else
		// Tile selected to hit;

        if (tileSpriteRenderer.sprite == whiteTile)
            tileSpriteRenderer.sprite = crossHair;
        else
            tileSpriteRenderer.sprite = whiteTile;   
	}
    void OnTriggerEnter2D(Collider2D col)
    {
        if ((col.gameObject.tag == "Ship" || col.gameObject.tag=="EnemyShip")&& shipName == null)
        {
            shipName = col.gameObject.name;
			tileSpriteRenderer.color = Color.yellow;
        }
		if (this.gameObject.layer == LayerMask.NameToLayer("EnemyTile") && col.gameObject.tag == "Ship")
		{
			tileSpriteRenderer.color = Color.black;
		}
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if ((col.gameObject.tag == "Ship" || col.gameObject.tag == "EnemyShip") && shipName == col.gameObject.name)
        {
            shipName = null;
            tileSpriteRenderer.color = Color.white;
        }
		if (this.gameObject.layer == LayerMask.NameToLayer("EnemyTile") && col.gameObject.tag == "Ship")
		{
			tileSpriteRenderer.color = Color.white;
		}
    }
    public void DisableTile(bool hit)
    {
        tileSpriteRenderer.sprite = whiteTile;
        if (hit)
        {
            tileSpriteRenderer.color = Color.red;
            tileState = TileStates.ShipHit;
        }
        else
        {
            tileSpriteRenderer.color = Color.gray;
            tileState = TileStates.Disabled;
        }
        GetComponent<Collider2D>().enabled = false;
    }
    public string getShipName()
    {
        return shipName;
    }
    public TileStates getTileState()
    {
        return tileState;
    }
}
