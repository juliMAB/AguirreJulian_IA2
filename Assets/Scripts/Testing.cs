using UnityEditor;
using UnityEngine;

public class Testing : MonoBehaviour {

    [SerializeField, Tooltip("The size of the grid on X coordinate."), Range(100, 300)] private int sizeGridX = 100;
    [SerializeField, Tooltip("The size of the grid on Y coordinate."), Range(100, 300)] private int sizeGridY = 100;

    private Grid grid;

    private void Start() {
        grid = new Grid(sizeGridX, sizeGridY, 10f, new Vector3(0, 0));
    }

    private void Update() {
        
    }
    private void OnDrawGizmos()
    {
        if (grid != null)
        {
            grid.drawGizmos();
        }
    }
}
