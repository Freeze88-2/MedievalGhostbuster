using CostumDebug;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.PathFinding.GridGeneration
{
    public class GridGenerator : MonoBehaviour, IDebug
    {
        [SerializeField] private LayerMask unwalkablemask = default;
        [SerializeField] private Vector3 gridWorldSize = Vector3.one;
        [SerializeField] private float nodeRadius = 0.3f;

        private Node[,] _grid;
        private float nodeDiameter;
        private int gridSizeX, gridSizeY;

        private LineRenderer _line;

        public void StartGridGeneration()
        {
            nodeDiameter = nodeRadius * 2;
            gridSizeX = (int)(gridWorldSize.x / nodeDiameter);
            gridSizeY = (int)(gridWorldSize.z / nodeDiameter);
            CreateGrid();
        }

        // Update is called once per frame
        public void CreateGrid()
        {
            _grid = new Node[gridSizeX, gridSizeY];

            Vector3 bottomCorner = new Vector3(transform.position.x -
                (gridWorldSize.x / 2), transform.position.y, transform.position.z -
                (gridWorldSize.z / 2));

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector3 worldPoint = new Vector3(bottomCorner.x +
                        (x * nodeDiameter + nodeRadius), bottomCorner.y,
                        bottomCorner.z + (y * nodeDiameter + nodeRadius));
                    bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius,
                        unwalkablemask));
                    _grid[x, y] = new Node(walkable, worldPoint, new Vector2(x, y));
                }
            }
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    GetNeighbors(_grid[x, y]);
                }
            }
        }

        private void GetNeighbors(Node node)
        {
            List<Node> a = new List<Node>();

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    float distX = Mathf.Abs(Mathf.Abs(_grid[x, y].pos.x) -
                        Mathf.Abs(node.pos.x));
                    float distY = Mathf.Abs(Mathf.Abs(_grid[x, y].pos.y) -
                        Mathf.Abs(node.pos.y));

                    if ((distX == 1 && distY == 1) ||
                        (distX == 1 && distY == 0) ||
                        (distX == 0 && distY == 1))
                    {
                        a.Add(_grid[x, y]);
                    }
                }
            }
            node.neighbors = a.ToArray();
        }

        public Node GetClosestNode(Vector3 position)
        {
            float percentX = Mathf.Clamp01((position.x + gridWorldSize.x / 2)
                / gridWorldSize.x);
            float percentY = Mathf.Clamp01((position.z + gridWorldSize.z / 2)
                / gridWorldSize.z);

            int x = Mathf.RoundToInt((gridSizeX -1) * percentX);
            int y = Mathf.RoundToInt((gridSizeY -1) * percentY);

            if (_grid != null)
            {
                return _grid[x, y];
            }

            else
            {
                return null;
            }
        }

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
                (gridWorldSize.x / 2), transform.position.y,
                transform.position.z - (gridWorldSize.z / 2)));

                _line.SetPosition(1, new Vector3(transform.position.x +
                (gridWorldSize.x / 2), transform.position.y,
                transform.position.z - (gridWorldSize.z / 2)));

                _line.SetPosition(2, new Vector3(transform.position.z +
                (gridWorldSize.z / 2), transform.position.y,
                transform.position.x + (gridWorldSize.x / 2)));

                _line.SetPosition(3, new Vector3(transform.position.z -
                (gridWorldSize.z / 2), transform.position.y,
                transform.position.x + (gridWorldSize.x / 2)));

                _line.SetPosition(4, new Vector3(transform.position.x -
                (gridWorldSize.x / 2), transform.position.y,
                transform.position.z - (gridWorldSize.z / 2)));
                yield return new WaitForEndOfFrame();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, gridWorldSize);

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