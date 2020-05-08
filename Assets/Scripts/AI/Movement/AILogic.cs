using AI.PathFinding;
using AI.PathFinding.GridGeneration;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Movement
{
    /// <summary>
    /// Checks pathfinding and returns a point
    /// </summary>
    public class AILogic
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

        // Unique ID of the ghost
        private readonly int _iD;

        /// <summary>
        /// Consctructor the AILogic
        /// </summary>
        /// <param name="_grid"> The area of this AI </param>
        public AILogic(GridGenerator _grid)
        {
            Path = new List<Vector3>();
            this._grid = _grid;
            _aStar = new AStarAlgorithm();
            _iD = System.DateTime.Now.Millisecond + Random.Range(0, 10000);
        }

        public Vector3? GetPoint(GameObject init, GameObject target)
        {
            SetGhostMinDistance(false);

            _start = _grid.GetClosestNode(init.transform.position);

            _end = _grid.GetClosestNode(target.transform.position);

            if (_start != null && _end != null)
            {
                _start.GhostID = _iD;
                Path = _aStar.CalculatePath(_start, _end);
            }
            SetGhostMinDistance(true);

            if (Path != null && Path.Count > 0)
            {
                return Path[0];
            }
            else
            {
                return null;
            }
        }

        private void SetGhostMinDistance(bool state)
        {
            if (_start != null)
            {
                for (int i = 0; i < _start.neighbors.Length; i++)
                {
                    if (state)
                    {
                        if (_start.neighbors[i].GhostID == null)
                        {
                            _start.neighbors[i].GhostID = _iD;
                        }
                    }
                    else
                    {
                        if (_start.neighbors[i].GhostID == _iD)
                        {
                            _start.neighbors[i].GhostID = null;
                        }
                    }
                }
            }
        }
    }
}