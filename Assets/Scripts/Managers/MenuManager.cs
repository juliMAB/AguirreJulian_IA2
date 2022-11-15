using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
    public static MenuManager Instance;

    [SerializeField] private GameObject _selectedHeroObject,_tileObject,_tileUnitObject;


    void Awake() {
        Instance = this;
    }

    public void ShowTileInfo(Tile tile) {

        if (tile == null)
        {
            _tileObject.SetActive(false);
            _tileUnitObject.SetActive(false);
            return;
        }

        _tileObject.GetComponentInChildren<Text>().text = tile.TileName;
        _tileObject.SetActive(true);

        if (tile.OccupiedUnit != null && tile.OccupiedUnit.Count > 0) 
        {
            _tileUnitObject.GetComponentInChildren<Text>().text = "";
            for (int i = 0; i < tile.OccupiedUnit.Count; i++)
                if (tile.OccupiedUnit[i]!=null)
                    _tileUnitObject.GetComponentInChildren<Text>().text += tile.OccupiedUnit[i].UnitName + "\n";
            _tileUnitObject.SetActive(true);
        }
    }
}
