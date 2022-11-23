using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{

    #region PRIVATE_FIELDS
    private bool isRunning = false;
    private float currentTimePerIteration;
    private int initialFoodCuantity;
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
        //gridManager.SetSize(sizeGridX, sizeGridZ);
        SceneExtents = new Vector3(sizeGridX, 0.0f, sizeGridZ); // aca iria la relacion a la grid.
        
    }

    private void FixedUpdate()
    {
        if (!isRunning)
            return;
        float dt = Time.deltaTime;
        
        if (currentTimePerIteration < TimePerTurn)
        {
            currentTimePerIteration += dt;
            return;
        }
        currentTimePerIteration = 0;
        simConfig.MyUpdate(currentTurn, MaxTurns);
        for (int w = 0; w < IterationCount; w++)
        {

            for (int i = 0; i < populations.Length; i++)
                if (foodSpawner.foods.Count > 0)
                    populations[i].FindFood_OnThink(foodSpawner.foods);

            
            populations[0].MoveUpdate(populations[1]); // preguntar si el siguiente es el siguiente de otro-> ceder posicion o quedarse.

            for (int i = 0; i < populations.Length; i++)
                populations[i].LastUpdate();

            if (currentTurn > MaxTurns)
            {
                Debug.Log("Epoc");
                gridManager.ClearLists();
                for (int i = 0; i < populations.Length; i++)
                    populations[i].Epoc();
                foodSpawner.CreateFoods(SceneExtents, initialFoodCuantity);
                currentTurn = 0;
            }
            currentTurn++;
        }
    }
    #endregion

    #region PRIVATE_METHODS
    private void StartSimulation()
    {
        gridManager.SetSize(sizeGridX, sizeGridZ);
        gridManager.GenerateGrid();
        int totalAgents = 0;
        for (int i = 0; i < populations.Length; i++)
        {
            totalAgents += populations[i].PopulationCount;
            populations[i].StartSimulation(i);
        }
        initialFoodCuantity = totalAgents;
        foodSpawner.CreateFoods(SceneExtents, initialFoodCuantity);
        isRunning = true;
    }

    #endregion

    #region PUBLIC_METHODS



    public void RelocateGameObject(GameObject food) => food.transform.position = Utilitys.GetRandomPos(SceneExtents);
    public void PauseSimulation() => isRunning = !isRunning;
    public void StopSimulation() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    #endregion
}
