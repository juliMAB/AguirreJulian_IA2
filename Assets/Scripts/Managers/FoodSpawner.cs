using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject GameObjectPrefab = null;

    public List<Food> foods = new List<Food>();

    public void CreateFoods(Vector3 SceneHalfExtents,int foodCuantity)
    {
        // Destroy previous created GameObjects
        DestroyFoods();

        for (int i = 0; i < foodCuantity; i++)
        {
            Vector2Int position = Utilitys.GetRandomPosFood();
            GameObject go = GameObject.Instantiate(GameObjectPrefab, Vector3.zero, Quaternion.identity,transform);
            go.name = "Food_"+foods.Count;
            Food FoodUnit = go.AddComponent<Food>();
            FoodUnit.UnitName = go.name;
            FoodUnit.MyStart((x) => { foods.Remove(x); });
            foods.Add(FoodUnit);
            FoodUnit.RelocateFood();
        }
    }
    void DestroyFoods()
    {
        foreach (BaseUnit go in foods)
            GameObject.Destroy(go.gameObject);
        foods.Clear();
    }
}
