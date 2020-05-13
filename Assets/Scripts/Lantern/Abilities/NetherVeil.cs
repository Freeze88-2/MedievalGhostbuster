using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lantern.Abilities
{
    public class NetherVeil : MonoBehaviour, IAbility
    {
        [SerializeField] private float _duration = 6f;

        private WaitForSeconds _wait;
        private IEntity _player;
        private bool _inNetherInstance;

        public (GhostColor, GhostColor) AbilityColors
        {
            get => (GhostColor.Red, GhostColor.Blue);
        }

        public bool HabilityEnded { get; private set; }


        // Start is called before the first frame update
        void Start()
        {

            HabilityEnded = false;
            _wait = new WaitForSeconds(_duration);
            _player = GameObject.FindGameObjectWithTag("Player")
                .GetComponent<IEntity>();
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
                _player.IsTargatable = true;
                HabilityEnded = true;
                _inNetherInstance = false;
            }
        }

        private IEnumerator InNetherInstance()
        {
            _inNetherInstance = true;
            _player.IsTargatable = false;

            yield return _wait;

            _player.IsTargatable = true;
            HabilityEnded = true;
            _inNetherInstance = false;
        }
    }
}
