using System.Collections;
using System.Collections.Generic;
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
    }
}
