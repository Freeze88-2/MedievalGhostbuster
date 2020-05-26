using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftDoor : MonoBehaviour , IPuzzleInteractable
{
    private Vector3 _startTransform;
    private Rigidbody _rb;

    private void Start()
    {
        _startTransform = transform.position;
        _rb = GetComponent<Rigidbody>();
    }

    public void ActivatePuzzlePiece(bool active, float time)
    {
        StopCoroutine(MoveDoor(false));
        StartCoroutine(MoveDoor(active));
    }

    private IEnumerator MoveDoor(bool active)
    {
        if (active)
        {
            Vector3 endPos = transform.position;
            endPos.y += 3f;

            while (transform.position.y < endPos.y)
            {
                _rb.velocity = transform.up * 1f;
                yield return null;
            }
            _rb.velocity = Vector3.zero;
        }
        else
        {
            while (transform.position.y > _startTransform.y)
            {
                _rb.velocity = -transform.up * 1f;
                yield return null;
            }
            _rb.velocity = Vector3.zero;
        }
    }
}
