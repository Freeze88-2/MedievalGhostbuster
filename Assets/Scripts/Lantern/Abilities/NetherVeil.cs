using System.Collections;
using UnityEngine;

namespace Lantern.Abilities
{
    public class NetherVeil : MonoBehaviour, IAbility
    {
        [SerializeField] private float _duration = 6f;
        [SerializeField] private GameObject _particles;

        private WaitForSeconds _wait;
        private IEntity _player;
        private GameObject _playerObj;
        private bool _inNetherInstance;
        private GameObject _curparticles;
        public (GhostColor, GhostColor) AbilityColors
        {
            get => (GhostColor.Red, GhostColor.Blue);
        }

        public bool HabilityEnded { get; private set; }

        // Start is called before the first frame update
        private void Start()
        {
            _curparticles = null;
            HabilityEnded = false;
            _wait = new WaitForSeconds(_duration);
            _playerObj = GameObject.FindGameObjectWithTag("Player");
            _player = _playerObj.GetComponent<IEntity>();
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
                Destroy(_curparticles);
                _player.IsTargatable = true;
                HabilityEnded = true;
                _inNetherInstance = false;
            }
        }

        private IEnumerator InNetherInstance()
        {
            Physics.Raycast(_playerObj.transform.position,
                -_playerObj.transform.up, out RaycastHit hit, 100f,
                LayerMask.GetMask("Default"));

            _curparticles = Instantiate(_particles,hit.point, Quaternion.identity,_playerObj.transform);

            
            _inNetherInstance = true;
            _player.IsTargatable = false;

            yield return _wait;

            _player.IsTargatable = true;
            HabilityEnded = true;
            _inNetherInstance = false;
            Destroy(_curparticles);
        }
    }
}