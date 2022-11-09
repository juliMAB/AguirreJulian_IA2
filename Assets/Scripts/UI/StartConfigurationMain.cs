using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StartConfigurationMain : MonoBehaviour
{

    public Text GridSizeXTxt;
    public Text GridSizeZTxt;
    public Slider GridSizeXSlider;
    public Slider GridSizeZSlider;
    public Button StartSimButton;

    string GridSizeX = "SizeX : ";
    string GridSizeZ = "SizeZ : ";

    public void MyStart(UnityAction<float>updateGridSizeX, UnityAction<float> updateGridSizeZ, UnityAction onStart)
    {
        GridSizeXTxt.text = GridSizeX + GridSizeXSlider.value.ToString();
        GridSizeZTxt.text = GridSizeZ + GridSizeZSlider.value.ToString();
        GridSizeXSlider.onValueChanged.AddListener((v) => {GridSizeXTxt.text = GridSizeX + v.ToString();});
        GridSizeZSlider.onValueChanged.AddListener((v) => {GridSizeZTxt.text = GridSizeZ + v.ToString();});
        GridSizeXSlider.onValueChanged.AddListener(updateGridSizeX);
        GridSizeZSlider.onValueChanged.AddListener(updateGridSizeZ);
        StartSimButton.onClick.AddListener(onStart);
        StartSimButton.onClick.AddListener(() => { gameObject.transform.parent.gameObject.SetActive(false); });
    }
}
