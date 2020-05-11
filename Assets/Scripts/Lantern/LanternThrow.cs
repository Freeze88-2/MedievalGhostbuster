using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lantern
{
    public class LanternThrow : MonoBehaviour
    {
        [SerializeField] private GameObject lanternMain;
        [SerializeField] private GameObject capturer;
        [SerializeField] private int _secondsToDespawn = 3;

        private Rigidbody _lanternRB;
        private float _activeTime;
        private LanternBehaviour behaviour;

        // Start is called before the first frame update
        void Start()
        {
            _lanternRB = lanternMain.GetComponent<Rigidbody>();
            behaviour = capturer.GetComponentInChildren<LanternCapture>().lantern;
            _activeTime = 0;
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
            if (capturer.activeSelf)
            {
                _activeTime += Time.deltaTime;
            }
            else
            {
                _activeTime = 0;
            }

            if ((int)_activeTime % 60 > _secondsToDespawn)
            {
                capturer.SetActive(false);
            }
        }

        private void LanternThrowing()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (!capturer.activeSelf)
                {
                    capturer.SetActive(true);
                    lanternMain.transform.position = transform.position;

                    _lanternRB.velocity = Camera.main.transform.forward * 10f;
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
            while(!ability.HabilityEnded)
            {
                yield return null;
            }
            behaviour.EmptyLantern();
        }
    }
}