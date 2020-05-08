using UnityEngine;

namespace Lantern
{
    public interface IAbility
    {
        (GhostColor, GhostColor) AbilityColors { get; }

        void ActivateAbility();
    }
}
