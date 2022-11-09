using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{

    #region PRIVATE_FIELDS
    private bool isRunning = false;
    #endregion

    #region EXPOSED_FIELDS
    [SerializeField] private StartConfigurationMain mainConfig = null;
    [SerializeField] private FoodSpawner foodSpawner = null;
    [SerializeField] private GridManager gridManager = null;
    [SerializeField] private Vector3 SceneHalfExtents = new Vector3(20.0f, 0.0f, 20.0f); // aca iria la relacion a la grid.
    [SerializeField] private int IterationCount = 1;

    [SerializeField] private int sizeGridX = 100;
    [SerializeField] private int sizeGridZ = 100;
    #endregion

    public int TeamsCount { get { return 2; } }

    #region SINGLETON
    static Main instance = null;

    public static Main Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<Main>();

            return instance;
        }
    }

    void Awake()
    {
        instance = this;
    }
    #endregion


    #region UNITY_CALLS

    private void Start()
    {
        if(gridManager==null)
            gridManager = gameObject.AddComponent<GridManager>();

        mainConfig.MyStart((x) => { sizeGridX = (int)x; }, (z) => { sizeGridZ = (int)z; }, StartSimulation);
        gridManager.SetSize(sizeGridX, sizeGridZ);
    }

    private void FixedUpdate()
    {
        if (!isRunning)
            return;
    }
    #endregion
    #region PRIVATE_METHODS
    private void StartSimulation()
    {
        gridManager.GenerateGrid();

        isRunning = true;
    }

    #endregion
    #region PUBLIC_METHODS



    public void RelocateGameObject(GameObject food) => food.transform.position = Utilitys.GetRandomPos(SceneHalfExtents);
    public void PauseSimulation() => isRunning = !isRunning;
    public void StopSimulation() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    #endregion
}
