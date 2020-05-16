using AI.PathFinding.GridGeneration;
using UnityEngine;

namespace AI.Movement
{
    /// <summary>
    /// Stores most of the information of the ghost state
    /// </summary>
    public class AIEntity : MonoBehaviour, IEntity
    {
        // -- Target given --
        [SerializeField] public GameObject target = null;

        // -- Designated area --
        [SerializeField] protected GameObject area = null;

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
        /// The current speed of the ghost
        /// </summary>
        public float Speed { get; set; }

        /// <summary>
        /// The current hp of the ghost
        /// </summary>
        public float Hp { get; private set; }

        /// <summary>
        /// If this ghost can be targetted
        /// </summary>
        public bool IsTargatable { get; set; }

        public Vector3 Velocity => rb.velocity;

        /// <summary>
        /// The rigidbody attached to this gameobject
        /// </summary>
        protected Rigidbody rb;

        /// <summary>
        /// Assigns the variables from IEntity to the ones given
        /// </summary>
        protected virtual void Start()
        {
            area.GetComponent<GridGenerator>().StartGridGeneration();
            // Gets the rigidbody of this gameobject
            rb = GetComponent<Rigidbody>();
            // Sets the color to the one of the editor
            GColor = _gcolor;
            // Sets the Maximum hp to the one of the editor
            MaxHp = _maxHp;
            // Sets the Maximum speed to the one of the editor
            MaxSpeed = _maxSpeed;
            // Sets the current hp to the one of the editor
            Hp = _hp;
            // Set's if this ghost can perform actions or be performed on
            Speed = _maxSpeed;
            IsTargatable = true;
        }

        /// <summary>
        /// Subtract the specefied amount of hp from the entity
        /// </summary>
        /// <param name="amount"> The amount of hp to be subtracted </param>
        public void DealDamage(float amount)
        {
            // subtracts the amount from the hp
            Hp = Mathf.Max(Hp - amount, 0);

            // Checks if the hp is 0
            if (Hp == 0)
            {
                // Kills the ghost
                Destroy(gameObject);
            }
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
}