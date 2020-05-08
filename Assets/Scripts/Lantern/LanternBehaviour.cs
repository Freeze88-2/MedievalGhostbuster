using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lantern
{
    public class LanternBehaviour 
    {
        public GhostColor?[] Colors { get; private set; }
        private readonly HabilityManager habilities;

        public LanternBehaviour(GameObject[] obs)
        {
            Colors = new GhostColor?[2];
            habilities = new HabilityManager(obs);
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
            return habilities.GetAbility(Colors[0].Value, Colors[1].Value);
        }
    }
}
