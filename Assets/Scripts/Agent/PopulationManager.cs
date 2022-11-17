using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using static UnityEngine.ParticleSystem;

public class PopulationManager : MonoBehaviour
{

    #region EXPOSED_FIELDS

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

    #endregion

    #region PRIVATE_FIELDS

    private int teamID = -1;

    private int CurrentGeneration = 0;

    private GeneticAlgorithm genAlg;

    private List<Agent> populationGOs = new List<Agent>();

    private List<Genome> LoadGen = null;

    private FileDataHandler fileDataHandler = null;

    private bool load = false;

    #endregion

    #region PUBLIC_FIELDS
    public int PopulationCount { get => populationCount; }

    #endregion

    #region UNITY_CALLS
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
            (x) => { P = x; },
            ( ) => { LoadData(); }
            );
        SimulationScreen.MyUpdate(0, 0, 0, 0);
        SimulationScreen.MyStart(SaveData);
    }

    #endregion

    #region PUBLIC_METHODS
    public void StartSimulation(int teamID , FileDataHandler fileDataHandler)
    {
        this.fileDataHandler = fileDataHandler;
        this.teamID = teamID;
        SimulationScreen.transform.parent.gameObject.SetActive(true);

        genAlg = new GeneticAlgorithm(EliteCount, MutationChance, MutationRate);

        GenerateInitialPopulation();
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

            for (int i = 0; i < populationGoToReproduce.Count; i++)
            {
                NeuralNetwork brain = populationGoToReproduce[i].brain;

                brain.SetWeights(newGenomes[i].genome);

                Agent t = CreateTank(newGenomes[i], brain);
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
    public void FindFoodUpdate(float dt,List<Food> foods)
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
    public void MoveUpdate(PopulationManager OtherPopulation)
    {
        foreach (Agent t in populationGOs)
        {
            foreach (Agent t2 in OtherPopulation.populationGOs) // comparar con sus enemigos si seder su nueva tile y no moverse.
            {
                if (t.NewTile == t2.NewTile)
                    if (Random.value > t.ThinkFightOrRun())//huir.
                        t.NewTile = t.PreviousTile;
            }
        }
    }
    public void FightUpdate(PopulationManager OtherPopulation)
    {
        for (int i = 0; i < populationGOs.Count; i++)
        {
            for (int w = 0; w < OtherPopulation.populationGOs.Count; w++)
            {
                if(populationGOs[i].NewTile == OtherPopulation.populationGOs[w].NewTile)
                {
                    if (Random.value > 0.5f)
                    {
                        Agent c = populationGOs[i];
                        populationGOs.Remove(c);
                        Destroy(c.gameObject);
                        i--;
                        w = 0;
                    }
                    else
                    {
                        Agent c = OtherPopulation.populationGOs[w];
                        OtherPopulation.populationGOs.Remove(c);
                        Destroy(c.gameObject);
                        w--;
                    }
                }

            }
        }
    }
    public void LastUpdate()
    {
        foreach (Agent t in populationGOs)
        {
            t.AskTileFoodorMove(); // comer la comida o moverse a la newTile.
        }
    }
    
    #endregion

    #region PRIVATE_METHODS
    private void GenerateInitialPopulation()
    {

        foreach (Agent go in populationGOs)
            Destroy(go.gameObject);

        populationGOs.Clear();
        if (load)
        {
            for (int i = 0; i < PopulationCount; i++)
            {

                NeuralNetwork brain = CreateBrain();

                Genome genome = LoadGen[i];

                brain.SetWeights(genome.genome);

                populationGOs.Add(CreateTank(genome, brain));
            }
        }

        for (int i = 0; i < populationCount; i++)
        {
            NeuralNetwork brain = CreateBrain();
            
            Genome genome = new Genome(brain.GetTotalWeightsCount());

            brain.SetWeights(genome.genome);

            populationGOs.Add(CreateTank(genome, brain));
        }
    }
    private Agent CreateTank(Genome genome, NeuralNetwork brain)
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
    private void SetTankPos(Agent t)
    {
        Tile gridPos;
        int totalpopulation = populationGOs.Count();
        int y = 0;
        if (t.teamID == 0)
        {
            y = 0;
            while (totalpopulation > Utilitys.currentGrid.Width)
            {
                y++;
                totalpopulation -= Utilitys.currentGrid.Width;
            }

            Vector2Int resPos = new Vector2Int(Mathf.CeilToInt(populationGOs.Count()), y);
            gridPos = Utilitys.currentGrid.GetTileAtPosition(resPos);
        }
        else
        {
            y = Utilitys.currentGrid.Height;
            while (totalpopulation > Utilitys.currentGrid.Width)
            {
                y--;
                totalpopulation -= Utilitys.currentGrid.Width;
            }
            Vector2Int resPos = new Vector2Int(Utilitys.currentGrid.Width - populationGOs.Count(), y);
            gridPos = Utilitys.currentGrid.GetTileAtPosition(resPos);
        }
        t.NewTile = gridPos;
        t.OccupiedTile = t.NewTile;
        t.MoveToNewTile();
    }
    private NeuralNetwork CreateBrain()
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

    private void LoadData()
    {
        string TeamData = "TeamData" + teamID.ToString();
        GameData data = new GameData();
        data = fileDataHandler.Load(teamID.ToString());
        CurrentGeneration = data.generationNumber;
        populationCount = data.PopulationCount;
        EliteCount = data.EliteCount;
        MutationChance = data.MutationChance;
        MutationRate = data.MutationRate;
        InputsCount = data.InputsCount;
        HiddenLayers = data.HiddenLayers;
        OutputsCount = data.OutputsCount;
        NeuronsCountPerHL = data.NeuronsCountPerHL;
        Bias = data.Bias;
        P = data.P;

        LoadGen = data.genomes;
        load = true;
    }

    private void SaveData()
    {
        GameData data = new GameData();
        data.generationNumber = CurrentGeneration;
        data.PopulationCount = PopulationCount;
        data.EliteCount = EliteCount;
        data.MutationChance = MutationChance;
        data.MutationRate = MutationRate;
        data.InputsCount = InputsCount;
        data.HiddenLayers = HiddenLayers;
        data.OutputsCount = OutputsCount;
        data.NeuronsCountPerHL = NeuronsCountPerHL;
        data.Bias = Bias;
        data.P = P;
        foreach (var item in populationGOs)
            data.genomes.Add(item.genome);

        fileDataHandler.Save(data, teamID.ToString());
    }
    #endregion
}
