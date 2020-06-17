using UnityEngine;

namespace AI.Movement
{
    /// <summary>
    /// Finds the closest AI and creates a velocity vector to move away from it
    /// </summary>
    public class AIObstacleAvoidance : IBehaviour
    {
        // Stores all the AI Entities given
        private readonly AIEntity[] _aIEntities;

        // Stores the maximum distance to detect other AIs
        private readonly float _detectionRadius;

        /// <summary>
        /// Constructor of the AISeparation
        /// </summary>
        /// <param name="entities"> All the entities found </param>
        /// <param name="maxDis"> The maximum range of detection </param>
        public AIObstacleAvoidance(AIEntity[] entities, float maxDis)
        {
            // Stores the given values to the variables created
            _aIEntities = entities;
            _detectionRadius = maxDis;
        }

        /// <summary>
        /// Calculates the angle to where the AI the AI wants to look at
        /// </summary>
        /// <param name="current"> The AI to be moved </param>
        /// <param name="target"> The target position of the AI </param>
        /// <returns> A new  <see cref="SteeringBehaviour"/>
        /// with a angle </returns>
        public SteeringBehaviour GetOutput(AIEntity current, Vector3 target)
        {
            // Defines the distance of detection according to the velocity
            float dynamicLen = 2 *
                (current.Velocity.magnitude / current.Speed);

            // Defines a vector for the detection
            Vector3 ahead = current.transform.position +
                current.Velocity.normalized * dynamicLen;

            // Defines a vector for the velocity to be added
            Vector3 avoidance = Vector3.zero;

            // Gets the closest AI
            AIEntity mostThreatening = FindMostThreateningObstacle(current);

            // Checks if the mostThreatning exists
            if (mostThreatening != null)
            {
                // Finds the force for the X of the vector
                avoidance.x = ahead.x - mostThreatening.transform.position.x;

                // Finds the force for the Z of the vector
                avoidance.z = ahead.z - mostThreatening.transform.position.z;

                // Calculates the push force by finding the distance
                float pushForce = Vector3.Distance(
                    mostThreatening.transform.position,
                    current.transform.position) - _detectionRadius;

                // Normalizes the avoidance vector
                avoidance = avoidance.normalized;
                // Multiplies the force by a value
                avoidance *= Mathf.Abs(pushForce * 150f);
            }
            // Returns a new SteeringBehaviour with the wanted velocity
            return new SteeringBehaviour(avoidance, 0f);
        }

        /// <summary>
        /// Finds the closest and AI to the current AI
        /// </summary>
        /// <param name="ent"> The current AI </param>
        /// <returns> The closest AI </returns>
        private AIEntity FindMostThreateningObstacle(AIEntity ent)
        {
            // Creates a new AIEntity
            AIEntity mostThreatening = null;

            // Cycles through all the AIs
            for (int i = 0; i < _aIEntities.Length; i++)
            {
                // Checks if the AI is not the one given or non-existing
                if (_aIEntities[i] == null || _aIEntities[i] == ent) continue;

                // Checks if the distance between the current and AI is less
                // than the given max radius
                bool collision = Vector3.Distance(ent.transform.position,
                    _aIEntities[i].transform.position) < _detectionRadius;

                // Checks if it's colliding and the distance to the current
                // is lower than the distance to the mostThreatning
                if (collision && (mostThreatening == null ||
                    Vector3.Distance(ent.transform.position,
                    _aIEntities[i].transform.position) <
                    Vector3.Distance(ent.transform.position,
                    mostThreatening.transform.position)))
                {
                    // Assigns the current AI to the mostThreatning
                    mostThreatening = _aIEntities[i];
                }
            }
            // Returns the closest AI
            return mostThreatening;
        }
    }
}