using System.Collections;
using UnityEngine;

namespace Lantern
{
    public class LanternThrow : MonoBehaviour
    {
        [SerializeField] private int _secondsToDespawn = 3;
        [SerializeField] private float _maxDistance = 15f;
        [SerializeField] private Transform _player = null;
        [SerializeField] private GameObject _cursor = null;

        private GameObject _capturer;
        private Rigidbody _lanternRB;
        private float _activeTime;
        private LanternBehaviour behaviour;
        private LineRenderer _line;

        // Start is called before the first frame update
        private void Start()
        {
            _line = GetComponent<LineRenderer>();
            _lanternRB = GetComponent<Rigidbody>();
            _capturer = GameObject.Find("Lantern_Object");
            _activeTime = 0;
            behaviour = _capturer.GetComponentInChildren<LanternCapture>().lantern;
            _capturer.SetActive(false);
        }

        // Update is called once per frame
        private void Update()
        {
            CheckLanternDespawn();
            LanternThrowing();
            AbilityCast();
        }

        private void CheckLanternDespawn()
        {
            if (_capturer.activeSelf)
            {
                _activeTime += Time.deltaTime;
            }
            else
            {
                _activeTime = 0;
            }

            if ((int)_activeTime % 60 > _secondsToDespawn)
            {
                _capturer.SetActive(false);
            }
        }

        private void LanternThrowing()
        {
            if (Camera.current != Camera.main)
            {
                Ray camRay = new Ray(_player.position, Camera.main.transform.forward);

                if (Physics.Raycast(camRay, out RaycastHit hit, 100f, LayerMask.GetMask("Default")))
                {
                    if (hit.distance < _maxDistance)
                    {
                        _line.enabled = true;
                        _cursor.SetActive(true);

                        Vector3 calcVel = ThrowLantern(hit.point, _player.position, Vector3.Distance(_player.transform.position, hit.point) / 5);
                        DrawPath(calcVel, Vector3.Distance(_player.transform.position, hit.point) / 5);

                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            if (!_capturer.activeSelf)
                            {
                                _capturer.SetActive(true);
                                transform.position = _player.position;

                                _lanternRB.velocity = calcVel;
                            }
                        }
                    }
                    else
                    {
                        _line.enabled = false;
                        _cursor.SetActive(false);
                    }
                }
            }
        }

        private Vector3 ThrowLantern(Vector3 target, Vector3 start, float time)
        {
            Vector3 dis = target - start;
            Vector3 disX = dis;
            dis.y = 0f;

            float sy = dis.y;
            float sx = disX.magnitude;

            float velocityX = sx / time;
            float velocityY = sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

            Vector3 final = disX.normalized;

            final *= velocityX;
            final.y = velocityY;

            return final;
        }
        private void DrawPath(Vector3 sim, float speed)
        {
            _line.positionCount = 101;
            _line.SetPosition(0, _player.position);

            for (int i = 1; i <= 100; i++)
            {
                float simtime = i / (float)30 * speed;
                Vector3 simss = (sim * simtime + Vector3.up * Physics.gravity.y * simtime * simtime / 2f);
                Vector3 point = _player.position + simss;

                _line.SetPosition(i, point);

                if (Physics.CheckSphere(point, 0.3f,
                    LayerMask.GetMask("Default")))
                {
                    _cursor.transform.position = point;
                }
            }
        }
        private void AbilityCast()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                behaviour.EmptyLantern();
            }
            if (Input.GetKey(KeyCode.Space))
            {
                behaviour.ShowColorsIn();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                IAbility ability = behaviour.GetAbility();

                if (ability != null)
                {
                    ability.ActivateAbility();
                    StartCoroutine(CheckAbilityStatus(ability));
                }
            }
        }

        private IEnumerator CheckAbilityStatus(IAbility ability)
        {
            while (!ability.HabilityEnded)
            {
                yield return null;
            }
            behaviour.EmptyLantern();
        }
    }
}