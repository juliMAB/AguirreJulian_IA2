using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StartConfigurationTeam : MonoBehaviour
{
    public Text PopulationCountTxt;
    public Slider PopulationCountSlider;
    string PopulationCountString = "Population Count : ";

    public Text EliteCountTxt;
    public Slider EliteCountSlider;
    string EliteCountString = "Elite Count : ";

    public Text MutationChanceTxt;
    public Slider MutationChanceSlider;
    string MutationChanceString = "Mutation Chance : ";

    public Text MutationRateTxt;
    public Slider MutationRateSlider;
    string MutationRateString = "Mutation Rate : ";

    public Text HiddenLayersCountTxt;
    public Slider HiddenLayersCountSlider;
    string HiddenLayersCountString = "HiddenLayers Count : ";

    public Text NeuronsPerHiddenLayerTxt;
    public Slider NeuronsPerHiddenLayerSlider;
    string NeuronsPerHiddenLayerString = "NeuronsPerHiddenLayer : ";

    public Text BiasTxt;
    public Slider BiasSlider;
    string BiasString = "Bias : ";

    public Text SigmoidSlopeTxt;
    public Slider SigmoidSlopeSlider;
    string SigmoidSlopeString = "SigmoidSlope : ";

    public Button LoadButton;

    public void MyStart(
        UnityAction<float> updatePopulationCount,
        UnityAction<float> updateEliteCount,
        UnityAction<float> updateMutationChance,
        UnityAction<float> updateMutationRate,
        UnityAction<float> updateHiddenLayersCount,
        UnityAction<float> updateNeuronsPerHiddenLayer,
        UnityAction<float> updateBias,
        UnityAction<float> updateSigmoidSlope,
        UnityAction        updateLoad
        )
    {
        PopulationCountTxt.text = PopulationCountString + PopulationCountSlider.value.ToString();
        PopulationCountSlider.onValueChanged.AddListener((v) => { PopulationCountTxt.text = PopulationCountString + v.ToString(); });
        PopulationCountSlider.onValueChanged.AddListener(updatePopulationCount);
        updatePopulationCount?.Invoke(PopulationCountSlider.value);

        EliteCountTxt.text = EliteCountString + EliteCountSlider.value.ToString();
        EliteCountSlider.onValueChanged.AddListener((v) => { EliteCountTxt.text = EliteCountString + v.ToString(); });
        EliteCountSlider.onValueChanged.AddListener(updateEliteCount);
        updateEliteCount?.Invoke(EliteCountSlider.value);

        MutationChanceTxt.text = MutationChanceString + MutationChanceSlider.value.ToString();
        MutationChanceSlider.onValueChanged.AddListener((v) => { MutationChanceTxt.text = MutationChanceString + v.ToString(); });
        MutationChanceSlider.onValueChanged.AddListener(updateMutationChance);
        updateMutationChance?.Invoke(MutationChanceSlider.value);

        MutationRateTxt.text = MutationRateString + MutationRateSlider.value.ToString();
        MutationRateSlider.onValueChanged.AddListener((v) => { MutationRateTxt.text = MutationRateString + v.ToString(); });
        MutationRateSlider.onValueChanged.AddListener(updateMutationRate);
        updateMutationRate?.Invoke(MutationRateSlider.value);

        HiddenLayersCountTxt.text = HiddenLayersCountString + HiddenLayersCountSlider.value.ToString();
        HiddenLayersCountSlider.onValueChanged.AddListener((v) => { HiddenLayersCountTxt.text = HiddenLayersCountString + v.ToString(); });
        HiddenLayersCountSlider.onValueChanged.AddListener(updateHiddenLayersCount);
        updateHiddenLayersCount?.Invoke(HiddenLayersCountSlider.value);

        NeuronsPerHiddenLayerTxt.text = NeuronsPerHiddenLayerString + NeuronsPerHiddenLayerSlider.value.ToString();
        NeuronsPerHiddenLayerSlider.onValueChanged.AddListener((v) => { NeuronsPerHiddenLayerTxt.text = NeuronsPerHiddenLayerString + v.ToString(); });
        NeuronsPerHiddenLayerSlider.onValueChanged.AddListener(updateNeuronsPerHiddenLayer);
        updateNeuronsPerHiddenLayer?.Invoke(NeuronsPerHiddenLayerSlider.value);

        BiasTxt.text = BiasString + "-" + BiasSlider.value.ToString();
        BiasSlider.onValueChanged.AddListener((v) => { BiasTxt.text = BiasString + "-" + v.ToString(); });
        BiasSlider.onValueChanged.AddListener(updateBias);
        updateBias?.Invoke(BiasSlider.value);

        SigmoidSlopeTxt.text = SigmoidSlopeString + SigmoidSlopeSlider.value.ToString();
        SigmoidSlopeSlider.onValueChanged.AddListener((v) => { SigmoidSlopeTxt.text = SigmoidSlopeString + v.ToString(); });
        SigmoidSlopeSlider.onValueChanged.AddListener(updateSigmoidSlope);
        updateSigmoidSlope?.Invoke(SigmoidSlopeSlider.value);

        LoadButton.onClick.AddListener(updateLoad);
    }
}
