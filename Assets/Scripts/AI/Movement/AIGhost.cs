using UnityEngine;

/// <summary>
/// Stores most of the information of the ghost state
/// </summary>
public class AIGhost : MonoBehaviour, IEntity
{
    // -- Target given --
    [SerializeField] protected GameObject target = null;
    // -- Designated area --
    [SerializeField] protected GameObject area = null;
    // The color of the ghost
    [SerializeField] protected GhostColor gcolor = GhostColor.Blue;
    // Maximum speed of the entity
    [SerializeField] protected float maxSpeed = 1f;
    // The Maximum hp possible
    [SerializeField] protected float maxHp = 100f;
    // The current hp of the ghost
    [SerializeField] protected float hp = 100f;

    // The color of the ghost
    public GhostColor GColor { get; protected set; }
    // The current hp of the ghost
    public float MaxHp { get; protected set; }
    // Maximum speed of the entity
    public float MaxSpeed { get; protected set; }
    // The current hp of the ghost
    public float Hp { get; protected set; }
    // The rigidbody attached to this gameobject
    protected Rigidbody rb;

    /// <summary>
    /// Subtract the specefied amount of hp from the entity
    /// </summary>
    /// <param name="amount"> The amount of hp to be subtracted </param>
    public void DealDamage(float amount)
    {
        // subtracts the amount from the hp
        Hp -= amount;
    }
    /// <summary>
    /// Adds the specefied amount to the hp from the entity
    /// </summary>
    /// <param name="amount"></param>
    public void Heal(float amount)
    {
        // Adds hp to the player capped at the defined max hp
        Hp = Mathf.Min(Hp + amount, MaxHp);
    }
}
