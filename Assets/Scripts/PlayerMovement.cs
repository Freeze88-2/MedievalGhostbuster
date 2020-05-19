using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("Use MovementController instead")]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float      _speed;
    [SerializeField] private float      _jumpHeight;
    [SerializeField] private float      _rotationSpeed;
    [SerializeField] private float      _gravity;
    private Vector3                     _moveDirection;
    private CharacterController         _cc;

    void Start()
    {
        _moveDirection      = Vector3.zero;
        _cc                 = GetComponent<CharacterController>();
    }

    void Update()
    {
        MovementUpdate();
    }

    private void MovementUpdate()
    {
        if(_cc.isGrounded)
        {
            _moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));
            _moveDirection = transform.TransformDirection(_moveDirection);
            _moveDirection *= _speed;
            
            if(Input.GetButtonDown("Jump"))
            {
                _moveDirection.y = _jumpHeight;
            } 
        }
        else
        {
            _moveDirection = new Vector3(0,
                _moveDirection.y, Input.GetAxis("Vertical"));
            _moveDirection = transform.TransformDirection(_moveDirection);
            _moveDirection.x *= _speed;
            _moveDirection.z *= _speed;
        }

        transform.Rotate(0, Input.GetAxis("Horizontal") * _rotationSpeed, 0);
        _cc.Move(_moveDirection * Time.deltaTime);
        _moveDirection.y -= _gravity * Time.deltaTime;
    }
}
