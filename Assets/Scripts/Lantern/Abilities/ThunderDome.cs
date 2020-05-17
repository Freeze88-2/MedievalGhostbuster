﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Lantern.Abilities
{
    public class ThunderDome : MonoBehaviour, IAbility
    {
        [SerializeField] private GameObject _particles = null;
        [SerializeField] private float _tickTime = 0.2f;

        private List<IEntity> _ghosts;
        private GameObject _player;
        private WaitForSeconds _wait;

        public bool HabilityEnded { get; private set; }

        public (GhostColor, GhostColor) AbilityColors
        {
            get => (GhostColor.Green, GhostColor.Blue);
        }

        // Start is called before the first frame update
        private void Start()
        {
            _wait = new WaitForSeconds(_tickTime);
            _ghosts = new List<IEntity>();
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        public void ActivateAbility()
        {
            Collider[] ghosts = Physics.OverlapSphere
                (_player.transform.position, 3f, LayerMask.GetMask("Entity"));

            for (int i = 0; i < ghosts.Length; i++)
            {
                _ghosts.Add(ghosts[i].gameObject.GetComponent<IEntity>());

                if (i >= 3) break;
            }
            if (ghosts.Length >= 1)
            {
                StartCoroutine(DestroyGhost());
                HabilityEnded = true;
            }
        }

        private IEnumerator DestroyGhost()
        {
            GameObject particles = Instantiate(_particles, _player.transform);

            while (_ghosts.Count > 0)
            {
                for (int i = 0; i < _ghosts.Count; i++)
                {
                    _ghosts[i].IsTargatable = false;
                    _ghosts[i].DealDamage(25);
                    _ghosts.Remove(_ghosts[i]);
                }
                yield return _wait;
            }
            Destroy(particles);
        }
    }
}