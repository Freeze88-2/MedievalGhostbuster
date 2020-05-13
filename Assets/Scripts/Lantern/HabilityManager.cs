using UnityEngine;

namespace Lantern
{
    public class HabilityManager
    {
        private readonly IAbility[] _abilities;

        public HabilityManager(IAbility[] abs)
        {
            _abilities = abs;
        }

        public IAbility GetAbility(GhostColor a, GhostColor b)
        {
            if (a == GhostColor.Red && b == GhostColor.Red)
            {
                return SearchWantedAbility((GhostColor.Red, GhostColor.Red));
            }
            else if (a == GhostColor.Green && b == GhostColor.Green)
            {
                return SearchWantedAbility((GhostColor.Green, GhostColor.Green));
            }
            else if (a == GhostColor.Blue && b == GhostColor.Blue)
            {
                return SearchWantedAbility((GhostColor.Blue, GhostColor.Blue));
            }
            else if ((a == GhostColor.Red && b == GhostColor.Green) || (a == GhostColor.Green && b == GhostColor.Red))
            {
                return SearchWantedAbility((GhostColor.Red, GhostColor.Green));
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