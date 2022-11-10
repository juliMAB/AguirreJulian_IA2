using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SimConfigurationMain : MonoBehaviour
{
    public Text MaxTurnsText;
    public Text CurrentTurnMaxTurnsText;
    public Slider MaxTurnsSlider;

    string MaxTurns = "MaxTurns : ";

    public void MyStart(UnityAction<float> updateMaxTurns)
    {
        MaxTurnsText.text = MaxTurns + MaxTurnsSlider.value.ToString();

        MaxTurnsSlider.onValueChanged.AddListener((v) => { MaxTurnsText.text = this.MaxTurns + v.ToString(); });
        MaxTurnsSlider.onValueChanged.AddListener(updateMaxTurns);
        
        updateMaxTurns?.Invoke(MaxTurnsSlider.value);
        
    }
    public void MyUpdate(int CT,int MT)
    {
        CurrentTurnMaxTurnsText.text = CT.ToString() + " / " + MT.ToString();
    }
}
