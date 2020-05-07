using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private Camera     _mainCamera;
    [SerializeField] private float      _rotationSpeed;
    [SerializeField] private float      _speed;
    private float                       _inputX;
    private float                       _inputZ;
    private float                       _verticalVelocity;
    private Vector3                     _moveDirection;
    private Vector3                     _jumpVector;
    private CharacterController         _cc;
    
    void Start()
    {
        _speed                          = 5.0f;
        _verticalVelocity               = 5.0f;
        _rotationSpeed                  = 0.1f;
        _mainCamera                     = Camera.main;
        _cc                             = GetComponent<CharacterController>();
    }

    
    void Update()
    {
        PlayerMovementAndRotation();
        Jump(); 
    }

    void Jump()
    {
        Debug.Log(_cc.isGrounded);
        if (Input.GetButtonDown("Jump"))
        {
            _jumpVector = new Vector3 (0, _verticalVelocity, 0);
            _cc.Move(_jumpVector);
        }
        else
        {
            _jumpVector = new Vector3 (0, -_verticalVelocity, 0);
            _cc.Move(_jumpVector);
        }        
    }

    void PlayerMovementAndRotation()
    {
        _inputX             = Input.GetAxis("Horizontal");
        _inputZ             = Input.GetAxis("Vertical");

        var camera          = Camera.main;
        Vector3 forward     = _mainCamera.transform.forward;
        Vector3 right       = _mainCamera.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        _moveDirection = (forward * _inputZ) + (right *_inputX);

        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(_moveDirection), 
            _rotationSpeed);
        
        _cc.Move(_moveDirection *_speed * Time.deltaTime);
    }
}
