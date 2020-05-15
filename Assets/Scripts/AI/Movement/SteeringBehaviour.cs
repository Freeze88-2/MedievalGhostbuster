using UnityEngine;

public struct SteeringBehaviour
{
    public float Angle { get; }

    public Vector3 Velocity { get; }

    public SteeringBehaviour(Vector3 velocity, float angle)
    {
        Angle = angle;
        Velocity = velocity;
    }
}