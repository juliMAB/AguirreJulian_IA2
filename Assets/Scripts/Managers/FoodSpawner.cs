using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject GameObjectPrefab = null;

    List<GameObject> foods = new List<GameObject>();

    public void CreateFoods(Vector3 SceneHalfExtents,int foodCuantity)
    {
        // Destroy previous created GameObjects
        DestroyFoods();

        for (int i = 0; i < foodCuantity; i++)
        {
            Vector3 position = Utilitys.GetRandomPos(SceneHalfExtents);
            GameObject go = GameObject.Instantiate(GameObjectPrefab, position, Quaternion.identity,transform);
            go.name = "Food_"+foods.Count;
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
