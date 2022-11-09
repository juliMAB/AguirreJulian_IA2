using UnityEngine;
using UnityEngine.UI;

public class SimulationScreen : MonoBehaviour
{
    [SerializeField] private Text GenerationTxt;
    [SerializeField] private Text BestGenTxt;
    [SerializeField] private Text AverageGenTxt;
    [SerializeField] private Text WorstGenTxt;

    public void MyUpdate(int generation, float BestGen,float AverageGen,float WorstGen)
    {
        GenerationTxt.text = "Generation : " + generation.ToString();
        BestGenTxt.text = "BestGen : " + BestGen.ToString();
        AverageGenTxt.text = "AverageGen : " + AverageGen.ToString();
        WorstGenTxt.text = "WorstGen : " + WorstGen.ToString();
    }
}
