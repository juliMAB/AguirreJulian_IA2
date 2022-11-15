using UnityEngine;

public class Food : BaseUnit
{
    #region PRIVATE_FIELD
    private int maxIterations = 100;
    private int currentIteration = 0;
    private System.Action<Food> onDestroyd;
    #endregion

    #region UNITY_CALLS
    private void Start()
    {
        Faction = Faction.Food;
    }
    private void OnDestroy()
    {
        onDestroyd?.Invoke(this);
    }
    #endregion

    #region PUBLIC_METHODS
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
    #endregion

}
