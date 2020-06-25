using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _forgeMenu;
    [SerializeField] private Camera  _menuCam;
    [SerializeField] private int     _escCount;
    [SerializeField] private int     _tabCount;

    private bool _isPaused;

    void Start()
    {
        //_menuCam = GetComponentInChildren<Camera>();
        _pauseMenu.SetActive(false);
        _forgeMenu.SetActive(false);
        _menuCam.enabled = false;
        _escCount = 0;
        _tabCount = 0;
        _isPaused = false;
    }

    void Update()
    {
        Pause();
        Forge();
    }

    private void Pause()
    {
        if (Input.GetButtonDown("Pause"))
        {
            _escCount++;
            if (_escCount == 1)
            {
                _pauseMenu.SetActive(true);
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
                _pauseMenu.SetActive(false);
                _isPaused = false;
                Time.timeScale = 1;
                _escCount = 0;
            }
        }
    }

    private void Forge()
    {
        if (Input.GetButtonDown("Forge"))
        {
            _tabCount++;
            if (_tabCount == 1)
            {
                _forgeMenu.SetActive(true);
                _menuCam.enabled = true;
                Time.timeScale = 0;
                _isPaused = true;
            }
            else if (_tabCount == 2)
            {
                _menuCam.enabled = false;
                _forgeMenu.SetActive(false);
                _isPaused = false;
                Time.timeScale = 1;
                _tabCount = 0;
            }
        }
    }
}
