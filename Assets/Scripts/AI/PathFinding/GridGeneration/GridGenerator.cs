using CostumDebug;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.PathFinding.GridGeneration
{
    /// <summary>
    /// Creates the grid where the AI can walk on
    /// </summary>
    public class GridGenerator : MonoBehaviour, IDebug
    {
        // The layer the AI can't walk on
        [SerializeField] private LayerMask unwalkablemask = default;
        // The size of the area
        [SerializeField] private Vector3 areaSize = Vector3.one;
        // The size of the each node
        [SerializeField] private float nodeRadius = 0.3f;

        // Stores all the grid nodes
        private Node[,] _grid;
        // Stores the diamater of the node
        private float nodeDiameter;
        // Stores the size on the X and Z 
        private int gridSizeX, gridSizeY;
        // A line renderer for debugging
        private LineRenderer _line;

        /// <summary>
        /// Called to generate the grid
        /// </summary>
        public void StartGridGeneration()
        {
            // Finds the diameter of the node
            nodeDiameter = nodeRadius * 2;
            // Finds the size on the X of the area
            gridSizeX = (int)(areaSize.x / nodeDiameter);
            // Finds the size on the Z of the area
            gridSizeY = (int)(areaSize.z / nodeDiameter);
            // Creates the grid
            CreateGrid();
        }

        /// <summary>
        /// Update is called once per frame
        /// </summary>
        public void CreateGrid()
        {
            // Creates a new grid witht he given size
            _grid = new Node[gridSizeX, gridSizeY];

            // Finds the position in the world of the bottom corner
            Vector3 bottomCorner = new Vector3(transform.position.x -
                (areaSize.x / 2), transform.position.y, transform.position.z -
                (areaSize.z / 2));

            // Loops for the X and Z size of the square
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    // Finds the world position of the current node
                    Vector3 worldPoint = new Vector3(bottomCorner.x +
                        (x * nodeDiameter + nodeRadius), bottomCorner.y,
                        bottomCorner.z + (y * nodeDiameter + nodeRadius));

                    // Checks if the node will be on a uwalkable place
                    bool walkable = !(Physics.CheckSphere(worldPoint,
                        nodeRadius, unwalkablemask));

                    // Creates a new Node on the X, Y and walkable bool
                    _grid[x, y] = new Node(walkable, worldPoint,
                        new Vector2(x, y));
                }
            }
            // Checks all the grid nodes
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    // Finds the neighbours of that node
                    GetNeighbors(_grid[x, y]);
                }
            }
        }

        /// <summary>
        /// Finds the neighbouring nodes of the node given
        /// </summary>
        /// <param name="node"> Center node </param>
        private void GetNeighbors(Node node)
        {
            // Creates a list of nodes
            List<Node> a = new List<Node>();

            // Searches through all the nodes on the grid
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    // Finds the distance from the current to the given one
                    float distX = Mathf.Abs(Mathf.Abs(_grid[x, y].pos.x) -
                        Mathf.Abs(node.pos.x));
                    float distY = Mathf.Abs(Mathf.Abs(_grid[x, y].pos.y) -
                        Mathf.Abs(node.pos.y));

                    // Checks if the node is adjacent
                    if ((distX == 1 && distY == 1) ||
                        (distX == 1 && distY == 0) ||
                        (distX == 0 && distY == 1))
                    {
                        // Adds that node to list
                        a.Add(_grid[x, y]);
                    }
                    // Checks if the list has the maximum amount of nodes
                    if (a.Count >= 8) break;
                }
            }
            // Transforms that list into an array
            node.neighbors = a.ToArray();
        }

        /// <summary>
        /// Finds the closest node to the world position given
        /// </summary>
        /// <param name="position"> World position to be searched </param>
        /// <returns> The node found </returns>
        public Node GetClosestNode(Vector3 position)
        {
            // Finds the percentage of how far it is from the center x
            float percentX = Mathf.Clamp01((position.x + areaSize.x / 2)
                / areaSize.x);
            // Finds the percentage of how far it is from the center y
            float percentY = Mathf.Clamp01((position.z + areaSize.z / 2)
                / areaSize.z);

            // Converts the percentage into the wanted x
            int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
            // Converts the percentage into the wanted Y
            int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

            // Cheks if the Grid is generated and returns the node if it is
            return _grid != null ? _grid[x, y] : null;
        }

        // I don't want to comment this >.>
        public void RunDebug(bool active)
        {
            StopCoroutine(DebugLine());
            Destroy(_line);

            if (active)
            {
                _line = gameObject.AddComponent<LineRenderer>();

                _line.sortingLayerName = "Debug";
                _line.sortingOrder = 100;
                _line.positionCount = 5;
                _line.startWidth = 0.05f;
                _line.endWidth = 0.05f;
                _line.useWorldSpace = true;

                StartCoroutine(DebugLine());
            }
        }

        private IEnumerator DebugLine()
        {
            while (true)
            {
                _line.positionCount = 5;
                _line.SetPosition(0, new Vector3(transform.position.x -
                (areaSize.x / 2), transform.position.y,
                transform.position.z - (areaSize.z / 2)));

                _line.SetPosition(1, new Vector3(transform.position.x +
                (areaSize.x / 2), transform.position.y,
                transform.position.z - (areaSize.z / 2)));

                _line.SetPosition(2, new Vector3(transform.position.z +
                (areaSize.z / 2), transform.position.y,
                transform.position.x + (areaSize.x / 2)));

                _line.SetPosition(3, new Vector3(transform.position.z -
                (areaSize.z / 2), transform.position.y,
                transform.position.x + (areaSize.x / 2)));

                _line.SetPosition(4, new Vector3(transform.position.x -
                (areaSize.x / 2), transform.position.y,
                transform.position.z - (areaSize.z / 2)));
                yield return new WaitForEndOfFrame();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, areaSize);

            if (_grid != null)
            {
                foreach (Node a in _grid)
                {
                    Gizmos.color = a.Walkable ? a.GhostID != null ? Color.blue : Color.green : Color.red;
                    Gizmos.DrawWireCube(a.Position, new Vector3(1, 0, 1) * nodeDiameter);
                }
            }
        }
    }
}