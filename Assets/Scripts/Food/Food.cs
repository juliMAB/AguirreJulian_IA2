using UnityEngine;

public class Food : BaseUnit
{
    int maxIterations = 100;
    int currentIteration = 0;
    System.Action<Food> onDestroyd;
    private void Start()
    {
        Faction = Faction.Food;
    }
    public void MyStart(System.Action<Food> onDestroyd)
    {
        this.onDestroyd = onDestroyd;
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
    private void OnDestroy()
    {
        onDestroyd?.Invoke(this);
    }

}
