using System.Collections.Generic;
using UnityEngine;

public class Utilitys
{
    public static GridManager currentGrid;
    public static Vector3 GetRandomPos(Vector3 SceneHalfExtents)
    {
        return new Vector3(UnityEngine.Random.value * SceneHalfExtents.x * 2.0f - SceneHalfExtents.x, 0.0f, UnityEngine.Random.value * SceneHalfExtents.z * 2.0f - SceneHalfExtents.z);
    }
    public static Vector2Int GetRandomPosFood()
    {
        return new Vector2Int(UnityEngine.Random.Range(0,currentGrid.Width), UnityEngine.Random.Range(1, currentGrid.Height-1));
    }
    public static Food GetNearest(Transform questioner, List<Food> objects)
    {
        var nearest = objects[0];
        foreach (var item in objects)
        {
            if(Vector3.Distance(item.transform.position,questioner.position)<Vector3.Distance(nearest.transform.position,questioner.position))
                nearest = item;
        }
        return nearest;
    }
    public static float getBestFitness(List<Agent> population)
    {
        float fitness = 0;
        foreach (Agent g in population)
        {
            if (fitness < g.fitness)
                fitness = g.fitness;
        }

        return fitness;
    }
    public static float getAvgFitness(List<Agent> population)
    {
        float fitness = 0;
        foreach (Agent g in population)
        {
            fitness += g.fitness;
        }

        return fitness / population.Count;
    }
    public static float getWorstFitness(List<Agent> population)
    {
        float fitness = 0; // con 0 es mas lindo de ver que float.Max.
        foreach (Agent g in population)
        {
            if (fitness > g.fitness)
                fitness = g.fitness;
        }

        return fitness;
    }
    public static void SetAgentName(GameObject agent, int teamID,int indivCount)
    {
        agent.name = "Agente_";
        if (teamID == 0)
            agent.name += "Blue_";
        else
            agent.name += "Red_";
        agent.name += indivCount.ToString();
    }
}
