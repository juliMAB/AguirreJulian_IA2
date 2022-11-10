using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Tile : MonoBehaviour {
    public string TileName;
    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private bool _isWalkable;

    public List <BaseUnit> OccupiedUnit = new List<BaseUnit>(); // el caso maximo de posibilidades de cosas en la tile.


    public virtual void Init(int x, int y)
    {
        
    }

    void OnMouseEnter()
    {
        _highlight.SetActive(true);
        MenuManager.Instance.ShowTileInfo(this);
    }

    void OnMouseExit()
    {
        _highlight.SetActive(false);
        MenuManager.Instance.ShowTileInfo(null);
    }

    public void SetUnit(BaseUnit unit) {
        if (unit.OccupiedTile)
        if (unit.OccupiedTile.OccupiedUnit!=null)
        if (unit.OccupiedTile.OccupiedUnit.Count>0)
        if (unit.OccupiedTile.OccupiedUnit.Any((x)=>x==unit))
            unit.OccupiedTile.OccupiedUnit.Remove(unit);
        unit.transform.position = transform.position;
        AddUnitOnList(unit);
        unit.OccupiedTile = this;
    }
    public void AddUnitOnList(BaseUnit unit)
    {
        OccupiedUnit.Add(unit);
    }
    public void RemoveUnitOnList(BaseUnit unit)
    {
        OccupiedUnit.Remove(unit);
    }
    public bool HasFood()
    {
        for (int i = 0; i < OccupiedUnit.Count; i++)
        {
            if (OccupiedUnit[i] != null)
            {
                if (OccupiedUnit[i].Faction ==Faction.Food)
                    return true;
            }
        }
        return false;
    }
    public bool HasUnit()
    {
        for (int i = 0; i < OccupiedUnit.Count; i++)
        {
            if (OccupiedUnit[i] != null)
            {
                if (OccupiedUnit[i].Faction != Faction.Food)
                    return true;
            }
        }
        return false;
    }
}