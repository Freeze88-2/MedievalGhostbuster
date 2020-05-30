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
            for (int i = -1; i < _aIEntities.Length; i++)
            {
                Vector3 dir = Vector3.zero;
                if (i != -1)
                {
                    if (_aIEntities[i] == null || current == _aIEntities[i]
                        || current == null) continue;

                    dir = current.transform.position -
                        _aIEntities[i].transform.position;
                }
                else
                {
                    dir = current.transform.position -
                        current.target.transform.position;
                }
                float distance = dir.magnitude;

                if (distance < _maxDistance)
                {
                    float amount = 150 / distance * distance;

                    return new SteeringBehaviour(amount * dir.normalized, 0f);
                }
            }
            return new SteeringBehaviour(Vector3.zero, 0f);
        }
    }
}