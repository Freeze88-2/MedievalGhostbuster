using UnityEngine;

namespace AI.DecisionTrees
{
    /// <summary>
    /// Creates the normal behavior part of the decision trees, holding the
    /// methods to do so
    /// </summary>
    public class AINormalDecision
    {
        // The size of the ghost area on X
        private readonly float x;

        // The size of the ghost area on Z
        private readonly float z;

        // The position of the area
        private readonly Vector3 _areaPos;

        // The AI game object
        private readonly GameObject _ai;

        // The AI this ghost is interacting with
        private GameObject _choosenGhost;

        // The object this ghost is interacting with
        private GameObject _choosenObj;

        // Animator of the ghost
        private readonly Animator _anim;

        // The entity of the chosen ghost
        private IEntity _ghost;

        // If it already interacted with something
        private bool _firstTimeInteracting;

        // The root of the Normal behavior
        public IDecisionTreeNode NormalBehaviour { get; }

        /// <summary>
        /// Constructor of this class
        /// </summary>
        /// <param name="ai"> The current AI </param>
        /// <param name="x"> X size of the area </param>
        /// <param name="z"> Z size of the area </param>
        /// <param name="area"> Position of the area </param>
        public AINormalDecision(GameObject ai, float x, float z,
            Vector3 area, Animator anim)
        {
            // Sets the variables given to ones on this class
            this.x = x;
            this.z = z;
            _ai = ai;
            _areaPos = area;
            _anim = anim;
            _firstTimeInteracting = true;

            // Creates the 'Leaf' nodes of this behavior
            IDecisionTreeNode freeRoam = new ActionNode(FreeRoam);

            IDecisionTreeNode getObjectPos = new ActionNode(ObjectInteraction);

            IDecisionTreeNode getGhostPos = new ActionNode(GhostInteraction);

            IDecisionTreeNode objInt = new ActionNode(InteractWithObject);

            IDecisionTreeNode ghostInt = new ActionNode(InteractWithGhost);

            // Creates the actual nodes
            IDecisionTreeNode ghostIntNode = new DecisionNode
                (IsNearGhost, ghostInt, getGhostPos);

            IDecisionTreeNode objIntNode = new DecisionNode
                (IsNearObject, objInt, getObjectPos);

            IDecisionTreeNode interactionNodes = new DecisionNode
                (ConditionalRandomDecision, objIntNode, ghostIntNode);

            NormalBehaviour = new DecisionNode
                (RandomBinaryDecision, freeRoam, interactionNodes);
        }

        /// <summary>
        /// If the ghost is interacting with something updates the rotation
        /// of the ghost to face the target object
        /// </summary>
        public void UpdateRotation()
        {
            // Checks if either the ghost, object or is not the first time
            // interacting
            if ((_choosenGhost != null || _choosenObj != null)
                && !_firstTimeInteracting)
            {
                // Sets the vector to turn to the object that is not null
                Vector3 intPos = _choosenObj == null ?
                    _choosenGhost.transform.position :
                    _choosenObj.transform.position;

                // Gets the direction it should turn to
                Vector3 dir = _ai.transform.position - intPos;

                // Resets the value of Y to 0
                dir.y = 0;

                // Rotates gradually the Ghost towards the direction
                _ai.transform.rotation =
                    Quaternion.Lerp(_ai.transform.rotation,
                    Quaternion.LookRotation(-dir), Time.deltaTime * 30);

                // Checks if it is the ghost that is not null
                if (_choosenGhost != null)
                {
                    Vector3 dir2 = intPos - _ai.transform.position;
                    // Resets the value of Y to 0
                    dir2.y = 0;

                    // Rotates gradually the Ghost towards the direction
                    _choosenGhost.transform.rotation =
                        Quaternion.Lerp(_choosenGhost.transform.rotation,
                        Quaternion.LookRotation(-dir2), Time.deltaTime * 30);
                }
            }
        }

        /// <summary>
        /// Returns true if the random value (0, 1) is less than 0.5 and
        /// false otherwise
        /// </summary>
        /// <returns></returns>
        private bool RandomBinaryDecision()
        {
            if (_choosenObj != null || _choosenGhost != null)
            {
                return false;
            }
            // Returns true if the random value (0, 1) is less than 0.5 and
            // false otherwise
            return Random.value > 0.5f ? true : false;
        }

        /// <summary>
        /// If it's not interacting with anything chooses randomly, otherwise
        /// returns true if interacting with and object and false if with ghost
        /// </summary>
        /// <returns> bool depending on the interaction </returns>
        private bool ConditionalRandomDecision()
        {
            // Checks if the interaction object exist
            if (_choosenObj != null)
            {
                return true;
            }
            // Checks if the interaction ghost exist
            else if (_choosenGhost != null)
            {
                return false;
            }
            else
            {
                return RandomBinaryDecision();
            }
        }

        /// <summary>
        /// Checks if the current ghost is near the wanted ghost
        /// </summary>
        /// <returns> a bool </returns>
        private bool IsNearGhost()
        {
            // Returns true if the ghost exists and is less than 3 units away
            return _choosenGhost != null &&
                Vector3.Distance(_ai.transform.position,
                _choosenGhost.transform.position) <= 3f ? true : false;
        }

        /// <summary>
        /// Checks if the current ghost is near the wanted object
        /// </summary>
        /// <returns> a bool </returns>
        private bool IsNearObject()
        {
            return _choosenObj != null &&
                Vector3.Distance(_ai.transform.position,
                _choosenObj.transform.position) <= 3f ? true : false;
        }

        /// <summary>
        /// Creates a random position inside the area
        /// </summary>
        /// <returns> A random position </returns>
        private Vector3 FreeRoam()
        {
            // Creates a random X position inside the bounds of the area
            float rndX = Random.Range(-(x - 1), x) + _areaPos.x;

            // Creates a random Z position inside the bounds of the area
            float rndZ = Random.Range(-(z - 1), z) + _areaPos.z;

            // Returns a new Vector 3 with the created X and Z
            return new Vector3(rndX, 0, rndZ);
        }

        /// <summary>
        /// Chooses an object around the ghost to interact with
        /// </summary>
        /// <returns> The position of the object </returns>
        private Vector3 ObjectInteraction()
        {
            // Checks if the object exists already
            if (_choosenObj == null)
            {
                // Searches on a 5 unit radius for interactables
                Collider[] col = Physics.OverlapSphere(_ai.transform.position,
                    5f, LayerMask.GetMask("Interactable"));

                // If there's no objects free roams
                if (col.Length <= 0)
                {
                    return FreeRoam();
                }

                // Gets a random object from the col array
                Collider choosenCol = col[Random.Range(0, col.Length -1)];

                // Assigns the _currentObj to the object chosen
                _choosenObj = choosenCol.gameObject;
            }
            // Returns the position of the object
            return _choosenObj.transform.position;
        }

        /// <summary>
        /// Finds a ghost to interact with if it doesn't free roams
        /// </summary>
        /// <returns></returns>
        private Vector3 GhostInteraction()
        {
            // Checks if the target ghost exists
            if (_choosenGhost == null)
            {
                // Finds all entities in a 5 unit radius
                Collider[] col = Physics.OverlapSphere(_ai.transform.position,
                    5f, LayerMask.GetMask("Entity"));

                // Cycles through all the colliders found
                for (int i = 0; i < col.Length; i++)
                {
                    // Checks if that that collider is a ghost
                    if (col[i].CompareTag("GhostEnemy"))
                    {
                        // Assigns the _ghost to the entity of found
                        _ghost = col[i].gameObject.GetComponent<IEntity>();

                        // If the ghost can't move or is itself
                        if (!_ghost.IsTargatable || col[i].gameObject == _ai)
                        {
                            // Sets the _ghost to null and proceeds to the next
                            _ghost = null;
                            continue;
                        }

                        // Assigns the target ghost to the game object found
                        _choosenGhost = col[i].gameObject;

                        // Returns the position of the target ghost
                        return _choosenGhost.transform.position;
                    }
                }
                // If it doesn't find any free roams
                return FreeRoam();
            }
            // If it does returns the current ghost position
            return _choosenGhost.transform.position;
        }

        /// <summary>
        /// Finds a object to interact with if it doesn't free roams
        /// </summary>
        /// <returns> The target position </returns>
        private Vector3 InteractWithGhost()
        {
            // Checks if it's the first time interacting
            if (_firstTimeInteracting)
            {
                // Changes both ghosts animation
                _anim.SetTrigger("Defend");
                _choosenGhost.GetComponent<Animator>().SetTrigger("Defend");
            }

            // Checks if it already interacted or is the first time
            InteractionLogic();

            // Stops the target ghost from being able to move
            _ghost.IsTargatable = _firstTimeInteracting;

            // Returns a Vector3 with all 0s
            return Vector3.zero;
        }

        private Vector3 InteractWithObject()
        {
            if (_firstTimeInteracting)
            {
                Vector3 q = _choosenObj.transform.rotation.eulerAngles;

                _choosenObj.transform.rotation = 
                    Quaternion.Euler(q.x + 90f, q.y, q.z);
            }

            // Checks if it already interacted or is the first time
            InteractionLogic();

            // Returns a Vector3 with all 0s
            return Vector3.zero;
        }

        /// <summary>
        /// Checks if it already interacted or is the first time
        /// </summary>
        private void InteractionLogic()
        {
            // Checks if it has interacted yet
            if (_firstTimeInteracting)
            {
                // Sets the first time interacting to false
                _firstTimeInteracting = false;
            }
            else if (!_firstTimeInteracting)
            {
                // Resets the target ghost and object to null
                _choosenObj = null;
                _choosenGhost = null;

                // Resets the first time interacting
                _firstTimeInteracting = true;
            }
        }
    }
}