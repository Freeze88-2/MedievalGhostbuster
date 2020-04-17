using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float _speed;

    void Update()
    {
        PlayerMov();
    }

    void PlayerMov()
    {
        float _horizontalMov = Input.GetAxis("Horizontal");
        float _verticalMov = Input.GetAxis("Vertical");

        Vector3 playerMov = 
            new Vector3(_horizontalMov, 0f, _verticalMov).normalized
            * _speed * Time.deltaTime;
        transform.Translate(playerMov, Space.Self);
    }
}
