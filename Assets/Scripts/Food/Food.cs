using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : BaseUnit
{
    int maxIterations = 100;
    int currentIteration = 0;
    private void Start()
    {
        Faction = Faction.Food;
    }
    public void RelocateFood()
    {
        if (currentIteration>maxIterations)
        {
            Debug.LogError("una food se fue del maximo de iteraciones, se destruira.");
            Destroy(gameObject);
            return;
        }
        Vector2Int pos = Utilitys.GetRandomPosFood();
        Tile tile = Utilitys.currentGrid.GetTileAtPosition(pos);
        if (!tile.HasUnit() && !tile.HasFood())
        {
            currentIteration = 0;
            NewTile = tile;
            MoveToNewTile();
        }
        else
        {
            currentIteration++;
            RelocateFood();
        }
    }
}
