using UnityEngine;

public class Agent : BaseAgent
{
    [SerializeField] public int teamID = -1;
    [SerializeField] public float fitness = 0;
    [SerializeField] float agresivity = 0;
    [SerializeField] float friendly = 0;
    [SerializeField] public bool FlagDie = false;
    [SerializeField] public int eatFood = 0;
    [SerializeField] public int generationCount = 0;


    protected override float OnThinkFight()
    {
        Debug.Log(UnitName + " pensando si pelear");
        return agresivity;
    }

    public float OnThinkShared()
    {
        Debug.Log(UnitName + " pensando en compartir");
        return friendly;
    }

    public void CalculateNewRandPosition()
    {
        Debug.Log(UnitName + "a cedido su lugar");
        Tile NewTile;
        do
        {
            NewTile = Utilitys.currentGrid.GetTileAtPosition
                (
                new Vector2Int(
                    this.NewTile.pos.x + Random.Range(-1, 2),
                    this.NewTile.pos.y + Random.Range(-1, 2)
                    )
                );

        } while (this.NewTile == NewTile);
        
    }

    protected override void OnThink()
    {
        Vector3 dirToMine = GetDirToMine(food.gameObject);
        Vector3 dir = this.transform.up;
        if (dir.x<0.01f)
        {
            dir.x = 0;
        }
        if (dir.y<0.01f)
        {
            dir.y = 0;
        }

        inputs[0] = dirToMine.x;
        inputs[1] = dirToMine.y;
        inputs[2] = dir.x;
        inputs[3] = dir.y;
        inputs[4] = agresivity;
        inputs[5] = friendly;

        float[] output = brain.Synapsis(inputs);
        agresivity = Mathf.Clamp01(output[2]);
        friendly = Mathf.Clamp01(output[3]);
        SetForces(output[0], output[1]);
    }

    public void SetTeamID(int id) => teamID = id;

    public override void OnReset()
    {
        fitness = 1;
        genome.fitness = fitness;
        eatFood = 0;
    }
    protected override void OnTakeFood()
    {
        fitness += 1;
        eatFood++;
        genome.fitness = fitness;
    }
}
