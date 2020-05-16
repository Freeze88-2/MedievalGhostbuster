using System.Collections.Generic;
using UnityEngine;

namespace Lantern.Abilities
{
    public class PhantomGale : MonoBehaviour, IAbility
    {
        [SerializeField] private float _coneRadiusX = 3f;
        [SerializeField] private float _coneRadiusY = 0.5f;
        [SerializeField] private float _coneLenght = 5f;
        [SerializeField] private GameObject _particles;

        private List<RaycastHit> rays = new List<RaycastHit>();
        private List<IEntity> alreadyCounted = new List<IEntity>();
        private GameObject _player;

        public (GhostColor, GhostColor) AbilityColors
        {
            get => (GhostColor.Red, GhostColor.Green);
        }

        public bool HabilityEnded { get; private set; }

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            HabilityEnded = false;
        }

        public void ActivateAbility()
        {
            HabilityEnded = false;
            rays.Clear();
            alreadyCounted.Clear();

            for (float j = -_coneRadiusY; j <= _coneRadiusY; j += 0.1f)
            {
                for (float i = -_coneRadiusX; i <= _coneRadiusX; i += 0.2f)
                {
                    Vector3 ray = (_player.transform.forward * _coneLenght) +
                        (_player.transform.right * i);

                    Vector3 start = _player.transform.position;
                    start.y += j;

                    Debug.DrawRay(start, ray, Color.red, 100f);

                    RaycastHit[] allhit = Physics.RaycastAll(start, ray
                        , _coneLenght, LayerMask.GetMask("Entity"));

                    for (int z = 0; z < allhit.Length; z++)
                    {
                        if (allhit[z].collider != null)
                        {
                            rays.Add(allhit[z]);
                        }
                    }
                }
            }
            PushEntities();
            GameObject particle = Instantiate(_particles);
            particle.transform.position = _player.transform.position;
            particle.transform.rotation = _player.transform.rotation;

            HabilityEnded = true;
        }

        private void PushEntities()
        {
            for (int i = 0; i < rays.Count; i++)
            {
                IEntity ghost = rays[i].collider.gameObject.GetComponent<IEntity>();

                if (ghost != null && !alreadyCounted.Contains(ghost))
                {
                    alreadyCounted.Add(ghost);

                    rays[i].collider.attachedRigidbody.AddExplosionForce(50f, _player.transform.position, 100f, 5f, ForceMode.Impulse);
                }
            }
        }
    }
}