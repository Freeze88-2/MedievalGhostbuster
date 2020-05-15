using UnityEngine;

namespace AI.Movement
{
    public interface IBehaviour
    {
        SteeringBehaviour GetOutput(AIEntity entity, Vector3 vector);
    }
}