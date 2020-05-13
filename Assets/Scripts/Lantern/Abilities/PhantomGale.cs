using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lantern.Abilities
{
    public class PhantomGale : MonoBehaviour, IAbility
    {
        [SerializeField] private float _coneRadiusX = 3f;
        [SerializeField] private float _coneRadiusY = 2f;
        [SerializeField] private float _coneLenght = 5f;

        List<RaycastHit> rays = new List<RaycastHit>();
        List<IEntity> alreadyCounted = new List<IEntity>();
        private GameObject _player;
        private WaitForSecondsRealtime _wait;
        private int _timer;
        private bool _isPushing;

        public (GhostColor, GhostColor) AbilityColors
        {
            get => (GhostColor.Red, GhostColor.Green);
        }

        public bool HabilityEnded { get; private set; }

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _wait = new WaitForSecondsRealtime(0.5f);
            _timer = 0;
            _isPushing = false;
            HabilityEnded = false;
        }

        public void ActivateAbility()
        {
            HabilityEnded = false;
            rays.Clear();
            alreadyCounted.Clear();

            for (float j = -_coneRadiusY; j <= _coneRadiusY; j += 0.2f)
            {
                for (float i = -_coneRadiusX; i <= _coneRadiusX; i += 0.2f)
                {
                    Vector3 ray = (_player.transform.forward * _coneLenght) +
                        (_player.transform.right * i) + (_player.transform.up * j);

                    //Ray r = new Ray(_player.transform.position, ray);

                    Debug.DrawRay(_player.transform.position, ray, Color.red, 100f);

                    RaycastHit[] allhit = Physics.RaycastAll(_player.transform.position, ray
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

            //HabilityEnded = true;
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