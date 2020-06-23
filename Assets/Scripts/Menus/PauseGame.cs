using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    [SerializeField] private int escCount;
    [SerializeField] private int tabCount;
    private Camera menuCam;

    void Start()
    {
        menuCam = GetComponentInChildren<Camera>();
        menuCam.enabled = false;
        escCount = 0;
        tabCount = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Pause()
    {
        if (Input.GetButtonUp("Pause"))
        {
            escCount++;
            if (escCount == 1)
            {
                menuCam.enabled = true;
                Time.timeScale = 0;
            }
            else if (escCount == 2)
            {
                menuCam.enabled = false;
                Time.timeScale = 1;
                escCount = 0;
            }
        }
    }
}
