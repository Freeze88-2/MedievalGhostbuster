/// <summary>
/// Common methods and properties for an entity in game
/// </summary>
public interface IEntity
{
    /// <summary>
    /// The current hp of the ghost
    /// </summary>
    float MaxHp { get; }

    /// <summary>
    /// Maximum speed of the entity
    /// </summary>
    float MaxSpeed { get; }

    /// <summary>
    /// The current hp of the ghost
    /// </summary>
    float Hp { get; }

    /// <summary>
    /// The current speed of the entity
    /// </summary>
    float Speed { get; set; }

    /// <summary>
    /// The amount of damage the ghost should deal
    /// </summary>
    float DamageAmount { get; }

    /// <summary>
    /// If this Entity can be targetted
    /// </summary>
    bool IsTargatable { get; set; }

    /// <summary>
    /// The color of the ghost
    /// </summary>
    GhostColor GColor { get; }

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