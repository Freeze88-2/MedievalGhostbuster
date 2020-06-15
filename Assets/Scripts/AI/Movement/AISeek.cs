using UnityEngine;

namespace AI.Movement
{
    /// <summary>
    /// Returns a <see cref="SteeringBehaviour"/>
    /// with a velocity to where the AI target 
    /// </summary>
    public class AISeek : IBehaviour
    {
        /// <summary>
        /// Calculates the velocity to where the AI want to move
        /// </summary>
        /// <param name="current"> The AI to be moved </param>
        /// <param name="target"> The target position of the AI </param>
        /// <returns> A new  <see cref="SteeringBehaviour"/> 
        /// with a velocity </returns>
        public SteeringBehaviour GetOutput(AIEntity current, Vector3 target)
        {
            // Calculates the direction to the target
            Vector3 velocity = current.transform.position - target;

            // Normalizes the vector and multiplies it by 40
            velocity = velocity.normalized * 40;

            // Returns a SteeringBehaviour with the velocity inverted
            return new SteeringBehaviour(-velocity, 0f);
        }
    }
}