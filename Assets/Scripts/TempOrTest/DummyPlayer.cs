using UnityEngine;

/// <summary>
/// Temporary Player to test mechanics
/// </summary>
//[System.Obsolete("DummyPlayer is obsolete, use a player script instead")]
public class DummyPlayer : MonoBehaviour, IEntity
{
    // The color of the ghost
    [SerializeField] private GhostColor _gcolor = GhostColor.Blue;

    // Maximum speed of the entity
    [SerializeField] private float _maxSpeed = 1f;

    // The Maximum hp possible
    [SerializeField] private float _maxHp = 100f;

    // The current hp of the ghost
    [SerializeField] private float _hp = 100f;

    /// <summary>
    /// The color of the ghost
    /// </summary>
    public GhostColor GColor { get; private set; }

    /// <summary>
    /// The current hp of the ghost
    /// </summary>
    public float MaxHp { get; private set; }

    /// <summary>
    /// Maximum speed of the entity
    /// </summary>
    public float MaxSpeed { get; private set; }

    /// <summary>
    /// The current speed of the player
    /// </summary>
    public float Speed { get; set; }

    /// <summary>
    /// The current hp of the ghost
    /// </summary>
    public float Hp { get; private set; }

    /// <summary>
    /// The amount of damage the ghost should deal
    /// </summary>
    public float DamageAmount { get; private set; }

    /// <summary>
    /// If this player can be targeted
    /// </summary>
    public bool IsTargatable { get; set; }

    public int NOfGhostsAround { get; set; }

    /// <summary>
    /// The rigid body attached to this game object
    /// </summary>
    protected Rigidbody rb;

    /// <summary>
    /// Assigns the variables from IEntity to the ones given
    /// </summary>
    private void Start()
    {
        // Gets the rigid-body of this game object
        rb = GetComponent<Rigidbody>();
        // Sets the color to the one of the editor
        GColor = _gcolor;
        // Sets the Maximum hp to the one of the editor
        MaxHp = _maxHp;
        // Sets the Maximum speed to the one of the editor
        MaxSpeed = _maxSpeed;
        // Sets the current hp to the one of the editor
        Hp = _hp;

        Speed = MaxSpeed;
        IsTargatable = true;
    }

    /// <summary>
    /// Subtract the specified amount of hp from the entity
    /// </summary>
    /// <param name="amount"> The amount of hp to be subtracted </param>
    public void DealDamage(float amount)
    {
        // subtracts the amount from the hp
        Hp = Mathf.Max(Hp - amount, 0);
        Debug.Log($"{Hp} / {MaxHp}");
    }

    /// <summary>
    /// Adds the specified amount to the hp from the entity
    /// </summary>
    /// <param name="amount"></param>
    public void Heal(float amount)
    {
        // Adds hp to the player capped at the defined max hp
        Hp = Mathf.Min(Hp + amount, MaxHp);
    }
}