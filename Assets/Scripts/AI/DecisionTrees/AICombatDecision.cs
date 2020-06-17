using UnityEngine;

namespace AI.DecisionTrees
{
    public class AICombatDecision
    {
        // Delay timer between attacks
        private int attackDelayTimer;

        // The angle to witch the next circle point should be at
        private float circleAngle;

        // The player script
        private readonly DummyPlayer _player;

        // The current AI
        private readonly GameObject _ai;

        // The Animator of the current AI
        private readonly Animator _anim;

        // The IEntity of the AI
        private readonly IEntity _ghost;

        // If this ghost is attacking
        public bool AttackingTag { get; private set; }

        // The root of the attacking behavior
        public IDecisionTreeNode AttackingNodes { get; }

        /// <summary>
        /// Constructor of this class
        /// </summary>
        /// <param name="player"> The player script </param>
        /// <param name="ai"> The current AI </param>
        /// <param name="anim"> This ghosts animator </param>
        public AICombatDecision(DummyPlayer player,
            GameObject ai, Animator anim)
        {
            // Sets the variables given to the ones created
            _player = player;
            _ai = ai;
            _anim = anim;
            _ghost = ai.GetComponent<IEntity>();

            // Sets the delay timer between attacks to 200
            attackDelayTimer = 100;

            // Creates the 'Leaf' nodes of this behavior
            IDecisionTreeNode circlePlayer = new ActionNode(CirclePlayer);

            IDecisionTreeNode attackPlayer = new ActionNode(Attack);

            IDecisionTreeNode getToPlayer = new ActionNode(GetPlayerPosition);

            // Creates the actual nodes
            IDecisionTreeNode canAttack = new DecisionNode
                (GetPlayerIsNear, attackPlayer, getToPlayer);

            AttackingNodes = new DecisionNode
                (HasSpaceNearPlayer, canAttack, circlePlayer);
        }

        /// <summary>
        /// Checks if the player is in the area
        /// </summary>
        public void PlayerIsInArea()
        {
            // Plays an 'Alerted" animation
            _anim.SetTrigger("Cast Spell");
            // If the ghost can't move, resets to be able to
            _ghost.IsTargatable = true;
        }

        /// <summary>
        /// Checks if the player has space near him
        /// </summary>
        /// <returns> True if has space of is attacking </returns>
        private bool HasSpaceNearPlayer()
        {
            // Checks if it has less than 4 ghosts or already attacked
            bool hasSpace = _player.NOfGhostsAround < 4 || AttackingTag;

            // If it has space
            if (hasSpace)
            {
                // Resets the circleAngle variable to 0
                circleAngle = 0;
            }
            // Returns the result
            return hasSpace;
        }

        /// <summary>
        /// Checks if the player is in attacking range
        /// </summary>
        /// <returns> True if the distance is less than 2 units </returns>
        private bool GetPlayerIsNear()
        {
            // Finds the distance between the current ghost and the player
            bool distanceToPlayer = Vector3.Distance(
                _ai.transform.position, _player.transform.position) <= 2;

            // Checks if it's attacking and the distance is more than 2 units
            if (!distanceToPlayer && AttackingTag)
            {
                // Removes a ghost from the amount of ghosts around the player
                _player.NOfGhostsAround -= 1;
                // Resets the AttackingTag to be false
                AttackingTag = false;
            }
            // Returns the result
            return distanceToPlayer;
        }

        /// <summary>
        /// Attacks the player when nearby
        /// </summary>
        /// <returns> An empty Vector3 </returns>
        private Vector3 Attack()
        {
            // Increments the attack delay by one
            attackDelayTimer++;

            Vector3 dir = _ai.transform.position - _player.transform.position;
            // Resets the value of Y to 0
            dir.y = 0;

            // Rotates gradually the Ghost towards the direction
            _ai.transform.rotation = Quaternion.Lerp(_ai.transform.rotation,
                Quaternion.LookRotation(-dir), Time.deltaTime * 30);

            // Checks if the delay is bigger than 100
            if (attackDelayTimer >= 100)
            {
                // Checks if it hasn't attacked yet
                if (!AttackingTag)
                {
                    // Increments the number of ghosts around the player by one
                    _player.NOfGhostsAround += 1;
                }

                // Sets the AttackingTag to true
                AttackingTag = true;

                // Plays an animation of attacking
                _anim.SetTrigger("Bite Attack");

                // Checks if the player actually exists
                if (_player != null)
                    // Deals damage to the player
                    _player.DealDamage(_ghost.DamageAmount);

                // Resets the attack delay
                attackDelayTimer = 0;
            }
            // Returns an empty Vector3
            return Vector3.zero;
        }

        /// <summary>
        /// Get's the next position in order to rotate around the player
        /// </summary>
        /// <returns> The next point to move to </returns>
        private Vector3 CirclePlayer()
        {
            // Checks if it's the first time circling the player
            if (circleAngle == 0)
            {
                // Gets the angle between the ghost and the player
                circleAngle = Vector3.SignedAngle(_ai.transform.position,
                    _player.transform.position, _ai.transform.up);
            }

            // Checks if the color of the current ghost is red
            if (_ghost.GColor == GhostColor.Red)
            {
                // Increments the angle by a velocity
                circleAngle += Time.deltaTime * 0.425f;
            }
            else
            {
                // Increments the angle by a velocity
                circleAngle += Time.deltaTime * 0.325f;
            }

            // Finds the new X position according to the current angle
            float x = -Mathf.Cos(circleAngle) * 5;
            // Finds the new Z position according to the current angle
            float z = Mathf.Sin(circleAngle) * 5;

            // Creates a new Vector with the a position in a circle
            Vector3 dir = new Vector3(x, 0, z);

            // Returns a position around the player
            return dir + _player.transform.position;
        }

        /// <summary>
        /// Gets the current position of the player
        /// </summary>
        /// <returns> The position of the player </returns>
        private Vector3 GetPlayerPosition() =>
            _player.gameObject.transform.position;
    }
}