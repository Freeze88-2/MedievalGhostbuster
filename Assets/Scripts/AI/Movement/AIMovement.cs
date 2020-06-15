/*
 * |---------------------------------------------------|
 * |                                                   |
 * |          André please cleanup this mess!          |
 * |                                                   |
 * |---------------------------------------------------|
 */

using CostumDebug;
using System.Collections;
using UnityEngine;

namespace AI.Movement
{
    /// <summary>
    /// Calculates and applies to the gameobject the movement
    /// </summary>
    public class AIMovement : AIEntity, IDebug
    {
        // Stores the current target
        private Vector3? nextPoint;

        // Provides a point for the AI to move to
        private AIPathing _ailogic;

        // Line for debugging the _path
        private LineRenderer _line;

        // The player entity
        private IEntity _player;

        // The player gameobjet
        private GameObject _playerObj;

        // If the ghost can move on the next fixed update
        private bool _canMove;

        // Stores the value of the velocity and angle
        private SteeringBehaviour _steerBehaviours;

        // Array of all behaviours
        private IBehaviour[] _behaviours;

        // Creates a new AIBrainController
        private AIBrainController _brain;

        // Stores the current target
        private Vector3 target;

        /// <summary>
        /// Use this for initialization
        /// </summary>
        protected override void Start()
        {
            // Calls the base class start
            base.Start();

            // Finds the Player gameobject
            _playerObj = GameObject.FindGameObjectWithTag("Player");

            // Finds the IEntity component of the player
            _player = _playerObj.GetComponent<IEntity>();

            // Creates the AIBrain
            _brain = new AIBrainController(area, gameObject, _playerObj);

            // Creates a new AIPathing passing in the _grid
            _ailogic = new AIPathing(area);


            // Gives a default value to _canMove
            _canMove = false;

            GameObject[] objs = GameObject.FindGameObjectsWithTag("GhostEnemy");

            AIEntity[] ss = new AIEntity[objs.Length];

            for (int i = 0; i < objs.Length; i++)
            {
                ss[i] = objs[i].GetComponent<AIEntity>();
            }

            _behaviours = new IBehaviour[4]
            {
                new AISeek(),
                new AISeparation(ss, 1.2f),
                new AIObstacleAvoidance(ss, 2f),
                new AIRotateToTarget()
            };
        }

        /// <summary>
        /// Called once per frame
        /// </summary>
        private void Update()
        {
            Vector3 oldTarget = target;

            target = _brain.GetDecision();

            // Checks if the area exists and can hit the player
            if (_player.IsTargatable)
            {
                _canMove = true;

                // Stores the next point from AIPathing
                // Gets a vector3 form the path-finding
                nextPoint = _ailogic.GetPoint
                    (gameObject.transform.position, target);


                if (nextPoint.HasValue && target != Vector3.zero)
                {
                    _steerBehaviours = new SteeringBehaviour();

                    for (int i = 0; i < _behaviours.Length; i++)
                    {
                        _steerBehaviours +=
                            _behaviours[i].GetOutput(this, nextPoint.Value);
                    }
                }
                else
                {
                    _canMove = false;
                }
            }
        }

        /// <summary>
        /// This is called every physics update
        /// </summary>
        private void FixedUpdate()
        {
            // If the ghost can move
            if (_canMove)
            {
                // Checks if the ghost has something below
                if (Physics.Raycast(transform.position, -transform.up, 0.1f))
                {
                    rb.velocity = _steerBehaviours.Velocity;

                    transform.rotation = Quaternion.Euler(0f,
                        _steerBehaviours.Angle, 0f);

                    if (rb.velocity.magnitude > Speed)
                    {
                        rb.velocity = rb.velocity.normalized * Speed;
                    }
                }
            }
        }

        /// <summary>
        /// Setups a debug _line in the game
        /// </summary>
        /// <param name="active"> Activate or deactivate the debug </param>
        public void RunDebug(bool active)
        {
            // Stops the drawing of the lines
            StopCoroutine(DebugLine());
            // Destroys the current _line
            Destroy(_line);

            // If its to activate the _line
            if (active)
            {
                // Adds a _line render to the gameobject
                _line = gameObject.AddComponent<LineRenderer>();

                // Creates a sorting layer
                _line.sortingLayerName = "Debug";
                // Sets the sorting layer order to 5
                _line.sortingOrder = 5;
                // Sets the number of positions of _line to 1
                _line.positionCount = 1;
                // Set's the first position to the current position
                _line.SetPosition(0, transform.position);
                // Sets the width of the _line at the _start
                _line.startWidth = 0.05f;
                // Sets the width of the _line at the _end
                _line.endWidth = 0.05f;
                // The _line uses worldspace coordinates
                _line.useWorldSpace = true;

                // Starts the drawing of the _line
                StartCoroutine(DebugLine());
            }
        }

        /// <summary>
        /// Coroutine for drawing the _line everyframe
        /// </summary>
        /// <returns> A _wait _timer </returns>
        private IEnumerator DebugLine()
        {
            // Performs a loop until the coroutine is stopped
            while (true)
            {
                // Sets the number of positions of _line to 1
                _line.positionCount = 1;
                // Set's the first position to the current position
                _line.SetPosition(0, transform.position);

                // Runs through every point found by the pathfinding
                for (int i = 0; i < _ailogic.Path.Count; i++)
                {
                    if (i + 1 < _ailogic.Path.Count)
                    {
                        // Adds a point to the _line
                        _line.positionCount += 1;
                        // Sets the position of that point to a _path point
                        _line.SetPosition(_line.positionCount - 1,
                            _ailogic.Path[i]);
                    }
                }
                // Adds one last point
                _line.positionCount += 1;
                // Sets the position of the point to the position of the target
                _line.SetPosition(_line.positionCount - 1,
                    target);
                // Waits for the _end of the frame
                yield return new WaitForEndOfFrame();
            }
        }

        private void OnDestroy()
        {
            if (_brain.attackingTag)
            {
                _playerObj.GetComponent<DummyPlayer>().NOfGhostsAround -= 1;
            }
        }
    }
}