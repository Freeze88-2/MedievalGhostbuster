using UnityEngine;

namespace Lantern.Abilities
{
    public class ThunderDome : MonoBehaviour, IAbility
    {
        private GameObject _player;
        public bool HabilityEnded { get; private set; }

        public (GhostColor, GhostColor) AbilityColors
        {
            get => (GhostColor.Green, GhostColor.Blue);
        }

        public void ActivateAbility()
        {
            Collider[] ghosts = Physics.OverlapSphere
                (_player.transform.position, 3f, LayerMask.GetMask("Entity"));

            for (int i = 0; i < ghosts.Length; i++)
            {
                ghosts[i].gameObject.GetComponent<IEntity>()
                    .DealDamage(float.PositiveInfinity);

                if (i >= 4) break;
            }
            if (ghosts.Length >= 1)
            {
                HabilityEnded = true;
            }
        }

        // Start is called before the first frame update
        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }
    }
}