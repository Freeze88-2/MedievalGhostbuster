using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lantern;

namespace Lantern
{
    public class Ability : MonoBehaviour, IAbility
    {
        public (GhostColor, GhostColor) AbilityColors
        { get => (GhostColor.Red, GhostColor.Red); }

        public virtual void ActivateAbility()
        {
            Vector3 v = Vector3.zero;
            v.y += 10;
            Debug.Log("Dashing in the 90's");
        }
    }
}