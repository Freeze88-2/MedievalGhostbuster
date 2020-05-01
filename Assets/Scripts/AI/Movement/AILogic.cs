using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Checks pathfinding and returns a point
/// </summary>
public class AILogic
{
    /// <summary>
    /// List of points found by the pathfinding
    /// </summary>
    public List<Vector3> Path { get; private set; }
    // The designated area of the ghost
    private readonly GridGenerator grid = null;
    // The starting node
    private Node start;
    // The ending node
    private Node end;
    // Instance of the pathfinding algorithm
    private readonly AStarAlgorithm AStar;
    // Unique ID of the ghost
    private readonly int iD;

    /// <summary>
    /// Consctructor the AILogic
    /// </summary>
    /// <param name="grid"> The area of this AI </param>
    public AILogic(GridGenerator grid)
    {
        Path = new List<Vector3>();
        this.grid = grid;
        AStar = new AStarAlgorithm();
        iD = System.DateTime.Now.Millisecond + Random.Range(0, 10000);
    }
    public Vector3? GetPoint(GameObject init, GameObject target)
    {
        SetGhostMinDistance(false);

        start = grid.GetClosestNode(init.transform.position);

        end = grid.GetClosestNode(target.transform.position);

        if (start != null && end != null)
        {
            start.GhostID = iD;
            Path = AStar.CalculatePath(start, end);
        }
        SetGhostMinDistance(true);


        if (Path != null && Path.Count > 0)
        {
            return Path[0];
        }
        else
        {
            return null;
        }
    }

    private void SetGhostMinDistance(bool state)
    {
        if (start != null)
        {
            for (int i = 0; i < start.neighbors.Length; i++)
            {
                if (state)
                {
                    if (start.neighbors[i].GhostID == null)
                    {
                        start.neighbors[i].GhostID = iD;
                    }
                }
                else
                {
                    if (start.neighbors[i].GhostID == iD)
                    {
                        start.neighbors[i].GhostID = null;
                    }
                }
            }
        }
    }
}
