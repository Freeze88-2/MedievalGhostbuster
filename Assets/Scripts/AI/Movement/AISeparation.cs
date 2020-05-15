using UnityEngine;

namespace AI.Movement
{
    public class AISeparation : IBehaviour
    {
        private readonly AIEntity[] _aIEntities;
        private readonly float _maxDistance;

        public AISeparation(AIEntity[] entities, float maxDis)
        {
            _aIEntities = entities;
            _maxDistance = maxDis;
        }

        // Update is called once per frame
        public SteeringBehaviour GetOutput(AIEntity current, Vector3 vector)
        {
            for (int i = 0; i < _aIEntities.Length; i++)
            {
                if (current == _aIEntities[i] || current == null) continue;

                Vector3 dir = current.transform.position -
                    _aIEntities[i].transform.position;
                float distance = dir.magnitude;

                if (distance < _maxDistance)
                {
                    float amount = 90 / distance * distance;

                    return new SteeringBehaviour(-(amount * dir.normalized), 0f);
                }
            }

            //Vector3 dirr = current.transform.position -current.target.transform.position;
            //float distancee = dirr.magnitude;

            //if (distancee < _maxDistance)
            //{
            //    float amount = 100 / distancee * distancee;

            //    return new SteeringBehaviour(-(amount * dirr.normalized), 0f);
            //}
            return new SteeringBehaviour(Vector3.zero, 0f);
        }
    }
}