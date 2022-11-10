using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject GameObjectPrefab = null;

    List<BaseUnit> foods = new List<BaseUnit>();

    public void CreateFoods(Vector3 SceneHalfExtents,int foodCuantity)
    {
        // Destroy previous created GameObjects
        DestroyFoods();

        for (int i = 0; i < foodCuantity; i++)
        {
            Vector2Int position = Utilitys.GetRandomPosFood();
            GameObject go = GameObject.Instantiate(GameObjectPrefab, Vector3.zero, Quaternion.identity,transform);
            go.name = "Food_"+foods.Count;
            BaseUnit FoodUnit = go.AddComponent<BaseUnit>();
            FoodUnit.UnitName = go.name;
            foods.Add(FoodUnit);

            Tile gridPos = Utilitys.currentGrid.GetTileAtPosition(position);
            gridPos.SetUnit(FoodUnit);

        }
    }
    void DestroyFoods()
    {
        foreach (BaseUnit go in foods)
            GameObject.Destroy(go);
        foods.Clear();
    }
}
