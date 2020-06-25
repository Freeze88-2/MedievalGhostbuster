using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    [SerializeField] private Transform[] _savedObjects;

    private const KeyCode SAVE_KEY = KeyCode.F3;   
    private const KeyCode LOAD_KEY = KeyCode.F4;

    private Vector3 _savedPosition;

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(Input.GetKeyDown(SAVE_KEY))
            SaveGame();
        else if (Input.GetKeyDown(LOAD_KEY))
            LoadGame();
    }

    void SaveGame()
    {
        for (int i = 0; i < _savedObjects.Length; i++)
        {
            _savedPosition = transform.localPosition;
            print(_savedPosition);
        }
        
    }

    void LoadGame()
    {
        for (int i = 0; i < _savedObjects.Length; i++)
        {
            transform.localPosition = _savedPosition;
            print(transform.position);
        }        
    }
}
