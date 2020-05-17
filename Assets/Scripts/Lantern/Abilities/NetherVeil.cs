using System.Collections;
using UnityEngine;

namespace Lantern.Abilities
{
    public class NetherVeil : MonoBehaviour, IAbility
    {
        [SerializeField] private float _duration = 6f;
        [SerializeField] private GameObject _particles = null;
        [SerializeField] private GameObject _postProcessing = null;

        private IEntity _player;
        private GameObject _playerObj;
        private bool _inNetherInstance;
        private GameObject _curparticles;
        private GameObject _po;

        public (GhostColor, GhostColor) AbilityColors
        {
            get => (GhostColor.Red, GhostColor.Blue);
        }

        public bool HabilityEnded { get; private set; }

        // Start is called before the first frame update
        private void Start()
        {
            _po = null;
            _curparticles = null;
            _playerObj = GameObject.FindGameObjectWithTag("Player");
            _player = _playerObj.GetComponent<IEntity>();

            HabilityEnded = false;
        }

        public void ActivateAbility()
        {
            if (!_inNetherInstance)
            {
                HabilityEnded = false;
                StartCoroutine(InNetherInstance());
            }
            else
            {
                StopCoroutine(InNetherInstance());
                ResetHability();
            }
        }

        private IEnumerator InNetherInstance()
        {
            _inNetherInstance = true;
            _player.IsTargatable = false;

            Physics.Raycast(_playerObj.transform.position,
                -_playerObj.transform.up, out RaycastHit hit, 100f,
                LayerMask.GetMask("Default"));

            _curparticles = Instantiate(_particles, hit.point, Quaternion.identity);
            _po = Instantiate(_postProcessing, _playerObj.transform);

            SphereCollider col = _po.GetComponent<SphereCollider>();

            float timer = 0;

            float amount = (col.radius / _duration);

            while (timer <= _duration)
            {
                col.radius -= amount * Time.deltaTime;
                timer += Time.deltaTime % 60;

                yield return null;

                ResetHability();
            }
        }

        private void ResetHability()
        {
            _player.IsTargatable = true;
            HabilityEnded = true;
            _inNetherInstance = false;
            Destroy(_curparticles);
            Destroy(_po);
        }
    }
}