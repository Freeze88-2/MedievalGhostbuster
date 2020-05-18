using UnityEngine;

public class EssenseWell : MonoBehaviour, IEntity
{
    [SerializeField] private GhostColor color;
    public float MaxHp => 0;

    public float MaxSpeed => 0;

    public float Hp => 0;

    public bool IsTargatable { get; set; }
    public float Speed { get; set; }
    public GhostColor GColor => color;

    public void DealDamage(float amount) {; }

    public void Heal(float amount) {; }
}
