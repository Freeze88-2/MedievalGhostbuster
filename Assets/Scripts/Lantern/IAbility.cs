namespace Lantern
{
    public interface IAbility
    {
        bool HabilityEnded { get; }
        (GhostColor, GhostColor) AbilityColors { get; }

        void ActivateAbility();
    }
}