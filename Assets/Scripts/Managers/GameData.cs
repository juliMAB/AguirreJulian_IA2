using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int generationNumber;
    public float BestFitness;
    public float AvrgFitness;
    public float WorstFitness;
    public int PopulationCount;
    public int EliteCount;
    public float MutationChance;
    public float MutationRate;
    public int InputsCount;
    public int HiddenLayers;
    public int OutputsCount;
    public int NeuronsCountPerHL;
    public float Bias;
    public float P;
    public List<Genome> genomes;
    public GameData()
    {
        genomes = new List<Genome>();
    }
}
