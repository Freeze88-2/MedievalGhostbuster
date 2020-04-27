using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    [SerializeField] private GameObject target = null;
    [SerializeField] private GameObject area = null;
    [SerializeField] private float maxAccel = 1f;
    [SerializeField] private float maxRotation = 1f;
    [SerializeField] private float maxSpeed = 1f;
    [SerializeField] private float maxAngularAccel = 1f;

    private GridGenerator grid = null;
    private Node start;
    private Node end;
    private AStarAlgorithm AStar;

    private int pathN = 0;

    private LineRenderer line;

    // The agent's rigid body
    private Rigidbody rb;

    //private AIlookRotation steeringBehaviour;
    public float MaxAngularAccel => maxAngularAccel;
    public Vector3 AngularVelocity => rb.angularVelocity;

    // Use this for initialization
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        grid = area.GetComponent<GridGenerator>();
        AStar = new AStarAlgorithm();
        //steeringBehaviour = GetComponent<AIlookRotation>();
    }

    // This is called every physics update
    private void FixedUpdate()
    {
        Vector3? nextPoint = GetPoint();

        Vector3 dir = transform.position - nextPoint.Value;
        dir.y = 0;
        transform.rotation = Quaternion.Lerp(transform.rotation,
                                             Quaternion.LookRotation(dir),
                                             Time.unscaledDeltaTime * 3f);

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
        if (pathN >= path.Count)
        {
            pathN = 0;
        }
        if (Vector3.Distance(transform.position, path[pathN]) < 0.01f &&
            pathN < path.Count - 1)
        {
            pathN++;
            return path[pathN];
        }
        else
        {
            return path[pathN];
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

    public void SetupLine(Material DebugMat, bool todo)
    {
        StopCoroutine(Debug());
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
            line.material = DebugMat;

            StartCoroutine(Debug());
        }
    }
    List<Vector3> debugPath;
    private IEnumerator Debug()
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