using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private Camera         _mainCamera;
    [SerializeField] private AudioClip      _stepsSound;
    [SerializeField] private AudioClip      _armorSound;
    [SerializeField] private AudioSource    _stepsSource;
    [SerializeField] private AudioSource    _armorSource;
    [SerializeField] private float          _verticalVelocity;
    private float                           _speed;
    private float                           _rotationSpeed;
    private float                           _inputX;
    private float                           _inputZ;
    private float                           _gravity;
    private float                           _jumpForce;
    private Vector3                         _moveDirection;
    private CharacterController             _cc;
    
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
            _verticalVelocity = -0.1f;

            if (Input.GetButtonDown("Jump"))
            {
                ApplyJump();
                //ArmorSound(_armorSource);
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

        StepsSound(_stepsSource);
        ArmorSound(_armorSource);
    }

    private void StepsSound(AudioSource _stepsSource)
    {
        _stepsSource.clip = _stepsSound;

        if (_cc.isGrounded == true && _stepsSource.isPlaying == false 
            && (_moveDirection.x != 0 || _moveDirection.z != 0))
        {
            _stepsSource.volume = Random.Range(0.3f, 0.4f);
            _stepsSource.pitch = Random.Range(0.7f, 1f);
            _stepsSource.Play();    
        }
    }

    private void ArmorSound(AudioSource _armorSource)
    {
        _armorSource.clip = _armorSound;

        if (_armorSource.isPlaying == false && _cc.isGrounded == true 
            && (_moveDirection.x != 0 || _moveDirection.z != 0))
        {
            _armorSource.volume = Random.Range(0.2f, 0.3f);
            _armorSource.pitch = Random.Range(0.8f, 1.0f);
            _armorSource.Play();             
        }
    }
}
