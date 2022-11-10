using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour {
    [SerializeField] private int _width, _height;

    [SerializeField] private Tile _grassTile, _mountainTile;

    [SerializeField] private Transform _cam;

    [SerializeField] private Transform _tilesConteiner;

    private Dictionary<Vector2, Tile> _tiles;

    public int Width { get => _width; }
    public int Height { get => _height; }

    public void SetSize(int widht, int height)
    {
        _width = widht;
        _height = height;
    }

    public void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++) {
                //var randomTile = _grassTile;
                var spawnedTile = Instantiate(_grassTile, new Vector3(x, y), Quaternion.identity, _tilesConteiner);
                spawnedTile.name = $"Tile {x} {y}";

              
                spawnedTile.Init(x,y);


                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
    }
    public Tile GetTileAtPosition(Vector2Int pos)
    {
        FixInGrid(ref pos);
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }
    private void FixInGrid(ref Vector2Int pos)
    {
        if (pos.x < 0)
            pos.x = _width-1;
        if (pos.x > _width-1)
            pos.x = 0;
        if (pos.y < 0)
            pos.y = 0;
        if (pos.y > _height-1)
            pos.y = _height-1;
    }
}