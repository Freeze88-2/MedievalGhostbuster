using AI.PathFinding.GridGeneration;
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
        // Provides a point for the AI to move to
        private AIPathing _ailogic;

        // Line for debugging the _path
        private LineRenderer _line;

        // The player entity
        private IEntity _player;

        // If the ghost can move on the next fixed update
        private bool _canMove;

        private SteeringBehaviour vel;

        private IBehaviour[] bevs;

        /// <summary>
        /// Use this for initialization
        /// </summary>
        protected override void Start()
        {
            // Calls the base class start
            base.Start();
            // Finds the IEntity component of the player
            _player = target.GetComponent<IEntity>();
            // Gives a default value to _canMove
            _canMove = false;

            GameObject[] objs = GameObject.FindGameObjectsWithTag("GhostEnemy");
            AIEntity[] ss = new AIEntity[objs.Length];

            for (int i = 0; i < objs.Length; i++)
            {
                ss[i] = objs[i].GetComponent<AIEntity>();
            }

            bevs = new IBehaviour[4] {
                new AISeek(),
                new AISeparation(ss, 2.2f),
                new AIObstacleAvoidance(ss),
                new AIRotateToTarget()};

            // Checks if the area exists
            if (area != null)
            {
                // Creates a new AIPathing passing in the _grid
                _ailogic = new AIPathing(area.GetComponent<GridGenerator>());
            }
        }

        /// <summary>
        /// Called once per frame
        /// </summary>
        private void Update()
        {
            // Stores the next point from AIPathing
            Vector3? nextPoint = null;

            float distanceToTarget = Vector3.Distance(transform.position,
                target.transform.position);

            // Checks if the target exists and the distance is less than 2.5
            if (target != null && distanceToTarget < 2.3f && 
                distanceToTarget > 1.8f)
            {
                // Attacks the target
                Attack();
                // Stops the ghost from moving
                _canMove = false;
            }
            // Checks if the area exists and can hit the player
            else if (area != null && _player.IsTargatable)
            {
                _canMove = true;

                // Gets a vector3 form the pathfinding
                nextPoint = _ailogic.GetPoint(gameObject.transform.position,
                    target.transform.position);

                if (nextPoint.HasValue)
                {
                    vel = new SteeringBehaviour();

                    for (int i = 0; i < bevs.Length; i++)
                    {
                        vel += bevs[i].GetOutput(this, i == 0 ?
                           nextPoint.Value : Vector3.zero);
                    }
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
                    rb.AddForce(vel.Velocity);
                    transform.rotation = Quaternion.Euler(0f, vel.Angle, 0f);

                    if (rb.velocity.magnitude > Speed)
                    {
                        rb.velocity = rb.velocity.normalized * Speed;
                    }
                }
            }
        }

        //--------------------------------------------------------------//
        //                          Temporary                           //
        //--------------------------------------------------------------//
        private void Attack()
        {
            Vector3 dir = target.transform.position - transform.position;
            // Resets the value of Y to 0
            dir.y = 0;

            // Rotates gradually the Ghost towards the direction
            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation(dir), Time.deltaTime *
                MaxSpeed * 6f);

            IEntity player = target.GetComponent<IEntity>();

            if (player != null)
                player.DealDamage(1f);
        }

        /// <summary>
        /// Setups a debug _line on the game
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
                    target.transform.position);
                // Waits for the _end of the frame
                yield return new WaitForEndOfFrame();
            }
        }
    }
}