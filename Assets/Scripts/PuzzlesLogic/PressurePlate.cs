using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private float _neededWeight;
    [SerializeField] private float _activeTiming;
    [SerializeField] private GameObject[] _gameObjects;
    [SerializeField] private Direction _dir;

    private IPuzzleInteractable[] _pieces;
    private WaitForSeconds _wait;
    private Vector3 _initialPos;
    private Vector3 _wantedPos;
    private bool _active;
    private GameObject _current;

    // Start is called before the first frame update
    void Start()
    {
        Physics.Raycast(transform.position,
            -transform.up, out RaycastHit hit, 100f,
            LayerMask.GetMask("Default"));

        _wantedPos = hit.point;
        _wantedPos.y -= 0.09f;
        _initialPos = transform.position;
        _wait = new WaitForSeconds(_activeTiming);
        _pieces = new IPuzzleInteractable[_gameObjects.Length];

        for (int i = 0; i < _gameObjects.Length; i++)
        {
            _pieces[i] = _gameObjects[i].GetComponent<IPuzzleInteractable>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_active)
        {
            transform.position = Vector3.Lerp(transform.position, _wantedPos, Time.deltaTime * 2);
        }
        else if (transform.position != _initialPos)
        {
            transform.position = Vector3.Lerp(transform.position, _initialPos, Time.deltaTime * 2);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") 
            || other.attachedRigidbody.mass >= _neededWeight)
        {
            if (!other.CompareTag("GameController"))
            {
                _active = true;
                if (_current == null)
                {
                    _current = other.gameObject;
                    PushButtonDown();
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        _current = null;
        StartCoroutine(PushButtonUp());
    }

    private void PushButtonDown()
    {
        _active = true;
        if (_dir == (Direction)0 || _dir == (Direction)2 )
        {
            DoPuzzleAction(_active);
        }
    }

    private IEnumerator PushButtonUp()
    {
        yield return _wait;

        _active = false;
        if (_dir == (Direction)1 || _dir == (Direction)2)
        {
            DoPuzzleAction(_active);
        }
    }

    private void DoPuzzleAction(bool activate)
    {
        for (int i = 0; i < _pieces.Length; i++)
        {
            _pieces[i].ActivatePuzzlePiece(activate, _activeTiming);
        }
    }
}
