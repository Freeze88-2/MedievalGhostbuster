using AI.PathFinding.GridGeneration;
using UnityEngine;

namespace AI.DecisionTrees
{
    /// <summary>
    /// Builds the various behaviors and controls the calling of the
    /// </summary>
    public class AIBrainController
    {
        // Stores the area the ghost is at
        private readonly GridGenerator _area;

        // Stores the combat part of the decision trees
        private readonly AICombatDecision combat;

        // Stores the normal behavior part of the decision trees
        private readonly AINormalDecision normal;

        // Stores the last found point from the decision trees
        private Vector3 _desiredPos;

        // Stores if in the previous frame the player was inside the area
        private bool _wasPlayerInArea;

        // The amount of time between normal behavior
        private int _counter;

        // Random time to perform normal decisions
        private readonly int _rndTimeForDecision;

        // Returns if the ghost is attacking
        public bool AttackingTag => combat.AttackingTag;

        // The root of the whole decision tree
        private readonly IDecisionTreeNode root;

        /// <summary>
        /// Constructor of the AIBrainController class
        /// </summary>
        /// <param name="area"> The area the ghost is in </param>
        /// <param name="ai"> The current AI </param>
        /// <param name="player"> The target player </param>
        /// <param name="anim"> The ghost animator </param>
        public AIBrainController(GridGenerator area, GameObject ai,
            DummyPlayer player, Animator anim)
        {
            // Sets the given area to the one created
            _area = area;

            // Initializes variables
            _rndTimeForDecision = Random.Range(190, 200);
            _desiredPos = Vector3.zero;

            // Creates the normal and the combat behavior of the decision tree
            combat = new AICombatDecision(player, ai, anim);
            normal = new AINormalDecision(ai, area.areaSize.x, area.areaSize.z,
                area.transform.position, anim);

            // Creates the root of the decision tree
            root = new DecisionNode(GetDesiredBehaviour, combat.AttackingNodes,
                normal.NormalBehaviour);
        }

        /// <summary>
        /// Gets the a bool depending on the player being inside of the area
        /// </summary>
        /// <returns> True if the player is in the area </returns>
        private bool GetDesiredBehaviour()
        {
            // Checks if it's the player wasn't in the area before and now is
            if (!_wasPlayerInArea && _area.PlayerIsInside)
            {
                // Sets the player was in the area bool to true
                _wasPlayerInArea = true;
                // Updates the animation and other variables
                combat.PlayerIsInArea();
            }
            if (!_area.PlayerIsInside)
            {
                // Sets the player was in the area bool to false
                _wasPlayerInArea = false;
                // Updates the rotation of the ghost
                normal.UpdateRotation();
            }
            // Returns if the player is in the area or not
            return _area.PlayerIsInside;
        }

        /// <summary>
        /// Returns the position found by the behavior tree
        /// </summary>
        /// <returns> The target position </returns>
        public Vector3 GetDecision()
        {
            // Increments the counter by one
            _counter++;

            // Checks the player position and if the counter is at the limit
            if (_counter >= _rndTimeForDecision || GetDesiredBehaviour())
            {
                // Creates a new ActionNode, calls the MakeDecision of the root
                ActionNode act = root.MakeDecision() as ActionNode;
                // Sets the desired position to the one given by the tree
                _desiredPos = act.Execute();
                // resets the counter
                _counter = 0;
            }
            // Returns the stored desired position
            return _desiredPos;
        }
    }
}