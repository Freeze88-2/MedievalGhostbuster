using AI.DecisionTrees;
using AI.PathFinding.GridGeneration;
using System.Collections;
using UnityEngine;

namespace AI.Movement
{
    /// <summary>
    /// Stores most of the information of the ghost state
    /// </summary>
    public class AIEntity : MonoBehaviour, IEntity
    {
        // Audio to be player on death
        [SerializeField] private AudioClip _deathSound = null;

        // The color of the ghost
        [SerializeField] private GhostColor _gcolor = GhostColor.Blue;

        // Maximum speed of the entity
        [SerializeField] private float _maxSpeed = 1f;

        // The Maximum hp possible
        [SerializeField] private float _maxHp = 100f;

        // The current hp of the ghost
        [SerializeField] private float _hp = 100f;

        // The current hp of the ghost
        [SerializeField] private float _damageAmount = 1f;

        // Respective AudioSource
        private AudioSource _audio;

        // Respective Animator
        private Animator _anim;

        // Designated area
        protected GridGenerator area;

        // Creates a new AIBrainController
        protected AIBrainController _brain;

        // The gameObject of the player;
        protected DummyPlayer _playerScript;

        /// <summary>
        /// The color of the ghost
        /// </summary>
        public GhostColor GColor => _gcolor;

        /// <summary>
        /// The current hp of the ghost
        /// </summary>
        public float MaxHp => _maxHp;

        /// <summary>
        /// Maximum speed of the entity
        /// </summary>
        public float MaxSpeed => _maxSpeed;

        /// <summary>
        /// The amount of damage the ghost should deal
        /// </summary>
        public float DamageAmount => _damageAmount;

        /// <summary>
        /// The current speed of the ghost
        /// </summary>
        public float Speed { get; set; }

        /// <summary>
        /// The current hp of the ghost
        /// </summary>
        public float Hp { get; private set; }

        /// <summary>
        /// If this ghost can be targeted
        /// </summary>
        public bool IsTargatable { get; set; }

        /// <summary>
        /// The current velocity of this AI
        /// </summary>
        public Vector3 Velocity => rb.velocity;

        /// <summary>
        /// The <see cref="Rigidbody"/> attached to this game object
        /// </summary>
        protected Rigidbody rb;

        /// <summary>
        /// Assigns the variables from IEntity to the ones given
        /// </summary>
        protected virtual void Start()
        {
            GetArea();
            _playerScript = GameObject.FindGameObjectWithTag("Player").
                GetComponent<DummyPlayer>();
            // Gets the animator of the AI
            _anim = GetComponent<Animator>();
            // Gets the RigidBody of this game object
            rb = GetComponent<Rigidbody>();
            // The audio source of the object
            _audio = GetComponent<AudioSource>();
            // Sets the current hp to the one of the editor
            Hp = _hp;
            // Set's if this ghost can perform actions or be performed on
            Speed = _maxSpeed;
            // If the Entity can move
            IsTargatable = true;
            // Creates the AIBrain
            _brain = new AIBrainController
                (area, gameObject, _playerScript, _anim);
        }

        /// <summary>
        /// Uses the animator to change between idle and "Fly Forward"
        /// </summary>
        /// <param name="walking"> Bool if it should be set or not </param>
        protected void SetAnimation(bool walking)
        {
            // Sets the animator bool to the one given
            _anim.SetBool("Fly Forward", walking);
        }

        /// <summary>
        /// Finds the area the ghost is in
        /// </summary>
        private void GetArea()
        {
            // Searches for the colliders on a 5 unit radius
            Collider[] col = Physics.OverlapSphere(transform.position, 5);

            // Searches through all the colliders
            for (int i = 0; i < col.Length; i++)
            {
                // Checks if that collider is a "GhostArea"
                if (col[i].gameObject.CompareTag("GhostArea"))
                {
                    // Gets the GridGenerator of that component
                    area = col[i].gameObject.GetComponent<GridGenerator>();
                    // Exits the loop
                    break;
                }
            }
        }

        /// <summary>
        /// Subtract the specified amount of hp from the entity
        /// </summary>
        /// <param name="amount"> The amount of hp to be subtracted </param>
        public void DealDamage(float amount)
        {
            // subtracts the amount from the hp
            Hp = Mathf.Max(Hp - amount, 0);

            // Checks if the hp is 0
            if (Hp <= 0)
            {
                // Sets the animation to of the ghost to die
                _anim.SetTrigger("Die");

                // Plays the death sound
                _audio.clip = _deathSound;
                _audio.volume = Random.Range(0.5f, 1.0f);
                _audio.pitch = Random.Range(0.5f, 1.0f);
                _audio.Play();

                // Doesn't allow the ghost to move
                IsTargatable = false;

                // Checks if the AI is trying to attack the player
                if (_brain.AttackingTag)
                {
                    // Removes one from the number of ghosts around the player
                    _playerScript.NOfGhostsAround -= 1;
                }

                // Kills the ghost
                StartCoroutine(KillGhost());
            }
            else
            {
                // Plays the damage animation
                _anim.SetTrigger("Take Damage");
            }
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

        /// <summary>
        /// Coroutine to check if the ghost sound as done playing on not
        /// </summary>
        /// <returns></returns>
        private IEnumerator KillGhost()
        {
            // Checks if the audio is still playing
            while (_audio.isPlaying)
            {
                yield return null;
            }
            // When it stops destroys the ghost
            Destroy(gameObject);
        }
    }
}