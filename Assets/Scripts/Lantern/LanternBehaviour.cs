using UnityEngine;

namespace Lantern
{
    public class LanternBehaviour
    {
        private (GameObject, IEntity)[] ghosts;

        private GhostColor?[] colors;

        private readonly HabilityManager _habilities;

        public LanternBehaviour(IAbility[] abs)
        {
            colors = new GhostColor?[2];
            ghosts = new (GameObject, IEntity)[2];
            _habilities = new HabilityManager(abs);
        }

        public (GameObject, IEntity)[] GetGhosts() => ghosts;
        public GhostColor?[] GetColors() => colors;

        public void EmptyLantern(bool DestroyGhosts)
        {

            if (!DestroyGhosts)
            {
                if (ghosts[0].Item1 != null)
                {
                    ghosts[0].Item1.SetActive(true);
                    ghosts[0].Item1.transform.localScale = Vector3.one;
                    ghosts[0].Item2.IsTargatable = true;
                }
                if (ghosts[1].Item1 != null)
                {
                    ghosts[1].Item1.SetActive(true);
                    ghosts[1].Item1.transform.localScale = Vector3.one;
                    ghosts[1].Item2.IsTargatable = true;
                }
            }
            else
            {
                if (ghosts[0].Item1 != null)
                {
                    Object.Destroy(ghosts[0].Item1);
                }
                if (ghosts[1].Item1 != null)
                {
                    Object.Destroy(ghosts[1].Item1);
                }
            }
            colors = new GhostColor?[2];
            ghosts = new (GameObject, IEntity)[2];
        }

        public void StoreColor(GhostColor color)
        {
            if (!colors[0].HasValue)
            {
                colors[0] = color;
            }
            else if (colors[0].HasValue)
            {
                colors[1] = color;
            }
        }

        public void StoreGhost(GameObject ghost, IEntity ghostEntity)
        {
            if (ghosts[0].Item1 == null)
            {
                ghosts[0].Item1 = ghost;
                ghosts[0].Item2 = ghostEntity;
            }
            else if (ghosts[0].Item1 != null)
            {
                ghosts[1].Item1 = ghost;
                ghosts[1].Item2 = ghostEntity;
            }
        }

        public IAbility GetAbility()
        {
            if (colors[0].HasValue && colors[1].HasValue)
            {
                return _habilities.GetAbility(colors[0].Value,
                    colors[1].Value);
            }
            return null;
        }
    }
}