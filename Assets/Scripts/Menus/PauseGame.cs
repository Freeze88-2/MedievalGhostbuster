using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    [SerializeField] private int _escCount;
    [SerializeField] private int _tabCount;
    private Camera _menuCam;
    private bool _isPaused;

    void Start()
    {
        _menuCam = GetComponentInChildren<Camera>();
        _menuCam.enabled = false;
        _escCount = 0;
        _tabCount = 0;
        _isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        Pause();
    }

    private void Pause()
    {
        if (Input.GetButtonDown("Pause"))
        {
            _escCount++;
            if (_escCount == 1)
            {
                _menuCam.enabled = true;
                Time.timeScale = 0;
                _isPaused = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else if (_escCount == 2)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                _menuCam.enabled = false;
                _isPaused = false;
                Time.timeScale = 1;
                _escCount = 0;
            }
        }
    }
}
