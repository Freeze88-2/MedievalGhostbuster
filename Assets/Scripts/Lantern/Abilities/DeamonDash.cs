using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lantern.Abilities
{
    public class DeamonDash : MonoBehaviour, IAbility
    {
        [SerializeField] GameObject a;
        public (GhostColor, GhostColor) AbilityColors
        { get => (GhostColor.Red, GhostColor.Red); }

        public void ActivateAbility()
        {
            Vector3 v = Vector3.zero;
            v.y += 10;
            Instantiate(a, v, Quaternion.identity);
            Debug.Log("Dashing in the 90's");
        }

    }
}
