using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAgent : BaseUnit
{

    [SerializeField] public Genome genome;
    [SerializeField] public NeuralNetwork brain;
    [SerializeField] protected float[] inputs;

    [SerializeField] protected Food food;

    [SerializeField] private Vector3 newPos;


    public void SetBrain(Genome genome, NeuralNetwork brain)
    {
        this.genome = genome;
        this.brain = brain;
        inputs = new float[brain.InputsCount];
        OnReset();
    }
    protected Vector3 GetDirToMine(GameObject mine)
    {
        return (mine.transform.position - this.transform.position).normalized;
    }
    public void SetNearestFood(Food food)
    {
        this.food = food;
    }
    protected void SetForces(float leftForce, float rightForce)
    {
        newPos = this.transform.position;
        float rotFactor = Mathf.Clamp((rightForce - leftForce), -1.0f, 1.0f);
        rotFactor += 1.0f;
        rotFactor *= 2.0f; //para que el resultado vaya de 0 a 4; 0 front, 1 left,2 back, 3 right, 4 dontMove.
        int TotalRot = Mathf.RoundToInt (rotFactor) * 90;
        this.transform.rotation = Quaternion.AngleAxis(TotalRot, Vector3.forward);
        newPos += this.transform.up; // rotar y  adelantar en 1 forward.
    }

    public void Think()
    {
        OnThink();
        NewTile = Utilitys.currentGrid.GetTileAtPosition(new Vector2Int( Mathf.RoundToInt(newPos.x), Mathf.RoundToInt(newPos.y)));
    }
    public float ThinkFightOrRun()
    {
        return OnThinkFight();
    }
    public void ThinkAfterMove(float dt,float enemy)
    {
        OnThinkFightOrRun(dt,enemy);
    }
    public void AskTileFoodorMove()
    {
        if(food != null)
            if (food == NewTile.HasFood())
            {
                OnTakeFood();
                Destroy(food.gameObject);
            }
        MoveToNewTile();
    }

    protected virtual void OnThink()
    {

    }
    protected virtual float OnThinkFight()
    {
        return 1;
    }

    protected virtual void OnThinkFightOrRun(float dt,float enemy)
    {
        
    }

    protected virtual void OnTakeFood()
    {
    }

    public virtual void OnReset()
    {

    }
}
