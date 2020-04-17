using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float        _rotationSpeed;
    public Transform    CameraRig, Player;
    private float       _mouseX, _mouseY;
    private float       _minDistance;    
    private float       _maxDistance;   
    private float       _smooth;   
    private Vector3     _dollyDir;
    private Vector3     _dollyDirAdjusted;
    private float       _range; 

    void Start()
    {
        _rotationSpeed      = 1.0f;
        _minDistance        = 1.0f; 
        _maxDistance        = 4.5f; 
        _smooth             = 10.0f;
        _dollyDir           = transform.localPosition.normalized; 
        _range              = transform.localPosition.magnitude; 
    }

    void Update() 
    {
        CamControl();
        CamCollision();
    }

    private void CamControl()
    {
        _mouseX += Input.GetAxis("Mouse X") * _rotationSpeed; 
        _mouseY -= Input.GetAxis("Mouse Y") * _rotationSpeed; 
        _mouseY = Mathf.Clamp(_mouseY, -35, 60);

        transform.LookAt(CameraRig);

        CameraRig.rotation = Quaternion.Euler(_mouseY, _mouseX, 0);
    }

    private void CamCollision()
    {
        Vector3 desiredCameraPos = 
            transform.parent.TransformPoint(_dollyDir * _maxDistance);
        RaycastHit hit;

        if (Physics.Linecast 
            (transform.parent.position, desiredCameraPos, out hit))
        {
            _range = Mathf.Clamp(hit.distance, _minDistance, _maxDistance);
        }
        else
        {
            _range = _maxDistance;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition,
            _dollyDir * _range, Time.deltaTime * _smooth);
    }
}
