using System.Collections;
using UnityEngine;

namespace Lantern.Abilities
{
    public class DeamonDash : MonoBehaviour, IAbility
    {
        [SerializeField] private CharacterController playerRb = null;
        [SerializeField] private float dashSpeed = 25f;
        [SerializeField] private int dashDuration = 10;
        [SerializeField] private int nOfDahses = 3;

        private WaitForSecondsRealtime wait;
        private int timer;
        private bool isDashing;

        public (GhostColor, GhostColor) AbilityColors
        {
            get => (GhostColor.Red, GhostColor.Red);
        }

        public bool HabilityEnded { get; private set; }

        private void Start()
        {
            wait = new WaitForSecondsRealtime(0.5f);
            HabilityEnded = false;
            isDashing = false;
            timer = 0;
        }

        public void ActivateAbility()
        {
            HabilityEnded = false;
            if (!isDashing)
            {
                StartCoroutine(Dash());

                Debug.Log("Dashing in the 90's");
            }
            if (nOfDahses <= 0)
            {
                HabilityEnded = true;
                isDashing = false;
                timer = 0;
            }
        }

        private IEnumerator Dash()
        {
            isDashing = true;
            nOfDahses--;
            while (timer < dashDuration)
            {
                playerRb.Move(playerRb.transform.forward *
                    dashSpeed * Time.deltaTime);
                timer++;
                yield return null;
            }
            yield return wait;
            isDashing = false;
            timer = 0;
        }
    }
}