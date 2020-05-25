using System.Collections;
using UnityEngine;

namespace Lantern.Abilities
{
    public class TundraPalace : MonoBehaviour, IAbility
    {
        [SerializeField] private float _durationTime = 0f;
        [SerializeField] private float _radius = 15f;
        [SerializeField] private float _freezingRadius = 7f;
        [SerializeField] private GameObject _freezeEffect = null;
        [Tooltip("Sound Effects")]
        [SerializeField] private AudioClip _sound;

        private GameObject _player;
        private WaitForSeconds _wait;
        private bool _isReseting;

        public bool HabilityEnded { get; private set; }

        public (GhostColor, GhostColor) AbilityColors
        {
            get => (GhostColor.Blue, GhostColor.Blue);
        }

        // Start is called before the first frame update
        private void Start()
        {
            _isReseting = false;
            HabilityEnded = false;
            _player = GameObject.FindGameObjectWithTag("Player");
            _wait = new WaitForSeconds(_durationTime);
        }

        public void ActivateAbility()
        {
            if (!_isReseting)
            {
                HabilityEnded = false;

                Collider[] cols = Physics.OverlapSphere(
                    _player.transform.position, _radius,
                    LayerMask.GetMask("Entity"));

                Physics.Raycast(_player.transform.position, 
                    -_player.transform.up, out RaycastHit hit, 100f,
                    LayerMask.GetMask("Default"));

                Instantiate(_freezeEffect, hit.point, Quaternion.identity);

                for (int i = 0; i < cols.Length; i++)
                {
                    IEntity ghost = cols[i].gameObject.GetComponent<IEntity>();

                    if (ghost != null)
                    {
                        if (Vector3.Distance(cols[i].transform.position,
                            _player.transform.position) <= _freezingRadius)
                        {
                            ghost.Speed = 0f;
                        }
                        else
                        {
                            ghost.Speed /= 2;
                        }
                    }
                    else
                    {
                        if (Vector3.Distance(cols[i].transform.position,
                            _player.transform.position) < _freezingRadius)
                        {
                            cols[i].attachedRigidbody.velocity = Vector3.zero;
                        }
                        else
                        {
                            cols[i].attachedRigidbody.velocity /= 2;
                        }
                    }
                }
                StartCoroutine(ResetSpeed(cols));
            }
        }

        public void PlaySound(AudioSource audio)
        {
            audio.clip = _sound;
            audio.Play();
        }

        private IEnumerator ResetSpeed(Collider[] cols)
        {
            _isReseting = true;
            yield return _wait;

            for (int i = 0; i < cols.Length; i++)
            {
                IEntity ghost = cols[i].gameObject.GetComponent<IEntity>();

                if (ghost != null)
                {
                    ghost.Speed = ghost.MaxSpeed;
                }
            }
            _isReseting = false;
            HabilityEnded = true;
        }
    }
}