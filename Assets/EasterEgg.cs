using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEgg : MonoBehaviour
{
    private GameObject _bobby;
    private string _combination;
    private const string _wantedString = "bobby";

    // Start is called before the first frame update
    void Start()
    {
        _bobby = transform.GetChild(0).gameObject;
        _bobby.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        _combination += Input.inputString;

        if (_wantedString.Contains(_combination))
        {
            if (_combination == _wantedString)
            {
                _bobby.SetActive(_bobby.activeSelf ? false : true);
                _combination = null;
            }
        }
        else
        {
            _combination = null;
        }
    }
}
