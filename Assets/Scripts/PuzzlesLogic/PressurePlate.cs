using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private float _neededWeight;
    [SerializeField] private float _activeTiming;
    [SerializeField] private float _speed;
    [SerializeField] private GameObject[] _gameObjects;
    [SerializeField] private Direction[] _dirs;

    private (IPuzzleInteractable piece, Direction dir)[] _pieces;
    private WaitForSeconds _wait;
    private Vector3 _initialPos;
    private Vector3 _wantedPos;
    private bool _active;
    private GameObject _current;
    private AudioSource _audio;

    // Start is called before the first frame update
    void Start()
    {
        Physics.Raycast(transform.position,
            -transform.up, out RaycastHit hit, 10f);

        _active = false;
        _wantedPos = hit.point;
        _wantedPos.y -= 0.09f;
        _initialPos = transform.position;
        _wait = new WaitForSeconds(_activeTiming);
        _pieces = new (IPuzzleInteractable, Direction)[_gameObjects.Length];
        _audio = GetComponent<AudioSource>();

        for (int i = 0; i < _gameObjects.Length; i++)
        {
            _pieces[i].piece = _gameObjects[i].GetComponent<IPuzzleInteractable>();
            _pieces[i].dir = _dirs[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_active)
        {
            transform.position = Vector3.Lerp(transform.position, _wantedPos,
                Time.deltaTime * 2);
        }
        else if (transform.position != _initialPos)
        {
            transform.position = Vector3.Lerp(transform.position, _initialPos,
                Time.deltaTime * 2);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_current == null)
        {
            if (other.gameObject.CompareTag("Player")
            || other.attachedRigidbody.mass >= _neededWeight)
            {
                _current = other.gameObject;

                PushButtonDown();
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
        if (!_active)
        {
            _active = true;

            _audio.volume = Random.Range(0.5f, 1);
            _audio.pitch = Random.Range(0.5f, 1);
            _audio.Play();

            for (int i = 0; i < _pieces.Length; i++)
            {
                if ((_pieces[i].dir == (Direction)0 ||
                    _pieces[i].dir == (Direction)2))
                {
                    _pieces[i].piece.ActivatePuzzlePiece(_active, _speed);
                }
            }
        }
    }

    private IEnumerator PushButtonUp()
    {
        yield return _wait;

        _active = false;

        _audio.volume = Random.Range(0.5f, 1);
        _audio.pitch = Random.Range(0.5f, 1);
        _audio.Play();

        for (int i = 0; i < _pieces.Length; i++)
        {
            if (_pieces[i].dir == (Direction)1 ||
                _pieces[i].dir == (Direction)2)
            {
                _pieces[i].piece.ActivatePuzzlePiece(_active, _speed);
            }
        }
    }

    private void OnDrawGizmos()
    {
        foreach (GameObject a in _gameObjects)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, a.transform.position);
        }
    }
}
