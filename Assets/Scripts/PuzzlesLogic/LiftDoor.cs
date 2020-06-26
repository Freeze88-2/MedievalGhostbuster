using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftDoor : MonoBehaviour, IPuzzleInteractable
{
    [SerializeField] private float _height = 3.0f;
    private float _startTransform;
    private float _endTransform;
    // private Rigidbody _rb;

    private void Start()
    {
        _endTransform = 0;
        _startTransform = 0;
        // _rb = GetComponent<Rigidbody>();
    }

    public void ActivatePuzzlePiece(bool active, float time)
    {
        StopCoroutine(MoveDoor(active, time));
        StartCoroutine(MoveDoor(active, time));
    }

    private IEnumerator MoveDoor(bool active, float time)
    {
        if (active)
        {
            if (_endTransform == 0)
            {
                _endTransform = transform.position.y + _height; 
            }

            while (transform.position.y < _endTransform)
            {
                transform.position = (transform.position + transform.up *
                    Time.deltaTime * time);
                yield return null;
            }
            // _rb.velocity = Vector3.zero;
        }
        else
        {
            if (_startTransform == 0)
            {
                _startTransform = transform.position.y - _height;
            }

            while (transform.position.y > _startTransform)
            {
                transform.position = (transform.position + -transform.up *
                    Time.deltaTime * time);
                yield return null;
            }
            // _rb.velocity = Vector3.zero;
        }
    }
}
