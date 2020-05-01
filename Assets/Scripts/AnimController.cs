using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    [SerializeField] private Animator       _characterAnim;
    [SerializeField] private Animator       _lanternAnim;
    private int                             _random;

    private void Start()
    {
       // _characterAnim = GetComponentInChildren<Animator>();
        //_lanternAnim = GetComponentInChildren<Animator>();;
        _random = 0;
        
    }

    private void Update()
    {
        AnimChangeOnWalk();
        AnimChangeOnAttack();
        AnimChangeOnBlock();
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
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                _random = Random.Range(0,3);
                _characterAnim.SetInteger("AttackChain", _random);

                _characterAnim.SetTrigger("Attack");
            }
        }
    }

    private void AnimChangeOnBlock()
    {
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                _characterAnim.SetTrigger("Block");
                _lanternAnim.SetTrigger("Rotate");
            }
        }
    }
}
