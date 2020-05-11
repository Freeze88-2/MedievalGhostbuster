using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lantern.Abilities
{
    public class MedicKnight : MonoBehaviour , IAbility
    {
        [SerializeField] private float _healAmount = 0f;
        [SerializeField] private float _healTime = 0f;
        [SerializeField] private float _healTicks = 0f;

        private IEntity _player;
        private WaitForSeconds _wait;
        private float _timer;
        private bool _isHealing;
        public (GhostColor, GhostColor) AbilityColors
        {
            get => (GhostColor.Green, GhostColor.Green);
        }

        public bool HabilityEnded { get; private set; }

        public void ActivateAbility()
        {
            if (!_isHealing)
            {
                HabilityEnded = false;
                StartCoroutine(HealPlayer());
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            _isHealing = false;
            HabilityEnded = false;
            _wait = new WaitForSeconds(_healTicks);
            _player = GameObject.FindGameObjectWithTag("Player")
                .GetComponent<IEntity>();
        }
        private IEnumerator HealPlayer()
        {
            _isHealing = true;
            while ((int)_timer <= _healTime)
            {
                // ------- Heal Player ----------
                _player.Heal(_healAmount);
                Debug.Log(_player.Hp);

                yield return _wait;

                _timer += Time.deltaTime % 60;
            }
            _isHealing = false;
            HabilityEnded = true;
        }
    }
}
