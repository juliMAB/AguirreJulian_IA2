using UnityEngine;
using UnityEngine.UI;

public class EndSimScreen : MonoBehaviour
{
    [SerializeField] Button ResetButton;

    [SerializeField] Text dataText;
    public void MyStart(string data)
    {
        ResetButton.onClick.AddListener(()=>UnityEngine.SceneManagement.SceneManager.LoadScene(0));
        dataText.text = data;
    }
}
