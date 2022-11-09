using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class PopulationManager : MonoBehaviour
{
    private int TeamID = -1;

    [SerializeField] private GameObject AgentPrifab = null;
    [SerializeField] private StartConfigurationTeam teamConfig = null;
    [SerializeField] private SimulationScreen SimulationScreen = null;

    [SerializeField] private int populationCount = 10;

    [SerializeField] private int EliteCount = 4;
    [SerializeField] private float MutationChance = 0.10f;
    [SerializeField] private float MutationRate = 0.01f;
    
    [SerializeField] private int InputsCount = 4;
    [SerializeField] private int HiddenLayers = 1;
    [SerializeField] private int OutputsCount = 2;
    [SerializeField] private int NeuronsCountPerHL = 7;
    [SerializeField] private float Bias = 1f;
    [SerializeField] private float P = 0.5f;

    private int CurrentGeneration = 0;

    GeneticAlgorithm genAlg;

    List<Agent> populationGOs = new List<Agent>();
    List<Genome> population = new List<Genome>();
    List<NeuralNetwork> brains = new List<NeuralNetwork>();

    public int PopulationCount { get => populationCount; }

    private void Start()
    {
        teamConfig.MyStart(
            (x) => { populationCount = (int)x; },
            (x) => { EliteCount = (int)x; },
            (x) => { MutationChance = x/100.0f; },
            (x) => { MutationRate = x/100.0f; },
            (x) => { HiddenLayers = (int)x; },
            (x) => { NeuronsCountPerHL = (int)x; },
            (x) => { Bias = -x; },
            (x) => { P = x; }
            );
        SimulationScreen.MyUpdate(0, 0, 0, 0);
    }

    public void StartSimulation(int TeamID)
    {
        this.TeamID = TeamID;
        SimulationScreen.transform.parent.gameObject.SetActive(true);

        // Create and confiugre the Genetic Algorithm
        genAlg = new GeneticAlgorithm(EliteCount, MutationChance, MutationRate);



        GenerateInitialPopulation(TeamID);
    }

    // Generate the random initial population
    void GenerateInitialPopulation(int TeamID)
    {
        // Destroy previous tanks (if there are any)

        DestroyLists();

        for (int i = 0; i < populationCount; i++)
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
        GameObject go = Instantiate<GameObject>(AgentPrifab, position, Utilitys. GetRandomRot(),transform);
        go.name = "Agente_";
        if (teamID == 0)
            go.name += "Blue_";
        else
            go.name += "Red_";
        go.name += populationGOs.Count.ToString();
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

        SimulationScreen.MyUpdate(
            CurrentGeneration,
            Utilitys.getBestFitness(population),
            Utilitys.getAvgFitness(population),
            Utilitys.getWorstFitness(population)
            );

        // Evolve each genome and create a new array of genomes
        Genome[] newGenomes = genAlg.Epoch(population.ToArray());

        // Clear current population
        population.Clear();

        // Add new population
        population.AddRange(newGenomes);

        // Set the new genomes as each NeuralNetwork weights
        for (int i = 0; i < populationCount; i++)
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
