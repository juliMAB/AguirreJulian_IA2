using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Agent : BaseUnit
{
    [SerializeField] private int teamID = -1;
    [SerializeField] float fitness = 0;

    [SerializeField] protected Genome genome;
    [SerializeField] protected NeuralNetwork brain;
    [SerializeField] protected float[] inputs;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Think(float dt) { }

    public void SetNearestMine(GameObject food) { }

    public void SetTeamID(int id) => teamID = id;

    public void SetBrain(Genome genome, NeuralNetwork brain)
    {
        this.genome = genome;
        this.brain = brain;
        inputs = new float[brain.InputsCount];
        OnReset();
    }
    private void OnReset()
    {
        fitness = 1;
    }
}
