using UnityEngine;

namespace AI.Movement
{
    /// <summary>
    /// Returns a <see cref="SteeringBehaviour"/>
    /// with a rotation to where the AI target 
    /// </summary>
    public class AIRotateToTarget : IBehaviour
    {
        /// <summary>
        /// Calculates the angle to where the AI the AI wants to look at
        /// </summary>
        /// <param name="current"> The AI to be moved </param>
        /// <param name="target"> The target position of the AI </param>
        /// <returns> A new  <see cref="SteeringBehaviour"/> 
        /// with a angle </returns>
        public SteeringBehaviour GetOutput(AIEntity current, Vector3 target)
        {
            // Stores the angle to rotate to, default to zero
            float angle = 0f;
             
            // Checks if the velocity is not 0
            if (current.Velocity != Vector3.zero)
            {
                // Sets the angle equal to a lerp between the current 
                // rotation and the rotation to look at
                angle = Quaternion.Lerp(current.transform.rotation,
                        Quaternion.LookRotation(current.Velocity),
                        Time.deltaTime * current.MaxSpeed * 5f).eulerAngles.y;
            }

            // Output the steering
            return new SteeringBehaviour(Vector3.zero, angle);
        }
    }
}