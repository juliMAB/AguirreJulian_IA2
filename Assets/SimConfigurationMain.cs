using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SimConfigurationMain : MonoBehaviour
{
    [SerializeField] private Text MaxTurnsText;
    [SerializeField] private Text CurrentTurnMaxTurnsText;
    [SerializeField] private Slider MaxTurnsSlider;

    [SerializeField] private Text TimePerTurnText;
    [SerializeField] private Slider TimePerTurnSlider;

    [SerializeField] private Text IterationsPerTurnText;
    [SerializeField] private Slider IterationsPerTurnSlider;

    [SerializeField] private Button PauseButton;

    private string MaxTurns = "MaxTurns : ";
    private string TimeTurn = "TimePerTurn : ";
    private string IterationsPerTurn = "IterationsPerTurn : ";

    public void MyStart(UnityAction<float> updateMaxTurns, UnityAction<float> updateTimeTurn, UnityAction<float> updateIterationsPerTurn, UnityAction updatePause)
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

        PauseButton.onClick.AddListener(updatePause);
        
    }
    public void MyUpdate(int CT,int MT)
    {
        CurrentTurnMaxTurnsText.text = CT.ToString() + " / " + MT.ToString();
    }
}
