using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SimConfigurationMain : MonoBehaviour
{
    public Text MaxTurnsText;
    public Text CurrentTurnMaxTurnsText;
    public Slider MaxTurnsSlider;

    public Text TimePerTurnText;
    public Slider TimePerTurnSlider;

    public Text IterationsPerTurnText;
    public Slider IterationsPerTurnSlider;

    string MaxTurns = "MaxTurns : ";

    string TimeTurn = "TimePerTurn : ";

    string IterationsPerTurn = "IterationsPerTurn : ";

    public void MyStart(UnityAction<float> updateMaxTurns, UnityAction<float> updateTimeTurn, UnityAction<float> updateIterationsPerTurn)
    {
        MaxTurnsText.text = MaxTurns + MaxTurnsSlider.value.ToString();

        MaxTurnsSlider.onValueChanged.AddListener((v) => { MaxTurnsText.text = this.MaxTurns + v.ToString(); });
        MaxTurnsSlider.onValueChanged.AddListener(updateMaxTurns);

        TimePerTurnSlider.onValueChanged.AddListener((v) => { TimePerTurnText.text = TimeTurn + v.ToString(); });
        TimePerTurnSlider.onValueChanged.AddListener(updateTimeTurn);

        IterationsPerTurnSlider.onValueChanged.AddListener((v) => { IterationsPerTurnText.text = IterationsPerTurn + v.ToString(); });
        IterationsPerTurnSlider.onValueChanged.AddListener(updateIterationsPerTurn);

        updateIterationsPerTurn?.Invoke(IterationsPerTurnSlider.value);

        updateTimeTurn?.Invoke(TimePerTurnSlider.value);

        updateMaxTurns?.Invoke(MaxTurnsSlider.value);
        
    }
    public void MyUpdate(int CT,int MT)
    {
        CurrentTurnMaxTurnsText.text = CT.ToString() + " / " + MT.ToString();
    }
}
