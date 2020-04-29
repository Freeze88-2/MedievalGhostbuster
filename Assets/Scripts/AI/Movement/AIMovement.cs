using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;

public class AIMovement : MonoBehaviour
{
    [SerializeField] private GameObject target = null;
    [SerializeField] private GameObject area = null;
    [SerializeField] private float maxSpeed = 1f;

    private AILogic ailogic;
    private LineRenderer line;
    private Rigidbody rb;

    // Use this for initialization
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        ailogic = new AILogic(area.GetComponent<GridGenerator>());
    }

    // This is called every physics update
    private void FixedUpdate()
    {
        Vector3? nextPoint = ailogic.GetPoint(this.gameObject, target);

        if (nextPoint.HasValue)
        {
            Vector3 dir = transform.position - nextPoint.Value;
            dir.y = 0;

            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation(dir), Time.unscaledDeltaTime * 2f);

            rb.velocity = -transform.forward * maxSpeed;
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
            line.SetPosition(0, transform.position);
            line.startWidth = 0.05f;
            line.endWidth = 0.05f;
            line.useWorldSpace = true;

            StartCoroutine(DebugLine());
        }
    }

    private IEnumerator DebugLine()
    {
        while (true)
        {
            line.positionCount = 1;
            line.SetPosition(0, transform.position);

            for (int i = 0; i < ailogic.path.Count; i++)
            {
                if (i + 1 < ailogic.path.Count)
                {
                    line.positionCount += 1;
                    line.SetPosition(line.positionCount - 1, ailogic.path[i]);
                }
            }
            line.positionCount += 1;
            line.SetPosition(line.positionCount - 1, target.transform.position);
            yield return new WaitForFixedUpdate();
        }
    }
}