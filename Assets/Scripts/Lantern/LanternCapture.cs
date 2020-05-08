using System.Collections.Generic;
using UnityEngine;

namespace Lantern
{
    public class LanternCapture : MonoBehaviour
    {
        private Collider col;
        private LanternBehaviour lantern;
        private List<IEntity> ignored;

        [SerializeField] private GameObject[] objs = null;

        // Start is called before the first frame update
        private void Start()
        {
            col = GetComponent<Collider>();
            lantern = new LanternBehaviour(objs);
            ignored = new List<IEntity>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                lantern.EmptyLantern();
            }
            if (Input.GetKey(KeyCode.Space))
            {
                lantern.ShowColorsIn();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                IAbility ability = lantern.GetAbility();

                if (ability != null)
                {
                    ability.ActivateAbility();
                    if (ability.HabilityEnded)
                    {
                        lantern.EmptyLantern();
                    }
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("GhostEnemy"))
            {
                IEntity ghost = other.gameObject.GetComponent<IEntity>();

                if (!ignored.Contains(ghost))
                {
                    float catchResistence = (ghost.MaxHp - ghost.Hp) + 40;

                    int captureChance = Random.Range(0, 100);

                    if (captureChance < catchResistence
                        && !lantern.Colors[1].HasValue)
                    {
                        Vector3 vel = -(transform.position -
                            other.transform.position).normalized;

                        vel *= Vector3.Distance(transform.position,
                            other.transform.position) -
                            Vector3.Distance(col.bounds.max,
                            transform.position) * 4;

                        other.attachedRigidbody.velocity += vel *
                            Time.fixedDeltaTime;

                        if (Vector3.Distance(other.transform.position,
                            transform.position) < 0.4f &&
                            !lantern.Colors[1].HasValue)
                        {
                            lantern.StoreColor(ghost.GColor);

                            Destroy(other.gameObject);
                            ignored.Add(ghost);
                        }
                    }
                }
            }
        }
    }
}