using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StartConfigurationMain : MonoBehaviour
{

    public Text GridSizeXTxt;
    public Text GridSizeYTxt;
    public Slider GridSizeXSlider;
    public Slider GridSizeYSlider;
    public Button StartSimButton;

    public Text CurrentTurnMaxTurnsText;
    public Slider MaxTurnsSlider;

    string MaxTurns = "MaxTurns : ";

    string GridSizeX = "SizeX : ";
    string GridSizeY = "SizeY : ";

    public void MyStart(UnityAction<float>updateGridSizeX, UnityAction<float> updateGridSizeY, UnityAction onStart,UnityAction<float> updateMaxTurn)
    {
        GridSizeXTxt.text = GridSizeX + GridSizeXSlider.value.ToString();
        GridSizeYTxt.text = GridSizeY + GridSizeYSlider.value.ToString();
        CurrentTurnMaxTurnsText.text = MaxTurns + MaxTurnsSlider.value.ToString();
        GridSizeXSlider.onValueChanged.AddListener((v) => {GridSizeXTxt.text = GridSizeX + v.ToString();});
        GridSizeYSlider.onValueChanged.AddListener((v) => {GridSizeYTxt.text = GridSizeY + v.ToString();});
        MaxTurnsSlider.onValueChanged.AddListener((v) => {CurrentTurnMaxTurnsText.text = MaxTurns + v.ToString();});
        GridSizeXSlider.onValueChanged.AddListener(updateGridSizeX);
        GridSizeYSlider.onValueChanged.AddListener(updateGridSizeY);
        MaxTurnsSlider.onValueChanged.AddListener(updateMaxTurn);
        updateGridSizeX?.Invoke(GridSizeXSlider.value);
        updateGridSizeY?.Invoke(GridSizeYSlider.value);
        updateMaxTurn?.Invoke(MaxTurnsSlider.value);
        StartSimButton.onClick.AddListener(onStart);
        StartSimButton.onClick.AddListener(() => { gameObject.transform.parent.gameObject.SetActive(false); });
    }
}
