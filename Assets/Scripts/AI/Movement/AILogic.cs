using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILogic
{
    public List<Vector3> path { get; private set; }
    private GridGenerator grid = null;
    private Node start;
    private Node end;
    private AStarAlgorithm AStar;
    private int pathN;
    private int iD;

    public AILogic(GridGenerator grid)
    {
        pathN = 0;
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


        return GetNextPoint(init);
    }

    private Vector3? GetNextPoint(GameObject init)
    {
        if (path != null && path.Count > 0)
        {
            if (pathN >= path.Count - 1)
            {
                pathN = 0;
            }
            if (Vector3.Distance(init.transform.position, path[pathN]) < 0.01f &&
                pathN < path.Count)
            {
                pathN++;

                Debug.Log(pathN);
                Debug.Log(path.Count);
            }
            return path[pathN];
        }
        return null;
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
