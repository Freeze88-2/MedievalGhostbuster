/// <summary>
/// Comon methods and properties for an entity in game
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Type of color the entity is (mainly for ghosts)
    /// </summary>
    GhostColor GColor { get; }
    /// <summary>
    /// The Maximum health points of the Entity
    /// </summary>
    float MaxHp { get; }
    /// <summary>
    /// The Maximum movement speed of the Entity
    /// </summary>
    float MaxSpeed { get; }
    /// <summary>
    /// The current health points of the Entity
    /// </summary>
    float Hp { get; set; }

    /// <summary>
    /// Used to subtract HP from the entity
    /// </summary>
    /// <param name="amount"> The amount to be subctracted </param>
    void DealDamage(float amount);
    /// <summary>
    /// Used to subtract HP from the entity
    /// </summary>
    /// <param name="amount"> The amount to be added </param>
    void Heal(float amount);
}