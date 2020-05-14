using System.Collections.Generic;
using UnityEngine;

namespace Lantern
{
    public class LanternCapture : MonoBehaviour
    {
        [SerializeField] private GameObject objs = null;

        public LanternBehaviour lantern;
        private Collider _col;
        private List<IEntity> _ignored;
        private IEntity _alreadyCought;

        // Start is called before the first frame update
        private void Awake()
        {
            IAbility[] abs = objs.GetComponents<IAbility>();
            _col = GetComponent<Collider>();
            lantern = new LanternBehaviour(abs);
            _ignored = new List<IEntity>();
        }

        private void OnEnable()
        {
            _ignored.Clear();
            _alreadyCought = null;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("GhostEnemy"))
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
                        ghost.IsTargatable = false;
                        _alreadyCought = ghost;

                        Vector3 vel = -(transform.position -
                            other.transform.position).normalized;

                        vel *= Vector3.Distance(transform.position,
                            other.transform.position) -
                            Vector3.Distance(_col.bounds.max,
                            transform.position) * 100;

                        other.attachedRigidbody.velocity += vel *
                            Time.fixedDeltaTime;

                        if (Vector3.Distance(other.transform.position,
                            transform.position) < 0.1f &&
                            !lantern.Colors[1].HasValue)
                        {
                            lantern.StoreColor(ghost.GColor);

                            Destroy(other.gameObject);
                            _alreadyCought = null;
                        }
                    }
                    else
                    {
                        _ignored.Add(ghost);
                    }
                }
            }
        }
    }
}