using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAI : MonoBehaviour
{
    [SerializeField] private GameObject target = null;
    [SerializeField] private GameObject area = null;

    private GridGenerator grid = null;
    private Rigidbody rb;
    private Node start;
    private Node end;
    private AStarAlgorithm AStar;

    private int pathN = 0;
    private List<Vector3> path = new List<Vector3>();
    private bool canMove = false;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        grid = area.GetComponent<GridGenerator>();
        AStar = new AStarAlgorithm();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            canMove = true;
            pathN = 0;
        }

        if (start != null)
        {
            SetGhostMinDistance(false);
        }

        start = grid.GetClosestNode(transform.position);
        start.HasGhost = this;

        end = grid.GetClosestNode(target.transform.position);

        path = AStar.CalculatePath(start, end);

        SetGhostMinDistance(true);

        if (canMove)
        {
            rb.MovePosition(Vector3.MoveTowards(transform.position, path[pathN],
                Time.deltaTime * 2));

            if (Vector3.Distance(transform.position, path[pathN]) < 0.01f &&
                pathN < path.Count - 1)
            {
                pathN++;
            }
            else if (pathN >= path.Count)
                canMove = false;
        }
    }
    private void SetGhostMinDistance(bool state)
    {
        for (int i = 0; i < start.neighbors.Length; i++)
        {
            if (state)
            {
                if (start.neighbors[i].HasGhost == null)
                {
                    start.neighbors[i].HasGhost = this;
                }
            }
            else
            {
                if (start.neighbors[i].HasGhost == this)
                {
                    start.neighbors[i].HasGhost = null;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (start != null && path[0] != null)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawLine(start.Position, path[0]);
        }
        for (int i = 0; i < path.Count; i++)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(path[i], 0.1f);
            if (i + 1 < path.Count)
                Gizmos.DrawLine(path[i], path[i + 1]);
        }
    }
}
