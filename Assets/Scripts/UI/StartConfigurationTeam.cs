using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StartConfigurationTeam : MonoBehaviour
{
    public Text PopulationCountTxt;
    public Slider PopulationCountSlider;

    string PopulationCountString = "Population Count : ";
    public void MyStart(UnityAction<float> updatePopulationCount)
    {
        PopulationCountTxt.text = PopulationCountString + PopulationCountSlider.value.ToString();
        PopulationCountSlider.onValueChanged.AddListener((v) => { PopulationCountTxt.text = PopulationCountString + v.ToString(); });
        PopulationCountSlider.onValueChanged.AddListener(updatePopulationCount);
    }
}
