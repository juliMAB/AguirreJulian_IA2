using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class PopulationManager : MonoBehaviour
{
    private int TeamID = -1;

    [SerializeField] private GameObject AgentPrifab = null;
    [SerializeField] private StartConfigurationTeam teamConfig = null;

    public int PopulationCount = 10;

    private int EliteCount = 4;
    private float MutationChance = 0.10f;
    private float MutationRate = 0.01f;
    
    private int InputsCount = 4;
    private int HiddenLayers = 1;
    private int OutputsCount = 2;
    private int NeuronsCountPerHL = 7;
    private float Bias = 1f;
    private float P = 0.5f;

    private int CurrentGeneration = 0;

    GeneticAlgorithm genAlg;

    List<Agent> populationGOs = new List<Agent>();
    List<Genome> population = new List<Genome>();
    List<Genome> savePopulation = new List<Genome>();
    List<NeuralNetwork> brains = new List<NeuralNetwork>();


    private void Start()
    {
        teamConfig.MyStart((x) => { PopulationCount = (int)x; });
    }


    

    public float bestFitness 
    {
        get; private set;
    }

    public float avgFitness 
    {
        get; private set;
    }

    public float worstFitness 
    {
        get; private set;
    }

    private float getBestFitness()
    {
        float fitness = 0;
        foreach(Genome g in population)
        {
            if (fitness < g.fitness)
                fitness = g.fitness;
        }

        return fitness;
    }

    private float getAvgFitness()
    {
        float fitness = 0;
        foreach(Genome g in population)
        {
            fitness += g.fitness;
        }

        return fitness / population.Count;
    }

    private float getWorstFitness()
    {
        float fitness = float.MaxValue;
        foreach(Genome g in population)
        {
            if (fitness > g.fitness)
                fitness = g.fitness;
        }

        return fitness;
    }

    public void StartSimulation()
    {
        // Create and confiugre the Genetic Algorithm
        genAlg = new GeneticAlgorithm(EliteCount, MutationChance, MutationRate);


        
        GenerateInitialPopulation();
    }

    // Generate the random initial population
    void GenerateInitialPopulation()
    {
        // Destroy previous tanks (if there are any)

        DestroyLists();

        for (int i = 0; i < PopulationCount; i++)
        {
            NeuralNetwork brain = CreateBrain();
            
            Genome genome = new Genome(brain.GetTotalWeightsCount());

            brain.SetWeights(genome.genome);
            brains.Add(brain);

            population.Add(genome);

            populationGOs.Add(CreateTank(genome, brain,TeamID));
        }
    }

    Agent CreateTank(Genome genome, NeuralNetwork brain, int teamID)
    {
        Vector3 position = Utilitys.GetRandomPos(Vector3.zero);
        GameObject go = Instantiate<GameObject>(AgentPrifab, position, Utilitys. GetRandomRot());
        Agent t = go.GetComponent<Agent>();
        t.SetTeamID(teamID);
        t.SetBrain(genome, brain);
        return t;
    }

    void DestroyLists()
    {
        foreach (Agent go in populationGOs)
            Destroy(go.gameObject);

        populationGOs.Clear();
        population.Clear();
        brains.Clear();
    }

    // Creates a new NeuralNetwork
    NeuralNetwork CreateBrain()
    {
        NeuralNetwork brain = new NeuralNetwork();

        // Add first neuron layer that has as many neurons as inputs
        brain.AddFirstNeuronLayer(InputsCount, Bias, P);

        for (int i = 0; i < HiddenLayers; i++)
        {
            // Add each hidden layer with custom neurons count
            brain.AddNeuronLayer(NeuronsCountPerHL, Bias, P);
        }

        // Add the output layer with as many neurons as outputs
        brain.AddNeuronLayer(OutputsCount, Bias, P);

        return brain;
    }

    // Evolve!!!
    public void Epoc()
    //void Epoch()
    {
        // Increment generation counter
        CurrentGeneration++;

        // Calculate best, average and worst fitness
        bestFitness = getBestFitness();
        avgFitness = getAvgFitness();
        worstFitness = getWorstFitness();

        // Evolve each genome and create a new array of genomes
        Genome[] newGenomes = genAlg.Epoch(population.ToArray());

        // Clear current population
        population.Clear();

        // Add new population
        population.AddRange(newGenomes);

        // Set the new genomes as each NeuralNetwork weights
        for (int i = 0; i < PopulationCount; i++)
        {
            NeuralNetwork brain = brains[i];

            brain.SetWeights(newGenomes[i].genome);

            populationGOs[i].SetBrain(newGenomes[i], brain);
            populationGOs[i].transform.position = Utilitys.GetRandomPos(Vector3.zero);
            populationGOs[i].transform.rotation = Utilitys.GetRandomRot();
        }
    }

    public void LocalFixelUpdate(float dt,Vector3 SceneHalfExtents,List<GameObject> mines,int teamID)
	{
        foreach (Agent t in populationGOs)
        {
            // Get the nearest mine
            GameObject mine = Utilitys.GetNearest(t.transform, mines);

            // Set the nearest mine to current tank
            t.SetNearestMine(mine);

            // Think!! 

            t.Think(dt);

            // Just adjust tank position when reaching world extents
            Vector3 pos = t.transform.position;
            if (pos.x > SceneHalfExtents.x)
                pos.x -= SceneHalfExtents.x * 2;
            else if (pos.x < -SceneHalfExtents.x)
                pos.x += SceneHalfExtents.x * 2;

            if (pos.z > SceneHalfExtents.z)
                pos.z -= SceneHalfExtents.z * 2;
            else if (pos.z < -SceneHalfExtents.z)
                pos.z += SceneHalfExtents.z * 2;

            // Set tank position
            t.transform.position = pos;
        }
    }
}
