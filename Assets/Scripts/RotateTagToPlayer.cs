using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTagToPlayer : MonoBehaviour
{
    private GameObject _cam;

    // Start is called before the first frame update
    void Start()
    {
        _cam = GameObject.FindGameObjectWithTag("MainCamera");  
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(_cam.transform.position);
    }
}
