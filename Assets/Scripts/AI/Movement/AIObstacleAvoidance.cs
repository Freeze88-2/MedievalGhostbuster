using UnityEngine;

namespace AI.Movement
{
    public class AIObstacleAvoidance : IBehaviour
    {
        private readonly AIEntity[] entities;
        private float diameter = 5;
        public AIObstacleAvoidance(AIEntity[] allGhosts)
        {
            entities = allGhosts;
        }

        public SteeringBehaviour GetOutput(AIEntity current, Vector3 velocity)
        {
            //return new SteeringBehaviour();

            Vector3 ahead = current.transform.position + current.Velocity.normalized * diameter;
            Vector3 ahead2 = current.transform.position + current.Velocity.normalized * diameter * 0.5f;

            //for (int i = 0; i < entities.Length; i++)
            //{
            //    if (LineIntersectsCircle(ahead, ahead2, entities[i].transform.position))
            //    {
            //        Vector3 avoidanceForce = (ahead - entities[i].transform.position).normalized * 10f;
            //    }
            //}

            AIEntity mostThreatening = FindMostThreateningObstacle(current, ahead, ahead2);
            Vector3 avoidance = Vector3.zero;

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

            for (int i = 0; i < entities.Length; i++)
            {
                AIEntity obstacle = entities[i];
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

//AIEntity initialTarget = null;
//float initialMinSeperation = 0;
//float initialDistance = 0;
//Vector3 initialRealPos = Vector3.zero;
//Vector3 initialRealVel = Vector3.zero;
//float lowestTime = 10000f;

//for (int i = 0; i < entities.Length; i++)
//{
//    Vector3 realPos = current.transform.position - entities[i].transform.position;
//    Vector3 realVel = entities[i].Velocity - current.Velocity;
//    float collisionDelta = Vector3.Dot(realPos, realVel) / (realVel.magnitude * realVel.magnitude);

//    float minSeparation = realPos.magnitude - realVel.magnitude * collisionDelta;

//    if (minSeparation > 3) continue;

//    if (collisionDelta > 0 && collisionDelta < lowestTime)
//    {
//        lowestTime = collisionDelta;
//        initialTarget = entities[i];
//        initialMinSeperation = minSeparation;
//        initialDistance = realPos.magnitude;
//        initialRealPos = realPos;
//        initialRealVel = realVel;
//    }
//}

//if (initialTarget != null)
//{
//    Vector3 realPos =
//        initialMinSeperation <= 0 || initialDistance < 3f ?
//        current.transform.position -
//        initialTarget.transform.position :
//        initialRealPos + initialRealVel * lowestTime;

//    velocity = -realPos.normalized * current.Speed;
//}
//return new SteeringBehaviour(velocity, 0f);