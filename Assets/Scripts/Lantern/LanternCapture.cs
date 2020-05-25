using System.Collections.Generic;
using UnityEngine;

namespace Lantern
{
    public class LanternCapture : MonoBehaviour
    {
        [SerializeField] private GameObject objs = null;
        [SerializeField] private AudioClip _lanternCaptureSound;

        private AudioSource _lanternCaptureSource;
        public LanternBehaviour lantern;
        private Collider _col;
        private List<IEntity> _ignored;
        private IEntity _alreadyCought;
        private GameObject _currentGhost;

        // Start is called before the first frame update
        private void Awake()
        {
            IAbility[] abs = objs.GetComponents<IAbility>();
            _col = GetComponent<Collider>();
            lantern = new LanternBehaviour(abs);
            _ignored = new List<IEntity>();
            _lanternCaptureSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            _ignored.Clear();
            _alreadyCought = null;
            _currentGhost = null;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("GhostEnemy"))
            {
                CaptureGhost(other);
            }
            else if (other.CompareTag("EssenceWell"))
            {
                CaptureEssence(other);
            }
        }
        private void CaptureGhost(Collider other)
        {
            IEntity ghost = other.gameObject.GetComponent<IEntity>();

            if (_alreadyCought != null && ghost != _alreadyCought)
            {
                _ignored.Add(ghost);
            }
            if (!_ignored.Contains(ghost))
            {
                float catchResistence = (ghost.MaxHp - ghost.Hp) + 40;

                int captureChance = Random.Range(0, 100);

                if (captureChance < catchResistence
                    && !lantern.Colors[1].HasValue
                    || ghost == _alreadyCought)
                {
                    if (_currentGhost == null)
                    {
                        _currentGhost = other.gameObject;
                        ghost.IsTargatable = false;
                        _alreadyCought = ghost;
                    }

                    Vector3 vel = -(transform.position -
                        other.transform.position).normalized;

                    vel *= Vector3.Distance(transform.position,
                        other.transform.position) -
                        Vector3.Distance(_col.bounds.max,
                        transform.position) * 70;

                    other.attachedRigidbody.velocity += vel *
                        Time.fixedDeltaTime;

                    _currentGhost.transform.localScale *= 0.8f;

                    if (Vector3.Distance(other.transform.position,
                        transform.position) < 0.5f &&
                        !lantern.Colors[1].HasValue)
                    {
                        lantern.StoreColor(ghost.GColor);
                        
                        CaptureGhostSound(_lanternCaptureSource);

                        Destroy(other.gameObject);
                    }
                }
                else
                {
                    _ignored.Add(ghost);
                }
            }
        }
        private void CaptureEssence(Collider other)
        {
            if (_alreadyCought == null)
            {
                IEntity structure = other.gameObject.GetComponent<IEntity>();
                lantern.StoreColor(structure.GColor);
                _alreadyCought = structure;
            }
        }

        private void CaptureGhostSound(AudioSource _lanternCaptureSource)
        {
            _lanternCaptureSource.clip = _lanternCaptureSound;
            _lanternCaptureSource.volume = Random.Range(0.6f, 0.8f);
            _lanternCaptureSource.pitch = Random.Range(0.8f, 1f);
            _lanternCaptureSource.Play();
        }
    }
}