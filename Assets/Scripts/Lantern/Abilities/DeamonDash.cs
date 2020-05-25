using System.Collections;
using UnityEngine;

namespace Lantern.Abilities
{
    public class DeamonDash : MonoBehaviour, IAbility
    {
        [SerializeField] private float _dashSpeed = 25f;
        [SerializeField] private int _dashDuration = 10;
        [SerializeField] private int _nOfDahses = 3;
        [Tooltip("Sound Effects")]
        [SerializeField] private AudioClip _sound;

        private CharacterController _playerRb;
        private WaitForSecondsRealtime _wait;
        private int _timer;
        private bool _isDashing;
        private int _dashes;

        public (GhostColor, GhostColor) AbilityColors
        {
            get => (GhostColor.Red, GhostColor.Red);
        }

        public bool HabilityEnded { get; private set; }

        private void Start()
        {
            _playerRb = GameObject.FindGameObjectWithTag("Player")
                .GetComponent<CharacterController>();
            _wait = new WaitForSecondsRealtime(0.5f);
            _dashes = _nOfDahses;
            _timer = 0;
            _isDashing = false;
            HabilityEnded = false;
        }

        public void ActivateAbility()
        {
            HabilityEnded = false;
            if (!_isDashing)
            {
                StartCoroutine(Dash());
            }
            if (_dashes <= 0)
            {
                HabilityEnded = true;
                _isDashing = false;
                _timer = 0;
                _dashes = _nOfDahses;
            }
        }

        public void PlaySound(AudioSource audio)
        {
            audio.clip = _sound;
            audio.Play();
        }

        private IEnumerator Dash()
        {
            _isDashing = true;
            _dashes--;
            while (_timer < _dashDuration)
            {
                _playerRb.Move(_playerRb.transform.forward *
                    _dashSpeed * Time.deltaTime);
                _timer++;
                yield return null;
            }
            yield return _wait;
            _isDashing = false;
            _timer = 0;
        }
    }
}