using UnityEngine;

namespace Lantern
{
    public class LanternBehaviour
    {
        public (GameObject, IEntity)[] Ghosts { get; private set; }
        public GhostColor?[] Colors { get; private set; }
        private readonly HabilityManager _habilities;

        public LanternBehaviour(IAbility[] abs)
        {
            Colors = new GhostColor?[2];
            Ghosts = new (GameObject, IEntity)[2];
            _habilities = new HabilityManager(abs);
        }

        public void EmptyLantern(bool DestroyGhosts)
        {
            Colors = new GhostColor?[2];

            if (!DestroyGhosts)
            {
                if (Ghosts[0].Item1 != null)
                {
                    Ghosts[0].Item1.SetActive(true);
                    Ghosts[0].Item1.transform.localScale = Vector3.one;
                    Ghosts[0].Item2.IsTargatable = true;
                }
                if (Ghosts[1].Item1 != null)
                {
                    Ghosts[1].Item1.SetActive(true);
                    Ghosts[1].Item1.transform.localScale = Vector3.one;
                    Ghosts[1].Item2.IsTargatable = true;
                }
            }
            else
            {
                if (Ghosts[0].Item1 != null)
                {
                    Object.Destroy(Ghosts[0].Item1);
                }
                if (Ghosts[1].Item1 != null)
                {
                    Object.Destroy(Ghosts[1].Item1);
                }
            }
            Ghosts = new (GameObject, IEntity)[2];
        }

        public void StoreColor(GhostColor color)
        {
            if (!Colors[0].HasValue)
            {
                Colors[0] = color;
            }
            else if (Colors[0].HasValue)
            {
                Colors[1] = color;
            }
        }
        public void StoreGhost(GameObject ghost, IEntity ghostEntity)
        {
            if (Ghosts[0].Item1 == null)
            {
                Ghosts[0].Item1 = ghost;
                Ghosts[0].Item2 = ghostEntity;
            }
            else if (Ghosts[0].Item1 != null)
            {
                Ghosts[1].Item1 = ghost;
                Ghosts[1].Item2 = ghostEntity;
            }
        }
        public IAbility GetAbility()
        {
            if (Colors[0].HasValue && Colors[1].HasValue)
            {
                return _habilities.GetAbility(Colors[0].Value,
                    Colors[1].Value);
            }
            return null;
        }
    }
}