using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool Walkable { get; }
    public Vector3 Position { get; }
    public Node Parent { get; set; }
    public Vector2 pos;
    public Node[] neighbors;

    public float CombinedCost { get => closenessCost + distanceCost; }
    public float closenessCost;
    public float distanceCost;

    public Node(bool walk, Vector3 pos, Vector2 vector)
    {
        Walkable = walk;
        Position = pos;
        this.pos = vector;
        closenessCost = 0;
        distanceCost = 0;
    }
}
