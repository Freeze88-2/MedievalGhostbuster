using UnityEngine;

namespace Lantern
{
    public class HabilityManager
    {
        private readonly IAbility[] abilities;

        public HabilityManager(GameObject[] obs)
        {
            abilities = new IAbility[obs.Length];
            for (int i = 0; i < obs.Length; i++)
            {
                abilities[i] = obs[i].GetComponent<IAbility>();
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
            for (int i = 0; i < abilities.Length; i++)
            {
                if (abilities[i].AbilityColors == color)
                {
                    return abilities[i];
                }
            }
            return null;
        }
    }
}