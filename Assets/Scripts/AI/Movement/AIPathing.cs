using AI.PathFinding;
using AI.PathFinding.GridGeneration;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Movement
{
    /// <summary>
    /// Checks path finding and returns a point
    /// </summary>
    public class AIPathing
    {
        // List of _points found by the path finding
        public List<Vector3> Path { get; private set; }

        // The designated area of the ghost
        private readonly GridGenerator _grid = null;

        // The starting node
        private Node _start;

        // The ending node
        private Node _end;

        // Instance of the path finding algorithm
        private readonly AStarAlgorithm _aStar;

        // Old target give
        private Node _oldTarget;

        // The index to get the point on the list
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
            // Saves the old target not
            _oldTarget = _end;

            // Finds the node where the target is at
            _end = _grid.GetClosestNode(target - _grid.transform.position);

            // Checks if the target changed, if so it runs the path finding
            if (_end != null && _end != _oldTarget)
            {
                // Finds the node where the AI is at
                _start = _grid.GetClosestNode(init - _grid.transform.position);

                // Calculates the points on the path
                Path = _aStar.CalculatePath(_start, _end);

                // Resets the index of the Path List
                index = 0;
            }

            // Checks if the current index can be accessed on the list
            if (Path != null && Path.Count >= index + 1)
            {
                // Creates a temporary point with the next position to move to
                Vector3 point = Path[index];

                // Finds the current node the AI is at 
                Node currentPos = 
                    _grid.GetClosestNode(init - _grid.transform.position);

                // Checks if the AI is at the wanted position
                if (point == currentPos.Position)
                {
                    // Next time gets the next index
                    index++;
                }

                // Returns the wanted position
                return point;
            }
            else
            {
                // Returns no point
                return null;
            }
        }
    }
}