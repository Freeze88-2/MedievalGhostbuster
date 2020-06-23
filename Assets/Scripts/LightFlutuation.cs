using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlutuation : MonoBehaviour
{
    private Light _light;
    private WaitForSeconds _wait;
    private float _initIntensity;
    private float _wantedIntesity;
    private GameObject _player;
    private Renderer render;
    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<Renderer>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _light = GetComponent<Light>();
        _initIntensity = _light.intensity;
        _wait = new WaitForSeconds(0.2f);
        StartCoroutine(LightStrenght());
    }

    // Update is called once per frame
    void Update()
    {
        if (render.isVisible)
        {
            _light.enabled = true;
            _light.intensity = Mathf.Lerp(_light.intensity, _wantedIntesity, Time.deltaTime * Random.Range(5, 8));
        }
        else
        {
            _light.enabled = false;
        }
    }

    private IEnumerator LightStrenght()
    {
        while (true)
        {
            _wantedIntesity = Random.Range(_initIntensity - 3, _initIntensity + 3);

            yield return _wait;
        }
    }
}
