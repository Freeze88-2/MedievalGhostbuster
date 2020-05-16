using UnityEngine;

namespace AI.Movement
{
    public class AIObstacleAvoidance : IBehaviour
    {
        private readonly AIEntity[] _aIEntities;
        private readonly float diameter = 5;

        public AIObstacleAvoidance(AIEntity[] allGhosts)
        {
            _aIEntities = allGhosts;
        }

        public SteeringBehaviour GetOutput(AIEntity current, Vector3 velocity)
        {
            //return new SteeringBehaviour();

            Vector3 ahead = current.transform.position + current.Velocity.normalized * diameter;
            Vector3 ahead2 = current.transform.position + current.Velocity.normalized * diameter * 0.5f;
            Vector3 avoidance = Vector3.zero;

            AIEntity mostThreatening = FindMostThreateningObstacle(current, ahead, ahead2);

            if (mostThreatening != null)
            {
                avoidance.x = ahead.x - mostThreatening.transform.position.x;
                avoidance.z = ahead.z - mostThreatening.transform.position.z;

                avoidance = avoidance.normalized;
                avoidance.y = 0;
                avoidance *= 200f;
            }
            return new SteeringBehaviour(-avoidance, 0f);
        }

        private AIEntity FindMostThreateningObstacle(AIEntity ent, Vector3 ahead, Vector3 ahead2)
        {
            AIEntity mostThreatening = null;

            for (int i = 0; i < _aIEntities.Length; i++)
            {
                if (_aIEntities[i] == null) continue;

                AIEntity obstacle = _aIEntities[i];
                bool collision = LineIntersectsCircle(ahead, ahead2, obstacle.transform.position);

                if (collision && (mostThreatening == null ||
                            Distance(ent.transform.position,
                            obstacle.transform.position) <
                            Distance(ent.transform.position,
                            mostThreatening.transform.position)))
                {
                    mostThreatening = obstacle;
                }
            }
            return mostThreatening;
        }

        private bool LineIntersectsCircle(Vector3 ahead, Vector3 ahead2, Vector3 center)
        {
            return Distance(center, ahead) <= 2 || Distance(center, ahead2) <= 2;
        }
        private float Distance(Vector3 a, Vector3 b)
        {
            return Mathf.Sqrt((a.x - b.x) * (a.x - b.x) + (a.z - b.z) * (a.z - b.z));
        }
    }
}