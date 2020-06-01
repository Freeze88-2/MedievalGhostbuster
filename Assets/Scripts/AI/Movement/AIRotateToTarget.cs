using UnityEngine;

namespace AI.Movement
{
    public class AIRotateToTarget : IBehaviour
    {
        // Start is called before the first frame update
        public SteeringBehaviour GetOutput(AIEntity current, Vector3 target)
        {
            float angle = 0f;

            if (current.Velocity != Vector3.zero)
            {
                angle = Quaternion.Lerp(current.transform.rotation,
                        Quaternion.LookRotation(current.Velocity), Time.deltaTime *
                        current.MaxSpeed * 5f).eulerAngles.y;
            }

            // Output the steering
            return new SteeringBehaviour(Vector3.zero, angle);
        }
    }
}
