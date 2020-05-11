using UnityEngine;

namespace Lantern
{
    public class LanternBehaviour
    {
        public GhostColor?[] Colors { get; private set; }
        private readonly HabilityManager _habilities;

        public LanternBehaviour(IAbility[] abs)
        {
            Colors = new GhostColor?[2];
            _habilities = new HabilityManager(abs);
        }

        public void EmptyLantern()
        {
            Colors = new GhostColor?[2];
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

        public void ShowColorsIn()
        {
            Debug.Log($"{Colors[0]} and {Colors[1]}");
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