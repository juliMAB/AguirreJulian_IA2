using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : MonoBehaviour {
    public string UnitName;
    public Tile OccupiedTile;
    public Tile NewTile;
    public Tile PreviousTile;
    public Faction Faction;

    public void MoveToNewTile()
    {
        OccupiedTile?.RemoveUnitOnList(this);
        PreviousTile = OccupiedTile;
        OccupiedTile = NewTile;
        NewTile = null;
        transform.position = OccupiedTile.transform.position;
        OccupiedTile?.AddUnitOnList(this);
    }
}
