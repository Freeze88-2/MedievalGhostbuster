using UnityEngine;

namespace AI.Movement
{
    public class AIObstacleAvoidance : IBehaviour
    {
        private readonly AIEntity[] _aIEntities;
        private readonly float _detectionRadius;

        public AIObstacleAvoidance(AIEntity[] allGhosts, float radius)
        {
            _aIEntities = allGhosts;
            _detectionRadius = radius;
        }

        public SteeringBehaviour GetOutput(AIEntity current, Vector3 target)
        {
            float dynamicLen = 2 *(current.Velocity.magnitude / current.Speed);

            Vector3 ahead = current.transform.position + 
                current.Velocity.normalized * dynamicLen;

            Vector3 avoidance = Vector3.zero;

            AIEntity mostThreatening = FindMostThreateningObstacle(current);

            if (mostThreatening != null)
            {
                avoidance.x = ahead.x - mostThreatening.transform.position.x;
                avoidance.z = ahead.z - mostThreatening.transform.position.z;

                float pushForce = Vector3.Distance(
                    mostThreatening.transform.position, 
                    current.transform.position) - _detectionRadius;

                avoidance = avoidance.normalized;
                avoidance *= Mathf.Abs(pushForce * 100f);
            }
            return new SteeringBehaviour(avoidance, 0f);
        }

        private AIEntity FindMostThreateningObstacle(AIEntity ent)
        {
            AIEntity mostThreatening = null;

            for (int i = 0; i < _aIEntities.Length; i++)
            {
                if (_aIEntities[i] == null ||_aIEntities[i] == ent) continue;

                AIEntity obstacle = _aIEntities[i];

                bool collision = Vector3.Distance(ent.transform.position, 
                    obstacle.transform.position) < _detectionRadius;

                if (collision && (mostThreatening == null ||
                            Vector3.Distance(ent.transform.position,
                            obstacle.transform.position) <
                            Vector3.Distance(ent.transform.position,
                            mostThreatening.transform.position)))
                {
                    mostThreatening = obstacle;
                }
            }
            return mostThreatening;
        }
    }
}