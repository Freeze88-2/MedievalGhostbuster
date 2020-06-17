using UnityEngine;

namespace AI.Movement
{
    /// <summary>
    /// Stops the AI from being too close pushing them back with a given force
    /// </summary>
    public class AISeparation : IBehaviour
    {
        // Stores all the AI Entities given
        private readonly AIEntity[] _aIEntities;

        // Stores the maximum distance to detect other AIs
        private readonly float _maxDistance;

        /// <summary>
        /// Constructor of the AISeparation
        /// </summary>
        /// <param name="entities"> All the entities found </param>
        /// <param name="maxDis"> The maximum range of detection </param>
        public AISeparation(AIEntity[] entities, float maxDis)
        {
            // Stores the given values to the variables created
            _aIEntities = entities;
            _maxDistance = maxDis;
        }

        /// <summary>
        /// Calculates the velocity the AI should dodge to
        /// </summary>
        /// <param name="current"> The AI to be moved </param>
        /// <param name="target"> The target position of the AI </param>
        /// <returns> A new  <see cref="SteeringBehaviour"/>
        /// with a angle </returns>
        public SteeringBehaviour GetOutput(AIEntity current, Vector3 target)
        {
            // Creates an zero Vector3
            Vector3 vel = Vector3.zero;

            // Cycles through all the entities stored
            for (int i = 0; i < _aIEntities.Length; i++)
            {
                // If the entity doesn't exist or is the AI continue
                if (_aIEntities[i] == null || current == _aIEntities[i])
                {
                    continue;
                }

                // Gets the direction between the AI and the current Entity
                Vector3 dir = current.transform.position -
                     _aIEntities[i].transform.position;

                // Stores the distance by finding the magnitude of the dir
                float distance = dir.magnitude;

                // Checks if the distance is less than the maximum distance
                if (distance < _maxDistance)
                {
                    // Calculates the force to push itself
                    float amount = 20 / distance * distance;

                    // Adds the amount times the direction to the vel vector
                    vel += amount * dir.normalized;
                }
            }
            // Returns a new SteeringBehaviour with the desired velocity
            return new SteeringBehaviour(vel, 0f);
        }
    }
}