using UnityEngine;

public struct SteeringBehaviour
{
    public float Angle { get; }

    public Vector3 Velocity { get; }

    public SteeringBehaviour(Vector3 velocity, float angle)
    {
        Velocity = velocity;
        Angle = angle;
    }

    public static SteeringBehaviour operator +(SteeringBehaviour left, SteeringBehaviour right)
    {
        return new SteeringBehaviour(left.Velocity + right.Velocity, left.Angle + right.Angle);
    }
}