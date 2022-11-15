using UnityEngine;

public class AgentEmbellishment : MonoBehaviour
{
    private void Start()
    {
        if (GetComponent<Agent>().teamID == 0)
        {
            Material mat = GetComponent<Renderer>().material;
            float RG = Random.value * 0.5f;
            mat.color = Color.blue + new Color(RG + 0.5f, RG + 0.5f,0,1);
        }
        else
        {
            Material mat = GetComponent<Renderer>().material;
            float GB = Random.value * 0.5f;
            mat.color = new Color(1, GB + 0.5f, GB + 0.5f, 1);
        }
    }
}
