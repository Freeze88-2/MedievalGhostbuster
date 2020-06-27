using System.Collections;
using UnityEngine;

namespace Lantern.Abilities
{
    public class MedicKnight : MonoBehaviour, IAbility
    {
        [SerializeField] private float _healTime = 2;
        [SerializeField] private float _healAmount = 10f;

        [Tooltip("Sound Effects")]
        [SerializeField] private AudioClip _sound;

        private IEntity _player;
        private float _timer;
        private bool _isHealing;

        public (GhostColor, GhostColor) AbilityColors
        {
            get => (GhostColor.Green, GhostColor.Green);
        }

        public bool HabilityEnded { get; private set; }

        public int ID => 2;

        public int NActivations => 1;

        // Start is called before the first frame update
        private void Start()
        {
            _isHealing = false;
            HabilityEnded = false;
            _player = GameObject.FindGameObjectWithTag("Player")
                .GetComponent<IEntity>();
        }

        public void ActivateAbility()
        {
            if (!_isHealing)
            {
                HabilityEnded = false;
                StartCoroutine(HealPlayer());
            }
        }

        public void PlaySound(AudioSource audio)
        {
            audio.clip = _sound;
            audio.Play();
        }

        private IEnumerator HealPlayer()
        {
            _isHealing = true;
            while (_timer <= _healTime)
            {
                // ------- Heal Player ----------
                _player.Heal(_healAmount);

                _timer += Time.deltaTime;

                yield return null;
            }
            _isHealing = false;
            HabilityEnded = true;
        }
    }
}