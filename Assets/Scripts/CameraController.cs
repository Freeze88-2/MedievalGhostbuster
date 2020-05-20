using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera         _mainCamera;
    [SerializeField] private Camera         _leftShoulderCamera;
    [SerializeField] private Camera         _rightShoulderCamera;
    [SerializeField] private Transform      _player;
    [SerializeField] private bool           _drawGizmos = true;
    private CameraType                      _currentActiveCamera;    
    private float                           _rotationSpeed;
    private float                           _mouseX, _mouseY;   
    private float                           _smooth;
    private float                           _range;
    private int                             _playerLayer;
    private const string                    PLAYER_LAYER = "Player";

    // Change Y_OFFSET according to model (0.9 for capsule, 0.4 for skeleton)
    // This applies to cameraRig Y position (same values)
    private const float                     Y_OFFSET = 0.4f;

    private RaycastHit                      _cullingHit;
    private bool FireState                  
        => Input.GetMouseButton(1);
    private bool AltPressed                 
        => Input.GetKeyDown(KeyCode.LeftAlt);

    private Vector3 TargetPosition 
        => _player.position + new Vector3(0, Y_OFFSET, 0);

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        _rotationSpeed                  = 1.0f;
        _smooth                         = 3.0f;
        _mainCamera.enabled             = true; 
        _leftShoulderCamera.enabled     = false; 
        _rightShoulderCamera.enabled    = false;
        _playerLayer                    = LayerMask.NameToLayer(PLAYER_LAYER);
        _range         = Vector3.Distance(TargetPosition, transform.position);
        ChangeCameras(CameraType.Main);
    }

    void LateUpdate() 
    {
        CamControl();
        CamRigUpdate();
    }

    private void FixedUpdate() 
    {
        if (!Physics.Linecast 
            (TargetPosition, TargetPosition +  (-transform.forward * _range)
            , out _cullingHit))
        {
            _cullingHit = default; 
        }
    }
    
    void ChangeCameras(CameraType camToActivate)
    {
        _currentActiveCamera = camToActivate;

        switch (camToActivate)
        {
            case CameraType.None:
                ToggleCamera(null);
                break;
            case CameraType.Main:
                ToggleCamera(_mainCamera);
                break;
            case CameraType.LeftShoulder:
                ToggleCamera(_leftShoulderCamera);
                break;
            case CameraType.RightShoulder:
                ToggleCamera(_rightShoulderCamera);
                break;
        }        
    }
    
    void ToggleCamera(Camera cam)
    {
        _mainCamera.enabled                 = (cam == _mainCamera);
        _rightShoulderCamera.enabled        = (cam == _rightShoulderCamera);
        _leftShoulderCamera.enabled         = (cam == _leftShoulderCamera);
    }

    private void CamControl()
    {   
        if (FireState)
        {
            // Toggle default camera
            if(_currentActiveCamera == CameraType.Main)
            {
                ChangeCameras(CameraType.RightShoulder);
            }
            // Player wants to change
            else if (AltPressed)
            {
                // Swap the cams
                switch (_currentActiveCamera)
                {
                    case CameraType.LeftShoulder:
                        ChangeCameras(CameraType.RightShoulder);
                        break;
                    case CameraType.RightShoulder:
                    default:
                        ChangeCameras(CameraType.LeftShoulder);
                        break;                
                }
            }
            _player.rotation = Quaternion.Euler(0, _mouseX, 0);
        }
        else if (_currentActiveCamera != CameraType.Main)
        {
            ChangeCameras(CameraType.Main);
        }
    }

    // Main cam collision check
    private void CamRigUpdate()
    {
        UpdateRotation();
        UpdatePosition();        
    }

    void UpdateRotation()
    {
        _mouseX += Input.GetAxis("Mouse X") * _rotationSpeed; 
        _mouseY -= Input.GetAxis("Mouse Y") * _rotationSpeed; 
        _mouseY = Mathf.Clamp(_mouseY, -35, 60);

        transform.rotation = Quaternion.Euler(_mouseY, _mouseX, 0);
    }

    void UpdatePosition()
    {
        Vector3 desiredCameraPos = default;
        
        if (_cullingHit.collider == null)
        {
            desiredCameraPos = TargetPosition + (-transform.forward * _range);
        }
        else
        {
            desiredCameraPos = TargetPosition + (-transform.forward * 
                Vector3.Distance(_cullingHit.point, TargetPosition));
            desiredCameraPos = Vector3.Lerp(
                desiredCameraPos, 
                transform.position,
                Time.deltaTime * _smooth);
        }

        transform.position = desiredCameraPos;
    }

    void OnDrawGizmos() 
    {
        if (!_drawGizmos) return;

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(TargetPosition, 
            TargetPosition + (-transform.forward * _range));
        Gizmos.DrawSphere(TargetPosition, 0.02f);
    }
}
