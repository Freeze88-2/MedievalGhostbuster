/// <summary>
/// Comon methods and properties for an entity in game
/// </summary>
public interface IEntity
{
    // The color of the ghost
    GhostColor GColor { get; }

    // The current hp of the ghost
    float MaxHp { get; }

    // Maximum speed of the entity
    float MaxSpeed { get; }

    // The current hp of the ghost
    float Hp { get; }

    /// <summary>
    /// Subtract the specefied amount of hp from the entity
    /// </summary>
    /// <param name="amount"> The amount of hp to be subtracted </param>
    void DealDamage(float amount);

    /// <summary>
    /// Adds the specefied amount to the hp from the entity
    /// </summary>
    /// <param name="amount"></param>
    void Heal(float amount);
}