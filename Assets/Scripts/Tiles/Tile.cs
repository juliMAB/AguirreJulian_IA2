using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour {

    #region PUBLIC_FIELDS
    public string TileName;

    public List <BaseUnit> OccupiedUnit = null; // cosas en la tile.

    public List<BaseUnit> NewUnits = null; // cosas que va a tener el tile la prixima iteracion.
    #endregion

    #region EXPOSED_FIELD
    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private bool _isWalkable;
    #endregion

    #region UNITY_CALLS
    // no se si hacer esto en cada tile es lo mas optimo, pero como no es lo que se evalua, lo dejaremos asi.
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
    #endregion

    #region PUBLIC_METHODS
    public virtual void Init(int x, int y){ OccupiedUnit.Clear(); NewUnits.Clear(); }
    public void AddUnitOnList(BaseUnit unit)
    {
        OccupiedUnit.Add(unit);
    }
    public void RemoveUnitOnList(BaseUnit unit)
    {
        OccupiedUnit.Remove(unit);
    }
    public void AddUnitOnNewList(BaseUnit unit)
    {
        NewUnits.Add(unit);
    }
    public void RemoveUnitOnNewList(BaseUnit unit)
    {
        NewUnits.Remove(unit);
    }

    public bool HasFood()
    {
        bool hasFood = false;
        for (int i = 0; i < OccupiedUnit.Count; i++)
        {
            if (OccupiedUnit[i] != null)
            {
                if (OccupiedUnit[i].Faction ==Faction.Food)
                {
                    hasFood = true;
                }
            }
        }
        return hasFood;
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

    public void ClearTile()
    {
        OccupiedUnit.Clear();
    }
    #endregion
}