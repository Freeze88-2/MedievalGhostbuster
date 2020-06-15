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
        public SteeringBehaviour GetOutput(AIEntity current, Vector3 target)
        {
            Vector3 vel = Vector3.zero;

            for (int i = 0; i < _aIEntities.Length; i++)
            {
                if (_aIEntities[i] == null || current == _aIEntities[i]
                    || current == null) continue;

                Vector3 dir = current.transform.position -
                     _aIEntities[i].transform.position;

                float distance = dir.magnitude;

                if (distance < _maxDistance)
                {
                    float amount = 20 / distance * distance;

                    vel += amount * dir.normalized;
                }
            }
            return new SteeringBehaviour(vel, 0f);
        }
    }
}