using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonController : MonoBehaviour
{
    [SerializeField] private int        _maxIndex;

    private bool                        _keyDown;
    
    public int                          index;
    public AudioSource                  audioSource;

    void Start()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Vertical") != 0)
        {
            if(!_keyDown)
            {
                if(Input.GetAxis("Vertical") < 0)
                {
                    if(index < _maxIndex)
                    {
                        index++;
                    }
                    else
                    {
                        index = 0;
                    }
                }
                else if(Input.GetAxis("Vertical") > 0)
                {
                    if(index > 0)
                    {
                        index--;
                    }
                    else
                    {
                        index = _maxIndex;
                    }
                }
                _keyDown = true;
            }
        }
        else
        {
            _keyDown = false;
        }
    }
}
