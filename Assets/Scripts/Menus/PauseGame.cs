using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    [SerializeField] private int _escCount;
    [SerializeField] private int _tabCount;
    private Camera _menuCam;

    void Start()
    {
        _menuCam = GetComponentInChildren<Camera>();
        _menuCam.enabled = false;
        _escCount = 0;
        _tabCount = 0;
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
            }
            else if (_escCount == 2)
            {
                _menuCam.enabled = false;
                Time.timeScale = 1;
                _escCount = 0;
            }
        }
    }
}
