using System.Collections.Generic;
using UnityEngine;

public class AStarAlgorithm
{
    // Creates a list for storing a temporary path
    private readonly List<Node> open = new List<Node>();

    // Creates an HashSet for storing a locked path
    private readonly HashSet<Node> closed = new HashSet<Node>();

    // The finished full path to the target
    private readonly List<Node> path = new List<Node>();

    /// <summary>
    /// A* algorithm for searching the bes path to a position
    /// </summary>
    /// <param name="start"> The node it begins on </param>
    /// <param name="target"> The node it should end on </param>
    /// <returns> A list of positions </returns>
    public List<Vector3> CalculatePath(Node start, Node target)
    {
        // Clears the list to make sure they're empty before starting to
        // use them
        open.Clear();
        closed.Clear();

        // Adds the the ghost position 'start' to the open list
        open.Add(start);

        // Searches for a path while the open list has something or it
        // returned something
        while (open.Count > 0)
        {
            // Creates a variable and assigns it the first object on the
            // open list (initially the ghost itself)
            Node current = open[0];

            // Performs a loop while i is less than the amount of node
            // on Open list, ignoring the first node
            for (int i = 1; i < open.Count; i++)
            {
                // Checks if the cost of a node on the open list is less
                // or equals than the cost of the current node
                if (open[i].CombinedCost < current.CombinedCost
                    || open[i].CombinedCost == current.CombinedCost)
                {
                    // Checks which Object is clossest to the target
                    if (open[i].closenessCost < current.closenessCost)
                    {
                        // Sets the current Object to the current Object on
                        // the open list
                        current = open[i];
                    }
                }
            }

            // Removes the current piece from the open list
            open.Remove(current);
            // Adds it to the locked path list
            closed.Add(current);

            // Checks every neighbor of the target
            for (int i = 0; i < target.neighbors.Length; i++)
            {
                // Checks if the current position is a target neighbour
                if (current.pos == target.neighbors[i].pos)
                {
                    // Generates a list of positions and sends it back
                    return TracePath(start, target.neighbors[i]);
                }
            }

            // Checks all the Objects on the neighbors list
            for (int b = 0; b < current.neighbors.Length; b++)
            {
                // Checks if the Object is already in the locked path
                if (!closed.Contains(current.neighbors[b]) &&
                    current.neighbors[b].Walkable &&
                    current.neighbors[b].GhostID == null)
                {
                    // Local variable combining the distance to the start 
                    // and the distance between the current position and
                    // that neighbor
                    float newCostMov = current.distanceCost + Vector3.Distance
                        (current.pos, current.neighbors[b].pos);

                    // Checks if that variable is lower than the current
                    // distance of the Object and open list doesn't contain
                    // it
                    if (newCostMov < current.neighbors[b].distanceCost
                        || !open.Contains(current.neighbors[b]))
                    {
                        // Sets a new distance cost to that neighbor
                        current.neighbors[b].distanceCost = newCostMov;
                        // Sets a new closeness to that neighbor
                        current.neighbors[b].closenessCost = Vector3.Distance
                            (current.neighbors[b].pos, target.pos);
                        // Sets the parent of that neighbor the current
                        // object
                        current.neighbors[b].Parent = current;
                        // Adds that neighbor to the open list
                        open.Add(current.neighbors[b]);
                    }
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Forms a list of positions by getting the positions of the parents
    /// of the Nodes until the Node is the start position
    /// </summary>
    /// <param name="start"> The Object it starts on </param>
    /// <param name="end"> The Object it targets </param>
    private List<Vector3> TracePath(Node start, Node end)
    {
        // Creates a list of positions to store the path
        path.Clear();
        // Creates a local Object variable and assigns it the passed Object
        Node currentPiece = end;
        Node nextPiece = currentPiece.Parent;

        if (nextPiece != null)
        {
            Vector2 dir = GetDirection(currentPiece, nextPiece);

            // Runs the loop until the currentPiece is not the start
            while (currentPiece.pos != start.pos)
            {

                if (GetDirection(currentPiece, nextPiece) != dir ||
                    currentPiece.pos == start.pos ||
                    currentPiece.pos == end.pos)
                {
                    // Adds the position of the currentPiece to the list
                    path.Add(currentPiece);
                }

                dir = GetDirection(currentPiece, nextPiece);

                // Assigns the currentPiece the parent of that piece
                currentPiece = currentPiece.Parent;
                nextPiece = currentPiece.Parent;

            }
        }
        int count;

        for (int y = 0; y < path.Count; y++)
        {
            count = 0;
            for (int n = 0; n < path[y].neighbors.Length; n++)
            {
                if (path.Contains(path[y].neighbors[n]))
                {
                    count++;
                }
            }
            if (count >= 2)
            {
                path.Remove(path[y]);
            }
        }

        // The path forms from end to start, so it needs to be reversed
        path.Reverse();

        List<Vector3> a = new List<Vector3>();

        for (int k = 0; k < path.Count; k++)
        {
            a.Add(path[k].Position);
        }
        return a;
    }

    private Vector2 GetDirection(Node a, Node b)
    {
        return a.pos - b.pos;
    }
}


