using UnityEngine;

namespace AI.Movement
{
    public class AIObstacleAvoidance : IBehaviour
    {
        private readonly AIEntity[] _aIEntities;
        private bool isNeraby;
        public AIObstacleAvoidance(AIEntity[] allGhosts)
        {
            _aIEntities = allGhosts;
        }

        public SteeringBehaviour GetOutput(AIEntity current, Vector3 target)
        {
            //return new SteeringBehaviour();
            float dynamicLen = 2 *(current.Velocity.magnitude / current.Speed);

            Vector3 ahead = current.transform.position + current.Velocity.normalized * dynamicLen;
            Vector3 ahead2 = current.transform.position + current.Velocity.normalized * dynamicLen * 0.5f;
            Vector3 avoidance = Vector3.zero;

            AIEntity mostThreatening = FindMostThreateningObstacle(current, ahead, ahead2);

            if (mostThreatening != null)
            {
                avoidance.x = ahead.x - mostThreatening.transform.position.x;
                avoidance.z = ahead.z - mostThreatening.transform.position.z;

                avoidance = avoidance.normalized;
                avoidance.y = 0;
                avoidance *= Mathf.Abs(Vector3.Distance(mostThreatening.transform.position, current.transform.position) - 2f) * 100f;
            }
            return new SteeringBehaviour(avoidance, 0f);
        }

        private AIEntity FindMostThreateningObstacle(AIEntity ent, Vector3 ahead, Vector3 ahead2)
        {
            AIEntity mostThreatening = null;

            for (int i = 0; i < _aIEntities.Length; i++)
            {
                if (_aIEntities[i] == null ||_aIEntities[i] == ent) continue;

                AIEntity obstacle = _aIEntities[i];
                bool collision = Vector3.Distance(ent.transform.position, obstacle.transform.position) < 2f;

                
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