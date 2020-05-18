using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private Camera     _mainCamera;
    private float                       _speed;
    private float                       _rotationSpeed;
    private float                       _inputX;
    private float                       _inputZ;
    private float                       _verticalVelocity;
    private float                       _gravity;
    private float                       _jumpForce;
    private Vector3                     _moveDirection;
    private CharacterController         _cc;
    
    void Start()
    {
        _speed                          = 5.0f;
        _jumpForce                      = 10.0f;
        // _gravity = 20.0f -> Floaty
        // _gravity = 30.0f -> Sharper
        _gravity                        = 30.0f;
        _rotationSpeed                  = 0.3f;
        _mainCamera                     = Camera.main;
        _cc                             = GetComponent<CharacterController>();
    }

    void Update()
    {
        JumpCheck();                 
        PlayerMovementAndRotation();
    }

    void JumpCheck()
    {
        if(_cc.isGrounded)
        {
            _verticalVelocity = 0.0f;

            if (Input.GetButtonDown("Jump"))
            {
                ApplyJump();
            }
        }
        else
        {
            ApplyGravity();
        }          
    }

    void ApplyJump()
    {
        _verticalVelocity = _jumpForce;
    }

    void ApplyGravity()
    {
        _verticalVelocity -= _gravity * Time.deltaTime;
    }

    void PlayerMovementAndRotation()
    {
        _inputX             = Input.GetAxis("Horizontal");
        _inputZ             = Input.GetAxis("Vertical");

        Vector3 forward     = _mainCamera.transform.forward;
        Vector3 right       = _mainCamera.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        _moveDirection = ((forward * _inputZ) + (right *_inputX)).normalized;

        if (_moveDirection.magnitude != 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(_moveDirection), 
                _rotationSpeed);
        }

        _moveDirection.x *= _speed;
        _moveDirection.z *= _speed;
        _moveDirection.y = _verticalVelocity;
        
        _cc.Move(_moveDirection * Time.deltaTime);
    }
}
