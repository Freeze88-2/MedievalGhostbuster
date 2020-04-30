using System.Collections.Generic;
using UnityEngine;

public class AILogic
{
    public List<Vector3> path { get; private set; }
    private readonly GridGenerator grid = null;
    private Node start;
    private Node end;
    private readonly AStarAlgorithm AStar;
    private readonly int iD;

    public AILogic(GridGenerator grid)
    {
        path = new List<Vector3>();
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
            path = AStar.CalculatePath(start, end);
        }
        SetGhostMinDistance(true);


        if (path.Count > 0)
        {
            return path[0];
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
