using UnityEngine;

public class BaseUnit : MonoBehaviour {
    public string UnitName;
    public Tile OccupiedTile;
    public Tile NewTile;
    public Faction Faction;

    public void MoveToNewTile()
    {
        OccupiedTile?.RemoveUnitOnList(this);
        OccupiedTile = NewTile;
        NewTile = null;
        if(OccupiedTile!=null)
            transform.position = OccupiedTile.transform.position;
        OccupiedTile?.AddUnitOnList(this);
    }
}
