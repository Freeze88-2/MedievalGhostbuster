using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalItem : MonoBehaviour
{
    [SerializeField] private GameObject _container;
    [SerializeField] private GameObject _content;
    private Vector3 _containerRotation = new Vector3(0, 1.0f, 0);
    private Vector3 _contentRotation = new Vector3(0.5f, 0, 0);

    void Update()
    {
        RotateObjects();               
    }

    private void RotateObjects()
    {
        _container.transform.Rotate(_containerRotation);
        _content.transform.Rotate(_contentRotation);
    }

    IEnumerator WaitForEnd()
    {
        yield return new WaitForSecondsRealtime(3);
        print("Loading scene...");
        SceneManager.LoadScene(0);
    }

    private void OnTriggerEnter(Collider other) 
    {
        print("Game completed...");
        StartCoroutine(WaitForEnd());
    }
}
