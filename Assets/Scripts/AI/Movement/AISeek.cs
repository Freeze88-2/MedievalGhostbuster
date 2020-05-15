using UnityEngine;

namespace AI.Movement
{
    public class AISeek : IBehaviour
    {
        // Update is called once per frame
        public SteeringBehaviour GetOutput(AIEntity current, Vector3 target)
        {
            Vector3 velocity = current.transform.position - target;
            velocity = velocity.normalized * 50;

            return new SteeringBehaviour(velocity, 0);
        }
    }
}