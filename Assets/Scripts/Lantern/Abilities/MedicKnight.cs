using System.Collections;
using UnityEngine;

namespace Lantern.Abilities
{
    public class MedicKnight : MonoBehaviour, IAbility
    {
        [SerializeField] public float _healTime { get; set; }
        [SerializeField] private float _healAmount = 10f;
        [SerializeField] private float _healTicks = 0.2f;

        [Tooltip("Sound Effects")]
        [SerializeField] private AudioClip _sound;

        private IEntity _player;
        private WaitForSeconds _wait;
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
            _wait = new WaitForSeconds(_healTicks);
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

                yield return _wait;

                _timer += Time.deltaTime % 60;
            }
            _isHealing = false;
            HabilityEnded = true;
        }
    }
}