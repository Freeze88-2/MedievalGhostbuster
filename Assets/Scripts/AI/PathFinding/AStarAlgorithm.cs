using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarAlgorithm
{
    // Creates a list for storing a temporary path
    private readonly List<Node> open = new List<Node>();

    // Creates an HashSet for storing a locked path
    private readonly HashSet<Node> closed = new HashSet<Node>();

    // The finished full path to the target
    private List<Vector3> path = new List<Vector3>();


    /// <summary>
    /// A* algorithm for searching the bes path to a position
    /// </summary>
    /// <param name="target"> The end position it should arrive </param>
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

            // Performs a loop while i is less than the amount of Objects
            // on Open list, ignoring the first Object (the Ghost)
            for (int i = 1; i < open.Count; i++)
            {
                // Checks if the cost of a Object on the open list is less
                // or equals than the cost of the current Object
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

            // Checks if the current position is the same as the target
            if (current.Position == target.Position)
            {
                // Generates a list of positions and sends it back
                TracePath(start, target);
                return path;
            }

            // Checks all the Objects on the neighbors list
            for (int b = 0; b < current.neighbors.Length; b++)
            {
                // Checks if the Object is already in the locked path
                if (!closed.Contains(current.neighbors[b]) && current.neighbors[b].Walkable)
                {
                    // Local variable combining the distance to the start 
                    // and the distance between the current position and
                    // that neighbor
                    float newCostMov = current.distanceCost +
                        GetDistace(current, current.neighbors[b]);

                    // Checks if that variable is lower than the current
                    // distance of the Object and open list doesn't contain
                    // it
                    if (newCostMov < current.neighbors[b].distanceCost
                        || !open.Contains(current.neighbors[b]))
                    {
                        // Sets a new distance cost to that neighbor
                        current.neighbors[b].distanceCost = newCostMov;
                        // Sets a new closeness to that neighbor
                        current.neighbors[b].closenessCost =
                             GetDistace(current.neighbors[b], target);
                        // Sets the parent of that neighbor the current
                        // object
                        current.neighbors[b].Parent = current;
                        // Adds that neighbor to the open list
                        open.Add(current.neighbors[b]);
                    }
                }
            }
        }
        return path;
    }

    /// <summary>
    /// Forms a list of positions by getting the positions of the parents
    /// of the Object until the Object is the start position
    /// </summary>
    /// <param name="end"> The Object it targets </param>
    private void TracePath(Node start, Node end)
    {
        // Creates a list of positions to store the path
        path.Clear();
        // Creates a local Object variable and assigns it the passed Object
        Node currentPiece = end;

        // Runs the loop until the currentPiece is not the start
        while (currentPiece.Position != start.Position)
        {
            // Adds the position of the currentPiece to the list
            path.Add(currentPiece.Position);
            // Assigns the currentPiece the parent of that piece
            currentPiece = currentPiece.Parent;
        }
        // The path forms from end to start, so it needs to be reversed
        path.Reverse();
    }

    /// <summary>
    /// A simple method for calculating distances
    /// </summary>
    /// <param name="A"> The first position </param>
    /// <param name="B"> The Second position </param>
    /// <returns> An Integer with the aprox distance between the two
    /// </returns>
    private float GetDistace(Node A, Node B)
    {
        // Distance formula (square root of ((x-x^2) + (y-y^2)))
        return (Mathf.Sqrt(Mathf.Pow(Mathf.Abs(A.pos.x - B.pos.x), 2) +
         Mathf.Pow(Mathf.Abs(A.pos.y - B.pos.y), 2)));
    }
}


