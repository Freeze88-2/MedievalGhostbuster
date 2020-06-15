using UnityEngine;

namespace AI.Movement
{
    /// <summary>
    /// Holds information about a velocity and angle of a behavior
    /// </summary>
    public struct SteeringBehaviour
    {
        /// <summary>
        /// The desired angle for the AI to face
        /// </summary>
        public float Angle { get; }

        /// <summary>
        /// The desired velocity of for the AI
        /// </summary>
        public Vector3 Velocity { get; }

        /// <summary>
        /// Constructor of the SteeringBehaviour
        /// </summary>
        /// <param name="velocity"> A Vector3 with the wanted velocity </param>
        /// <param name="angle"> float with an angle </param>
        public SteeringBehaviour(Vector3 velocity, float angle)
        {
            // Velocity given
            Velocity = velocity;
            // Angle given
            Angle = angle;
        }

        /// <summary>
        /// Plus operator overload to add two SteeringBehaviours
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static SteeringBehaviour operator +
            (SteeringBehaviour left, SteeringBehaviour right)
        {
            // Returns both velocity vectors added and both angles added
            return new SteeringBehaviour
                (left.Velocity + right.Velocity, left.Angle + right.Angle);
        }
    }
}