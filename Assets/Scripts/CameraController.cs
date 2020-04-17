using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float _rotationSpeed = 1.0f;
    public Transform CameraRig, Player;
    float _mouseX, _mouseY;

    void Start()
    {
        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate() 
    {
         CamControl();
    }

    void CamControl()
    {
        _mouseX += Input.GetAxis("Mouse X") * _rotationSpeed; 
        _mouseY -= Input.GetAxis("Mouse Y") * _rotationSpeed; 
        _mouseY = Mathf.Clamp(_mouseY, -35, 60);

        transform.LookAt(CameraRig);

        CameraRig.rotation = Quaternion.Euler(_mouseY, _mouseX, 0);
        Player.rotation = Quaternion.Euler(0, _mouseX, 0);
    }
}
