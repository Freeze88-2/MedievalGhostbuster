using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    private Animator            _characterAnim;
    private PlayerMovement      _player;
    private int                 _random;

    private void Start()
    {
        _characterAnim = GetComponentInChildren<Animator>();
        _player = GetComponent<PlayerMovement>();
        _random = 0;
    }

    private void Update()
    {
        AnimChangeOnWalk();
        AnimChangeOnAttack();

        Debug.Log(_random);
    }

    private void AnimChangeOnWalk()
    {
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            _characterAnim.SetBool("IsWalking", false);
        }
        else if (Input.GetAxis("Horizontal") > 0
                || Input.GetAxis("Horizontal") < 0)
        {
            _characterAnim.SetBool("IsWalking", true);
        }
        else if (Input.GetAxis("Vertical") > 0
                || Input.GetAxis("Vertical") < 0)
        {
            _characterAnim.SetBool("IsWalking", true);
        }
    }

    private void AnimChangeOnAttack()
    {        
        if (Input.GetButtonDown("Fire1"))
        {
            _random = Random.Range(0,3);
            _characterAnim.SetInteger("AttackChain", _random);

            _characterAnim.SetTrigger("Attack");
        }
    }
}
