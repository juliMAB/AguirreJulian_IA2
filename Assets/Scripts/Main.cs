using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{

    #region PRIVATE_FIELDS
    private bool isRunning = false;
    private float currentTimePerTurn;
    #endregion

    #region EXPOSED_FIELDS
    [SerializeField] private StartConfigurationMain mainConfig = null;
    [SerializeField] private SimConfigurationMain simConfig = null;
    [SerializeField] private FoodSpawner foodSpawner = null;
    [SerializeField] private GridManager gridManager = null;

    [SerializeField] private PopulationManager[] populations = null;
    [SerializeField] private Vector3 SceneExtents = new Vector3(20.0f, 0.0f, 20.0f); // aca iria la relacion a la grid.
    [SerializeField] private int IterationCount = 1;
    [SerializeField] private int TimePerTurn = 0;

    [SerializeField] private int sizeGridX = 100;
    [SerializeField] private int sizeGridZ = 100;

    [SerializeField] private int currentTurn = 0;
    [SerializeField] private int MaxTurns = 100;

    #endregion

    public int TeamsCount { get { return 2; } }

    #region UNITY_CALLS

    private void Start()
    {
        if (gridManager == null)
            gridManager = gameObject.AddComponent<GridManager>();
        Utilitys.currentGrid = gridManager;
        if (foodSpawner == null)
            foodSpawner = new FoodSpawner();

        mainConfig.MyStart((x) => { sizeGridX = (int)x; }, (z) => { sizeGridZ = (int)z; }, StartSimulation, (x) => { MaxTurns = (int)x; });
        simConfig.MyStart((x) => { MaxTurns = (int)x; }, (x) => { TimePerTurn = (int)x; }, (x) => { IterationCount = (int)x; });
        gridManager.SetSize(sizeGridX, sizeGridZ);
        SceneExtents = new Vector3(sizeGridX, 0.0f, sizeGridZ); // aca iria la relacion a la grid.
        
    }

    private void FixedUpdate()
    {
        if (!isRunning)
            return;
        float dt = Time.deltaTime;
        simConfig.MyUpdate(currentTurn, MaxTurns);
        for (int w = 0; w < Mathf.Clamp(IterationCount / 100.0f * 50.0f, 1.0f, 50.0f); w++)
        {
            if (currentTimePerTurn < TimePerTurn)
            {
                currentTimePerTurn += dt;
                continue;
            }
            currentTimePerTurn = 0;
            for (int i = 0; i < populations.Length; i++)
            {
                populations[i].FindFoodUpdate(dt, SceneExtents, foodSpawner.foods, i);
                //populations[0].MoveUpdate(populations[1].PopulationGOs); // si tendria otro team lo compararia.
                //populations[1].MoveUpdate(populations[0].PopulationGOs); // si tendria otro team lo compararia.
                populations[i].LastUpdate();
            }
            if (currentTurn > MaxTurns)
            {
                Debug.Log("Epoc");
                //for (int i = 0; i < populations.Length; i++)
                //    populations[i].Epoc();
                currentTurn = 0;
            }
            currentTurn++;
        }
    }
    #endregion
    #region PRIVATE_METHODS
    private void StartSimulation()
    {
        gridManager.GenerateGrid();
        int totalAgents = 0;
        for (int i = 0; i < populations.Length; i++)
        {
            totalAgents += populations[i].PopulationCount;
            populations[i].StartSimulation(i);
        }
        foodSpawner.CreateFoods(SceneExtents, totalAgents);
        isRunning = true;
    }

    #endregion
    #region PUBLIC_METHODS



    public void RelocateGameObject(GameObject food) => food.transform.position = Utilitys.GetRandomPos(SceneExtents);
    public void PauseSimulation() => isRunning = !isRunning;
    public void StopSimulation() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    #endregion
}
