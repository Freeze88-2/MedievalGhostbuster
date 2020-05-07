using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class LanternCapture : MonoBehaviour
{
    private Collider col;
    private LanternBehaviour lantern;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider>();
        lantern = new LanternBehaviour();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            lantern.EmptyLantern();
        }
        if (Input.GetKey(KeyCode.Space))
        {
            lantern.ShowColorsIn();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("GhostEnemy"))
        {
            Vector3 vel = -(transform.position - other.transform.position);
            vel = vel.normalized;
            vel *= Vector3.Distance(transform.position,
                other.transform.position) - Vector3.Distance(col.bounds.max,
                transform.position) * 4;

            other.attachedRigidbody.velocity += vel * Time.fixedDeltaTime;

            float a = Vector3.Distance(other.transform.position, transform.position);

            if (Vector3.Distance(other.transform.position, transform.position) < 0.4f)
            {
                lantern.StoreColor
                    (other.gameObject.GetComponent<IEntity>().GColor);

                Destroy(other.gameObject);
            }
        }
    }
}
