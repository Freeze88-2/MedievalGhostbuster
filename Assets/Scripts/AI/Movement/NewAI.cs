using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAI : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] GameObject area;
    GridGenerator grid;

    private Rigidbody rb;
    Node start;
    Node end;
    AStarAlgorithm AStar;
    int pathN = 0;
    List<Vector3> path = new List<Vector3>();
    bool canMove = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        grid = area.GetComponent<GridGenerator>();
        AStar = new AStarAlgorithm();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            canMove = true;
            pathN = 0;
        }

            start = grid.GetClosestNode(transform.position);
            end = grid.GetClosestNode(target.transform.position);

            path = AStar.CalculatePath(start, end);

        if (canMove)
        {
            rb.MovePosition(path[pathN]);

            if (GetDistace(transform.position, path[pathN]) < 0.1f && pathN < path.Count -1)
            {
                pathN++;
            }
            else if (pathN >= path.Count -1)
                canMove = false;
        }
    }
    private float GetDistace(Vector3 A, Vector3 B)
    {
        // Distance formula (square root of ((x-x^2) + (y-y^2)))
        return (Mathf.Sqrt(Mathf.Pow(Mathf.Abs(A.x - B.x), 2) +
         Mathf.Pow(Mathf.Abs(A.y - B.y), 2)));
    }
    private void OnDrawGizmos()
    {
        foreach (Vector3 p in path)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(p, 0.1f);
        }
    }
}
