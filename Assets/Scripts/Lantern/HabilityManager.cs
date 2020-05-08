using UnityEngine;

namespace Lantern
{
    public class HabilityManager
    {
        private readonly IAbility[] _abilities;

        public HabilityManager(GameObject[] obs)
        {
            _abilities = new IAbility[obs.Length];
            for (int i = 0; i < obs.Length; i++)
            {
                _abilities[i] = obs[i].GetComponent<IAbility>();
            }
        }

        public IAbility GetAbility(GhostColor a, GhostColor b)
        {
            if (a == GhostColor.Red && b == GhostColor.Red)
            {
                return SearchWantedAbility((GhostColor.Red, GhostColor.Red));
            }
            return null;
        }

        private IAbility SearchWantedAbility((GhostColor, GhostColor) color)
        {
            for (int i = 0; i < _abilities.Length; i++)
            {
                if (_abilities[i].AbilityColors == color)
                {
                    return _abilities[i];
                }
            }
            return null;
        }
    }
}