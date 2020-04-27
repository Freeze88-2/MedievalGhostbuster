using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
public class AIMovement : MonoBehaviour
{
    [SerializeField] private GameObject target = null;
    [SerializeField] private GameObject area = null;
    [SerializeField] private float maxSpeed = 1f;

    private GridGenerator grid = null;
    private Node start;
    private Node end;
    private AStarAlgorithm AStar;

    private int pathN = 0;

    private LineRenderer line;
    private Rigidbody rb;

    // Use this for initialization
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        grid = area.GetComponent<GridGenerator>();
        AStar = new AStarAlgorithm();
    }

    // This is called every physics update
    private void FixedUpdate()
    {
        Vector3? nextPoint = GetPoint();

        Vector3 dir = transform.position - nextPoint.Value;
        dir.y = 0;

        transform.rotation = Quaternion.Lerp(transform.rotation,
            Quaternion.LookRotation(dir), Time.unscaledDeltaTime * 2f);

        rb.velocity = -transform.forward * maxSpeed;
    }

    private Vector3? GetPoint()
    {
        if (start != null)
        {
            SetGhostMinDistance(false);
        }

        start = grid.GetClosestNode(transform.position);
        start.HasGhost = this;

        end = grid.GetClosestNode(target.transform.position);

        List<Vector3> path = AStar.CalculatePath(start, end);
        debugPath = path;
        SetGhostMinDistance(true);

        return GetNextPoint(path);
    }

    private Vector3? GetNextPoint(List<Vector3> path)
    {
        if (path != null)
        {
            if (pathN >= path.Count - 1)
            {
                pathN = 0;
            }
            if (Vector3.Distance(transform.position, path[pathN]) < 0.01f &&
                pathN < path.Count - 1)
            {
                pathN++;

                Debug.Log(pathN);
                Debug.Log(path.Count);
            }
            return path[pathN];
        }
        return transform.position;
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

    public void SetupLine(bool todo)
    {
        StopCoroutine(DebugLine());
        Destroy(line);

        if (todo)
        {
            line = gameObject.AddComponent<LineRenderer>();

            line.sortingLayerName = "Debug";
            line.sortingOrder = 5;
            line.positionCount = 1;
            line.SetPosition(0, start.Position);
            line.startWidth = 0.05f;
            line.endWidth = 0.05f;
            line.useWorldSpace = true;

            StartCoroutine(DebugLine());
        }
    }

    private List<Vector3> debugPath;

    private IEnumerator DebugLine()
    {
        while (true)
        {
            line.positionCount = 1;
            line.SetPosition(0, start.Position);

            for (int i = 0; i < debugPath.Count; i++)
            {
                if (i + 1 < debugPath.Count)
                {
                    line.positionCount += 1;
                    line.SetPosition(line.positionCount - 1, debugPath[i]);
                }
            }
            line.positionCount += 1;
            line.SetPosition(line.positionCount - 1, end.Position);
            yield return new WaitForFixedUpdate();
        }
    }
}