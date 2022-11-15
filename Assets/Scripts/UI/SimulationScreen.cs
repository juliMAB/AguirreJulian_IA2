using UnityEngine;
using UnityEngine.UI;

public class SimulationScreen : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private Text GenerationTxt;
    [SerializeField] private Text BestGenTxt;
    [SerializeField] private Text AverageGenTxt;
    [SerializeField] private Text WorstGenTxt;
    [SerializeField] private Text ActualPopulationTxt;
    #endregion

    #region PUBLIC_FIELDS
    public void MyUpdate(int generation, float BestGen,float AverageGen,float WorstGen)
    {
        GenerationTxt.text = "Generation : " + generation.ToString();
        BestGenTxt.text = "BestGen : " + BestGen.ToString();
        AverageGenTxt.text = "AverageGen : " + AverageGen.ToString();
        WorstGenTxt.text = "WorstGen : " + WorstGen.ToString();
        
    }
    public void UpdateActualPopulation(int ActualPopulation)
    {
        ActualPopulationTxt.text = "Actual Population : " + ActualPopulation.ToString();
    }
    #endregion
}
