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
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _light = GetComponent<Light>();
        _initIntensity = _light.intensity;
        _wait = new WaitForSeconds(0.2f);
        StartCoroutine(LightStrenght());
    }

    // Update is called once per frame
    void Update()
    {
        float dis = Vector3.Distance(transform.position, _player.transform.position);

        RaycastHit[] allCols = Physics.RaycastAll(transform.position, _player.transform.position, 30, LayerMask.GetMask("Default"));
        bool canPass = allCols.Length == 0;

        if (dis < 35 && canPass)
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
