using UnityEngine;

namespace Lantern
{
    public interface IAbility
    {
        bool HabilityEnded { get; }
        (GhostColor, GhostColor) AbilityColors { get; }
        int ID { get; }
        int NActivations { get; }

        void ActivateAbility();
        void PlaySound(AudioSource audio);
    }
}