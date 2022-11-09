using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner
{
    public GameObject GameObjectPrefab = null;

    List<GameObject> foods = new List<GameObject>();

    public int FoodCuantity = 50;

    public void CreateFoods(Vector3 SceneHalfExtents)
    {
        // Destroy previous created GameObjects
        DestroyFoods();

        for (int i = 0; i < FoodCuantity; i++)
        {
            Vector3 position = Utilitys.GetRandomPos(SceneHalfExtents);
            GameObject go = GameObject.Instantiate(GameObjectPrefab, position, Quaternion.identity);
            foods.Add(go);
        }
    }
    void DestroyFoods()
    {
        foreach (GameObject go in foods)
            GameObject.Destroy(go);
        foods.Clear();
    }
}