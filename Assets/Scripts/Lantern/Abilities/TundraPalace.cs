using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lantern.Abilities
{
    public class TundraPalace : MonoBehaviour, IAbility
    {
        [SerializeField] private float _durationTime = 0f;

        private GameObject _player;
        private WaitForSeconds _wait;
        private bool _isReseting;

        public (GhostColor, GhostColor) AbilityColors
        {
            get => (GhostColor.Blue, GhostColor.Blue);
        }

        public bool HabilityEnded { get; private set; }

        public void ActivateAbility()
        {
            if (!_isReseting)
            {
                HabilityEnded = false;

                Collider[] cols = Physics.OverlapSphere(
                    _player.transform.position, 100f,
                    LayerMask.GetMask("Enemy"));

                for (int i = 0; i < cols.Length; i++)
                {
                    IEntity ghost = cols[i].gameObject.GetComponent<IEntity>();

                    if (Vector3.Distance(cols[i].transform.position,
                        _player.transform.position) < 5f)
                    {
                        ghost.Speed = 0f;
                    }
                    else
                    {
                        ghost.Speed /= 2;
                    }
                }

                StartCoroutine(ResetSpeed(cols));
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            _isReseting = false;
            HabilityEnded = false;
            _player = GameObject.FindGameObjectWithTag("Player");
            _wait = new WaitForSeconds(_durationTime);
        }

        private IEnumerator ResetSpeed(Collider[] cols)
        {
            _isReseting = true;
            yield return _wait;

            for (int i = 0; i < cols.Length; i++)
            {
                IEntity ghost = cols[i].gameObject.GetComponent<IEntity>();

                ghost.Speed = ghost.MaxSpeed;
            }
            _isReseting = false;
            HabilityEnded = true;
            Debug.Log("I has ended");
        }
    }
}