using System.Collections.Generic;
using UnityEngine;

public class Utilitys
{
    public static Vector3 GetRandomPos(Vector3 SceneHalfExtents)
    {
        return new Vector3(UnityEngine.Random.value * SceneHalfExtents.x * 2.0f - SceneHalfExtents.x, 0.0f, UnityEngine.Random.value * SceneHalfExtents.z * 2.0f - SceneHalfExtents.z);
    }
    public static GameObject GetNearest(Transform questioner, List<GameObject> objects)
    {
        GameObject nearest = objects[0];
        foreach (var item in objects)
        {
            if(Vector3.Distance(item.transform.position,questioner.position)<Vector3.Distance(nearest.transform.position,questioner.position))
                nearest = item;
        }
        return nearest;
    }
    public static Quaternion GetRandomRot()
    {
        return Quaternion.AngleAxis(UnityEngine.Random.value * 360.0f, Vector3.up);
    }
}
