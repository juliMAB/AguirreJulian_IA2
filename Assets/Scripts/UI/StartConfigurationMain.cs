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

    string GridSizeX = "SizeX : ";
    string GridSizeY = "SizeY : ";

    public void MyStart(UnityAction<float>updateGridSizeX, UnityAction<float> updateGridSizeY, UnityAction onStart)
    {
        GridSizeXTxt.text = GridSizeX + GridSizeXSlider.value.ToString();
        GridSizeYTxt.text = GridSizeY + GridSizeYSlider.value.ToString();
        GridSizeXSlider.onValueChanged.AddListener((v) => {GridSizeXTxt.text = GridSizeX + v.ToString();});
        GridSizeYSlider.onValueChanged.AddListener((v) => {GridSizeYTxt.text = GridSizeY + v.ToString();});
        GridSizeXSlider.onValueChanged.AddListener(updateGridSizeX);
        GridSizeYSlider.onValueChanged.AddListener(updateGridSizeY);
        updateGridSizeX?.Invoke(GridSizeXSlider.value);
        updateGridSizeY?.Invoke(GridSizeYSlider.value);
        StartSimButton.onClick.AddListener(onStart);
        StartSimButton.onClick.AddListener(() => { gameObject.transform.parent.gameObject.SetActive(false); });
    }
}
