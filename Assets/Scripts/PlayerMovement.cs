using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float        _speed;
    public float        _jumpHeight;
    public bool         _isGrounded = true;
    private Rigidbody   rb;

    void Start() 
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        PlayerMov();
    }

    void PlayerMov()
    {
        float _horizontalMov = Input.GetAxis("Horizontal");
        float _verticalMov = Input.GetAxis("Vertical");

        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            Vector3 playerMov = 
                new Vector3(_horizontalMov, 0f, _verticalMov).normalized
                * _speed * Time.deltaTime;
            transform.Translate(playerMov, Space.Self);
        }
        
        if (Input.GetButtonDown("Jump") && _isGrounded == true)
        {
            rb.AddForce(new Vector3(0, _jumpHeight, 0), ForceMode.Impulse);
            _isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision) 
    {
        if (collision.gameObject.tag == "Ground")
        {
            _isGrounded = true;
        }
    }
}
