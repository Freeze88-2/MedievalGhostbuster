using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private Vector3 start;
    [SerializeField] private Vector3 end;
    [SerializeField] private bool moveOnXAxis;

    private Rigidbody rb;
    private GameObject ontopObj;
    private bool somethingOnTop;

    // Start is called before the first frame update
    private void Start()
    {
        start.y = transform.position.y;
        end.y = transform.position.y;

        if (moveOnXAxis)
        {
            start.z = transform.position.z;
            end.z = transform.position.z;
        }
        else
        {
            start.x = transform.position.x;
            end.x = transform.position.x;
        }
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, start) <= 0.5f || Vector3.Distance(transform.position, end) <= 0.5f)
        {
            speed *= -1;
        }

        Vector3 axis = moveOnXAxis ? transform.right : transform.forward;

        rb.MovePosition(transform.position + axis *
            Time.fixedDeltaTime * speed);

        if (somethingOnTop)
        {
            ontopObj.transform.position += rb.velocity / 50;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") ||
            other.gameObject.layer == LayerMask.GetMask("Entity"))
        {
            if (other.gameObject.transform.parent.gameObject != null)
                ontopObj = other.gameObject.transform.parent.gameObject;
            else
                ontopObj = other.gameObject;
            somethingOnTop = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform.parent.gameObject == ontopObj)
        {
            somethingOnTop = false;
        }
    }

    private void OnDrawGizmos()
    {
        start.y = transform.position.y;
        end.y = transform.position.y;

        if (moveOnXAxis)
        {
            start.z = transform.position.z;
            end.z = transform.position.z;
        }
        else
        {
            start.x = transform.position.x;
            end.x = transform.position.x;
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(start, 0.5f);
        Gizmos.DrawSphere(end, 0.5f);
    }
}