using UnityEngine;

public class Utilitys
{
    public static Vector3 GetRandomPos(Vector3 SceneHalfExtents)
    {
        return new Vector3(UnityEngine.Random.value * SceneHalfExtents.x * 2.0f - SceneHalfExtents.x, 0.0f, UnityEngine.Random.value * SceneHalfExtents.z * 2.0f - SceneHalfExtents.z);
    }
}
