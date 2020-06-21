using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftDoor : MonoBehaviour , IPuzzleInteractable
{
    private Vector3 _startTransform;
    private Rigidbody _rb;
    private bool _isRunning;

    private void Start()
    {
        _startTransform = transform.position;
        _rb = GetComponent<Rigidbody>();
    }

    public void ActivatePuzzlePiece(bool active, float time)
    {
        if (!_isRunning)
        {
            StartCoroutine(MoveDoor(active));
        }
    }

    private IEnumerator MoveDoor(bool active)
    {
        _isRunning = true;

        if (active)
        {
            Vector3 endPos = transform.position;
            endPos.y += 3f;

            while (transform.position.y < endPos.y)
            {
                _rb.MovePosition(transform.position + transform.up *
                    Time.deltaTime * 0.5f);
                yield return null;
            }
            _rb.velocity = Vector3.zero;
        }
        else
        {
            while (transform.position.y > _startTransform.y)
            {
                _rb.MovePosition(transform.position + -transform.up *
                    Time.deltaTime * 0.5f);
                yield return null;
            }
            _rb.velocity = Vector3.zero;
        }

        _isRunning = false;
    }
}
