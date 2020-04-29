//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class NewAI : MonoBehaviour
//{
//    [SerializeField] private GameObject target = null;
//    [SerializeField] private GameObject area = null;

//    private GridGenerator grid = null;
//    private Rigidbody rb;
//    private Node start;
//    private Node end;
//    private AStarAlgorithm AStar;

//    private int pathN = 0;
//    private List<Vector3> path = new List<Vector3>();
//    private bool canMove = false;

//    LineRenderer line;
//    // Start is called before the first frame update
//    private void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//        grid = area.GetComponent<GridGenerator>();
//        AStar = new AStarAlgorithm();
//    }

//    // Update is called once per frame
//    private void Update()
//    {
//        if (Input.GetKey(KeyCode.Space))
//        {
//            canMove = true;
//            pathN = 0;
//        }

//        if (start != null)
//        {
//            SetGhostMinDistance(false);
//        }

//        start = grid.GetClosestNode(transform.position);

//        end = grid.GetClosestNode(target.transform.position);

//        path = AStar.CalculatePath(start, end);

//        SetGhostMinDistance(true);

//        if (canMove)
//        {
//            rb.MovePosition(Vector3.MoveTowards(transform.position, path[pathN],
//                Time.deltaTime * 2));

//            if (Vector3.Distance(transform.position, path[pathN]) < 0.01f &&
//                pathN < path.Count - 1)
//            {
//                pathN++;
//            }
//            else if (pathN >= path.Count)
//                canMove = false;
//        }
//    }
//    private void SetGhostMinDistance(bool state)
//    {
//        for (int i = 0; i < start.neighbors.Length; i++)
//        {
//            if (state)
//            {
//                if (start.neighbors[i].HasGhost == null)
//                {
//                }
//            }
//            else
//            {
//                if (start.neighbors[i].HasGhost == this)
//                {
//                    start.neighbors[i].HasGhost = null;
//                }
//            }
//        }
//    }
//    public void SetupLine(Material DebugMat, bool todo)
//    {
//        StopCoroutine(Debug());
//        Destroy(line);

//        if (todo)
//        {
//            line = gameObject.AddComponent<LineRenderer>();

//            line.sortingLayerName = "Debug";
//            line.sortingOrder = 5;
//            line.positionCount = 1;
//            line.SetPosition(0, start.Position);
//            line.startWidth = 0.05f;
//            line.endWidth = 0.05f;
//            line.useWorldSpace = true;
//            line.material = DebugMat;

//            StartCoroutine(Debug());
//        }
//    }
//    private IEnumerator Debug()
//    {
//        while (true)
//        {
//            line.positionCount = 1;
//            line.SetPosition(0, start.Position);

//            for (int i = 0; i < path.Count; i++)
//            {
//                if (i + 1 < path.Count)
//                {
//                    line.positionCount += 1;
//                    line.SetPosition(line.positionCount - 1, path[i]);
//                }
//            }
//            line.positionCount += 1;
//            line.SetPosition(line.positionCount - 1, end.Position);
//            yield return new WaitForFixedUpdate();
//        }
//    }
//}
