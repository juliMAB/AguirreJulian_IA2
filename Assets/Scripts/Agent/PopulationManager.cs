using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;
using UnityEditor;
using static UnityEditor.Progress;

public class PopulationManager : MonoBehaviour
{
    private int TeamID = -1;

    [SerializeField] private GameObject AgentPrifab = null;
    [SerializeField] private GameObject TeamConteiner = null;
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

    public int PopulationCount { get => populationCount; }

    public List<Agent> PopulationGOs { get => populationGOs; }

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

        genAlg = new GeneticAlgorithm(EliteCount, MutationChance, MutationRate);

        GenerateInitialPopulation(TeamID);
    }

    // Generate the random initial population
    void GenerateInitialPopulation(int TeamID)
    {

        foreach (Agent go in populationGOs)
            Destroy(go.gameObject);

        populationGOs.Clear();
        //populationGenomes.Clear();

        for (int i = 0; i < populationCount; i++)
        {
            NeuralNetwork brain = CreateBrain();
            
            Genome genome = new Genome(brain.GetTotalWeightsCount());

            brain.SetWeights(genome.genome);

            //populationGenomes.Add(genome);

            populationGOs.Add(CreateTank(genome, brain,TeamID));
        }
    }

    Agent CreateTank(Genome genome, NeuralNetwork brain, int teamID)
    {
        GameObject go = Instantiate<GameObject>(AgentPrifab, Vector3.zero, Quaternion.identity, TeamConteiner.transform);
        Utilitys.SetAgentName(go, teamID, populationGOs.Count);
        Agent t = go.GetComponent<Agent>();
        t.UnitName = go.name;
        t.SetTeamID(teamID);
        SetTankPos(t);
        t.SetBrain(genome, brain);
        return t;
    }
    void SetTankPos(Agent t)
    {
        Tile gridPos;
        if (t.teamID == 0)
        {
            Vector2Int resPos = new Vector2Int(Mathf.CeilToInt(populationGOs.Count()), 0);
            gridPos = Utilitys.currentGrid.GetTileAtPosition(resPos);
        }
        else
        {
            Vector2Int resPos = new Vector2Int(Utilitys.currentGrid.Width - populationGOs.Count(), Utilitys.currentGrid.Height);
            gridPos = Utilitys.currentGrid.GetTileAtPosition(resPos);
        }
        t.NewTile = gridPos;
        t.MoveToNewTile();
    }

    void DestroyLists()
    {
        foreach (Agent go in populationGOs)
            Destroy(go.gameObject);

        populationGOs.Clear();
        //populationGenomes.Clear();
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
    {
        //primero se actualiza el UI.
        CurrentGeneration++;
        SimulationScreen.MyUpdate(
            CurrentGeneration,
            Utilitys.getBestFitness(populationGOs),
            Utilitys.getAvgFitness(populationGOs),
            Utilitys.getWorstFitness(populationGOs)
            );
        //se crean las listas de las posibilidades.
        List<Agent> populationGOToSave = new List<Agent>();
        List<Agent> populationGoToReproduce = new List<Agent>();
        List<Genome> populationGenomeNew = new List<Genome>();

        //se cargan esas listas.
        for (int i = 0; i < populationGOs.Count; i++)
        {
            if (populationGOs[i].generationCount>2 || populationGOs[i].eatFood <= 0) // matar los que no comen y los adultos.
            {
                Agent c = populationGOs[i];
                populationGOs.Remove(c);
                Destroy(c.gameObject);
                i--;
                continue;
            }
            if (populationGOs[i].eatFood > 1) //reproducir a todos los que comieron mas de 2.
                populationGoToReproduce.Add(populationGOs[i]);

            if (populationGOs[i].eatFood > 0 && populationGOs[i].generationCount <= 2) // salvar a todos los que comieron mas de 1.
            {
                populationGOs[i].generationCount++;
                populationGOToSave.Add(populationGOs[i]);
            }
        }

        populationGOs.Clear();

        //inicio proceso de reproduccion.
        if (populationGoToReproduce.Count>=2)
        {
            foreach (var item in populationGoToReproduce)
                populationGenomeNew.Add(item.genome);
        
            Genome[] newGenomes = genAlg.Epoch(populationGenomeNew.ToArray());

            //List<Agent> newPopulationGO = new List<Agent>();
            //
            //newPopulationGO.AddRange(populationGOs);

            for (int i = 0; i < populationGoToReproduce.Count; i++)
            {
                NeuralNetwork brain = populationGoToReproduce[i].brain;

                brain.SetWeights(newGenomes[i].genome);

                Agent t = CreateTank(newGenomes[i], brain, TeamID);
                t.SetBrain(newGenomes[i], brain);
                populationGOs.Add(t); // se agregarn los nuevos indivuduos.
            }
        }
        //fin inicio de reproduccion.

        //salvar a los que comieron mas de 1.
        for (int i = 0; i < populationGOToSave.Count; i++)
        {
            SetTankPos(populationGOToSave[i]);
            populationGOs.Add(populationGOToSave[i]); // se agregan los viejos individos que vivieron.
        }
        foreach (var item in populationGOs)
            item.OnReset();
        SimulationScreen.UpdateActualPopulation(populationGOs.Count);
    }

    public void FindFoodUpdate(float dt,Vector3 SceneExtents,List<Food> foods,int teamID)
	{
        foreach (Agent t in populationGOs)
        {
            // Get the nearest mine
            Food food = Utilitys.GetNearest(t.transform, foods);

            // Set the nearest mine to current tank
            t.SetNearestFood(food);

            // Think!! 

            t.Think(dt); // piensa cual va a ser la posicion siguiente a la que se va a mover.
        }
    }
    public void MoveUpdate(List<Agent> OtherPopulation)
    {
        foreach (Agent t in populationGOs)
        {
            foreach (Agent t2 in OtherPopulation) // comparar con sus enemigos si seder su nueva tile y no moverse.
            {
                if (t.NewTile == t2.NewTile)
                    if (Random.value > t.ThinkFightOrRun())//huir.
                        t.NewTile = t.PreviousTile;
            }
        }
    }

    public void FightUpdate(List<Agent> OtherPopulation)
    {
        List<Agent> agentsToKill1 = new List<Agent>();
        List<Agent> agentsToKill2 = new List<Agent>();
        foreach (Agent t in populationGOs)
        {
            foreach (Agent t2 in OtherPopulation) // comparar con sus enemigos si secedieron.
            {
                if (t.NewTile == t2.NewTile)//sino se mata a uno de los 2.
                    if (Random.value > 0.5f)
                        agentsToKill2.Add(t2);
                    else
                        agentsToKill1.Add(t);
            }
        }
       // deleteDiesAgents(agentsToKill1, populationGOs);
       // deleteDiesAgents(agentsToKill2, OtherPopulation);
    }
    private void deleteDiesAgents(List<Agent> agentsToKill, List<Agent> mainList)
    {
        foreach (Agent item in agentsToKill)
        {
            mainList.Remove(item);
            Destroy(item);
        }
    }
    public void LastUpdate()
    {
        foreach (Agent t in populationGOs)
        {
            t.AskTileFoodorMove(); // comer la comida o moverse a la newTile.
        }
    }
}
