using AI.PathFinding;
using AI.PathFinding.GridGeneration;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Movement
{
    /// <summary>
    /// Checks pathfinding and returns a point
    /// </summary>
    public class AIPathing
    {
        /// <summary>
        /// List of _points found by the pathfinding
        /// </summary>
        public List<Vector3> Path { get; private set; }

        // The designated area of the ghost
        private readonly GridGenerator _grid = null;

        // The starting node
        private Node _start;

        // The ending node
        private Node _end;

        // Instance of the pathfinding algorithm
        private readonly AStarAlgorithm _aStar;

        // Old target give
        private Node _oldTarget;

        private int index = 0;

        /// <summary>
        /// Creates a new AIPathing for the AI to manage the target and position
        /// </summary>
        /// <param name="_grid"> The area of this AI </param>
        public AIPathing(GridGenerator _grid)
        {
            // Creates a new list of positions
            Path = new List<Vector3>();
            // Assigns the given grid to the one of this class
            this._grid = _grid;
            // Creates a new A* algorithm
            _aStar = new AStarAlgorithm();
        }

        /// <summary>
        /// Returns the next point of the path
        /// </summary>
        /// <param name="init"> Initial position of the ghost </param>
        /// <param name="target"> Destination of the ghost </param>
        /// <returns> The next position to move to </returns>
        public Vector3? GetPoint(Vector3 init, Vector3 target)
        {
            _oldTarget = _end;


            _end = _grid.GetClosestNode(target - _grid.transform.position);

            if (_end != null && _end != _oldTarget)
            {
                _start = _grid.GetClosestNode(init - _grid.transform.position);

                Path = _aStar.CalculatePath(_start, _end);
                index = 0;
            }

            if (Path != null && Path.Count >= index + 1)
            {
                Vector3 point = Path[index];

                Node currentPos = _grid.GetClosestNode(init - _grid.transform.position);

                if (point == currentPos.Position)
                    index++;

                return point;
            }
            else
            {
                return null;
            }
        }
    }
}