using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Utilities //: MonoBehaviour 
{
	/* Transforms position from screen space into world space.
	 * Screenspace is defined in pixels. The bottom-left of the screen is (0,0);
	 * the right-top is (pixelWidth,pixelHeight). The z position is in world units from the camera
	 */
    public static Vector3 ScreenToWorld(float x,float y,float z)
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(x, y, z));
    }

    public static Vector3 ScreenToWorld(Vector3 matrix)
    {
        return Camera.main.ScreenToWorldPoint(matrix);
    }
	/* Transforms position from world space into screen space.
	 * Screenspace is defined in pixels. The bottom-left of the screen is (0,0);
	 * the right-top is (pixelWidth,pixelHeight). The z position is in world units from the camera
	 */
    public static Vector3 WorldToScreen(float x,float y,float z)
    {
        return Camera.main.WorldToScreenPoint(new Vector3(x, y, z));
    }

    public static Vector3 WorldToScreen(Vector3 matrix)
    {
        return Camera.main.WorldToScreenPoint(matrix);
    }
    /*
     * IntToTileName method convert a column (int col) and a row (int row) to a string(Name of the Tile) 
     */
    public static string IntToTileName(int row, int col,bool isEnemyTile)
    {
        if(isEnemyTile)
            return "ERow" + row + "ECol" + col;
        else
            return "Row" + row + "Col" + col;
    }

    public static string ChangeTileCol(string tile,int amount)
    {
        return "Row" + tile[3] + "Col" + Convert.ToChar((Convert.ToInt32(tile[7]) + amount));
    }

    public static string ChangeTileRow(string tile, int amount)
    {
        return "Row" + Convert.ToChar((Convert.ToInt32(tile[3]) + amount)) + "Col" + tile[7];
    }

	public static void RemoveListItems<T>(List<T> firstList, List<T> secList)
	{
		foreach(T item in secList)
		{
			if(firstList.Remove(item))
				Debug.Log(item + " removed");
			else
				Debug.Log(item + " False");
		}
	}
}